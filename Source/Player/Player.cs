using Godot;
using System;
using System.Linq;

public partial class Player : CharacterBody2D
{
	public enum State
	{
		IDLE,
		MOVING,
        ATTACKING,
    }

	[Signal]
	public delegate void SuccessfulHitEventHandler();

	[Signal]
	public delegate void ItemPickedEventHandler(ItemBlueprint blueprint, int quantity);

	[Signal]
	public delegate void AttackedEventHandler(HitPayload payload);

	[Signal]
	public delegate void DamagedEventHandler(HitPayload payload);

	public bool ShadowsActive { get; set; } = false;

	public Life Life { get; init; } = new(5.0);
	public Inventory Inventory { get; } = new();
	public Experience Experience { get; } = new();
	public PlayerBody Body => _body;
	public Vector2 Direction { get; set; }
    public State CurrentState { get => currentState; set => currentState = value; }
    public CharacterSprite AnimatedSprite { get; private set; }
    public Area2D HitArea { get => hitArea; }
    public Area2D ForwardSlashArea { get => forwardSlashArea; }
    public float LastDirectionHorizontal { get => lastDirectionHorizontal; set => lastDirectionHorizontal = value; }

    public const float Speed = 100.0f;
	public const float JumpVelocity = -175.0f;

	private readonly PlayerBody _body = new();
	private PlayerCamera camera;
	private StateManager stateManager;

	private Timer shadowTimer = new()
	{
		OneShot = false,
		Autostart = true,
	};

	private State currentState = State.IDLE;

	private Area2D hitArea;
	private Area2D forwardSlashArea;
	private Area2D itemArea;

	private EntitySoundPlayer entitySoundPlayer;

	private float lastDirectionHorizontal = 1.0f;

	private ItemBlueprint shortSwordBlueprint;

	private PackedScene bigSlashPacked;
	private PackedScene shadowPacked;

	private readonly string[] attackAnimations = [
		"naked_attack_sword1",
		"naked_attack_sword2"
	];

	private int attackAnimationIndex = 0;

	public override void _Ready()
	{
		bigSlashPacked = GD.Load<PackedScene>("res://Scenes/big_slash.tscn");
		shadowPacked = GD.Load<PackedScene>("res://Scenes/shadow.tscn");

		ItemDB itemDB = GetNode<ItemDB>("/root/ItemDB");
		ItemBlueprint apple = itemDB.Retrieve("APPLE");
		shortSwordBlueprint = GD.Load<ItemBlueprint>("res://Resources/Items/short_sword.tres");
		AnimatedSprite = GetNode<CharacterSprite>("AnimatedSprite2D");
		hitArea = GetNode<Area2D>("HitArea");
		forwardSlashArea = GetNode<Area2D>("ForwardSlashArea");
		entitySoundPlayer = GetNode<EntitySoundPlayer>("EntitySoundPlayer");
		itemArea = GetNode<Area2D>("ItemArea");
		camera = GetNode<PlayerCamera>("Camera2D");
		AnimatedSprite.AnimationFinished += () =>
		{
			if (IsAttackAnimation())
			{
				CurrentState = State.IDLE;
			}
		};

		AnimatedSprite.FrameChanged += () =>
		{
			if (IsAttackAnimation())
			{
				if (AnimatedSprite.Frame == 2) entitySoundPlayer.PlayAttackSound(AttackType.SLASH);
				else if (AnimatedSprite.Frame == 5) Attack();
			}
		};

		itemArea.BodyEntered += (body) =>
		{
			if (body is DroppableItem droppableItem)
			{
				var item = droppableItem.CurrentItem;
				body.QueueFree();
				EmitSignal(SignalName.ItemPicked, item.Blueprint, item.Quantity);
			}
		};

		_body.Equip(PlayerBody.ItemType.WEAPON, shortSwordBlueprint);

		Inventory.Add(shortSwordBlueprint, 1);
		Inventory.Add(shortSwordBlueprint, 1);
		Inventory.Add(apple, 1);

		Life.Death += OnDeath;

		stateManager = new ();
		AddChild(stateManager);
		stateManager.ChangeState(new PlayerDefaultState(new (this)));

		AnimatedSprite.AnimationChanged += () =>
		{
			AnimatedSprite.Offset = new(0, 0);
			if (IsAttackAnimation())
			{
				AnimatedSprite.Offset = new(LastDirectionHorizontal * 2f, -5f);
			}
		};

		AddChild(shadowTimer);
		shadowTimer.Start(0.05);
		shadowTimer.Timeout += CreateShadow;
	}

	public void Hit(HitPayload hitPayload)
    {
        Life.Hit(hitPayload.Damage);
		AnimatedSprite.Hit();
		camera.Hit(hitPayload.Force.X);
		EmitSignal(SignalName.Damaged, hitPayload);
    }

	public void ProcessAttack()
	{
		if (CurrentState != State.ATTACKING)
		{
			if (Input.IsActionJustPressed("attack"))
			{
				PlayDefaultAttackAnimation();
				CurrentState = State.ATTACKING;

				// var swordSlashPacked = GD.Load<PackedScene>("res://Scenes/sword_slash.tscn");
				// var swordSlashInstance = swordSlashPacked.Instantiate<SwordSlash>();
				// AddChild(swordSlashInstance);
				// swordSlashInstance.Position = new(4.0f, 0.0f);
			}
		}
	}

	public void PlayDefaultAttackAnimation()
	{
		AnimatedSprite.Play(attackAnimations[attackAnimationIndex]);
		attackAnimationIndex += 1;
		attackAnimationIndex %= 2;
	}

	public void PlayForwardSlashAnimation()
	{
		AnimatedSprite.Play("naked_forward_slash");
	}

	public void ProcessState()
	{
		if (CurrentState == State.ATTACKING)
		{
			return;
		}

		if (Direction.X != 0.0)
		{
			CurrentState = State.MOVING;
		}
		else
		{
			CurrentState = State.IDLE;
		}

		if (CurrentState == State.IDLE)
		{
			AnimatedSprite.Play("naked_idle");
		}
		else if (CurrentState == State.MOVING)
		{
			AnimatedSprite.Play("naked_walk");
		}
	}

	public void CreateShadow()
	{
		var shadowInstance = shadowPacked.Instantiate<Shadow>();
		shadowInstance.Offset = AnimatedSprite.Offset;
		shadowInstance.Position = Position;
		shadowInstance.FlipH = Flipped();
		shadowInstance.InitialColor = new(1.0f, 0.0f, 0.0f, 0.2f);
		shadowInstance.Texture = AnimatedSprite.GetFrameTexture();
		GetTree().CurrentScene.CallDeferred("add_child", shadowInstance);
	}

	private double CalculateDamage()
	{
		var weapon = _body.Retrieve(PlayerBody.ItemType.WEAPON);
		double damage = 0;

		if (weapon is not null)
		{
			var payload = weapon.MeleeWeapon.DamageBlueprint.CreatePayload();

			foreach (var damageEntry in payload)
			{
				damage += damageEntry.Value;
			}
		}
		
		return damage;
	}

	private void Attack()
	{
		var bodies = hitArea.GetOverlappingBodies();

		var weapon = _body.Retrieve(PlayerBody.ItemType.WEAPON);
		double damage = 0;

		if (weapon is not null)
		{
			var payload = weapon.MeleeWeapon.DamageBlueprint.CreatePayload();

			foreach (var damageEntry in payload)
			{
				damage += damageEntry.Value;
			}
		}

		bool successfulHit = false;
		foreach (var body in bodies)
		{
			if (body is IHittable lifeHolder)
			{
				HitPayload hitPayload = new ()
				{
					Damage = damage,
					Force = new(LastDirectionHorizontal * 64.0f, -64.0f),
					Position = body.Position,
					Attack = AttackType.SLASH,
				};
				lifeHolder.Hit(hitPayload);
				EmitSignal(SignalName.Attacked, hitPayload);
				successfulHit = true;
			}
		}
		if (successfulHit) EmitSignal(SignalName.SuccessfulHit);
	}

	public void ForwardSlashAttack()
	{
		var bodies = forwardSlashArea.GetOverlappingBodies();

		bool successfulHit = false;
		foreach (var body in bodies)
		{
			if (body is IHittable lifeHolder)
			{
				HitPayload hitPayload = new ()
				{
					Damage = 10.0,
					Force = new(LastDirectionHorizontal * 128.0f, -128.0f),
					Position = body.Position,
					Attack = AttackType.SLASH,
				};
				lifeHolder.Hit(hitPayload);
				EmitSignal(SignalName.Attacked, hitPayload);
				successfulHit = true;
			}
		}
		if (successfulHit) EmitSignal(SignalName.SuccessfulHit);

		var bigSlashInstance = bigSlashPacked.Instantiate<AnimatedSprite2D>();
		AddChild(bigSlashInstance);
		bigSlashInstance.FlipH = Flipped();

		bigSlashInstance.Position = new(LastDirectionHorizontal * 10.0f, 0.0f);

		entitySoundPlayer.PlayForwardSlash();
	}

	private void OnDeath()
	{
		SetProcess(false);
		SetPhysicsProcess(false);
		AnimatedSprite.Hide();
	}

	private bool IsAttackAnimation()
	{
		return attackAnimations.Contains(AnimatedSprite.Animation.ToString());
	}

	private bool Flipped()
	{
		return LastDirectionHorizontal <= 0.0f;
	}
}
