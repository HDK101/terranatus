using Godot;
using System;
using System.Linq;

public partial class Player : CharacterBody2D
{
	public enum State
	{
		IDLE,
		MOVING,
		JUMPING,
		FALLING,
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
	public Mana Mana { get; init; } = new(5);
	public Inventory Inventory { get; } = new();
	public Experience Experience { get; } = new();
	public PlayerBody Body => _body;
	public Vector2 Direction { get; set; }
	public State CurrentState { get => currentState; set => currentState = value; }
	public CharacterSprite AnimatedSprite { get; private set; }
	public Area2D HitArea { get => hitArea; }
	public Area2D ForwardSlashArea { get => forwardSlashArea; }
	public float LastDirectionHorizontal { get => lastDirectionHorizontal; set => lastDirectionHorizontal = value; }
	public Skills Skills => _skills;
	public bool IsWalking => Velocity.X != 0;
	public bool IsAttacking { get; private set; } = false;

    public PlayerView View { get; set; }

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
	private PackedScene jumpFallParticlesPacked;

	private PackedSceneDB packedSceneDB;

	private readonly string[] attackAnimations = [
		"aemilia_naked_attack_sword1",
		"aemilia_naked_attack_sword1"
	];

	private int attackAnimationIndex = 0;

	private Skills _skills;

	public override void _Ready()
	{
		packedSceneDB = GetNode<PackedSceneDB>("/root/PackedSceneDB");
		bigSlashPacked = GD.Load<PackedScene>("res://Scenes/big_slash.tscn");
		shadowPacked = GD.Load<PackedScene>("res://Scenes/shadow.tscn");
		jumpFallParticlesPacked = GD.Load<PackedScene>("res://Scenes/fall_particles.tscn");

		ItemDB itemDB = GetNode<ItemDB>("/root/ItemDB");
		ItemBlueprint apple = itemDB.Retrieve("APPLE");
		shortSwordBlueprint = GD.Load<ItemBlueprint>("res://Resources/Items/short_sword.tres");
		AnimatedSprite = GetNode<CharacterSprite>("AnimatedSprite2D");
		hitArea = GetNode<Area2D>("HitArea");
		forwardSlashArea = GetNode<Area2D>("ForwardSlashArea");
		entitySoundPlayer = GetNode<EntitySoundPlayer>("EntitySoundPlayer");
		itemArea = GetNode<Area2D>("ItemArea");
		camera = GetNode<PlayerCamera>("Camera2D");
		View = new(GetNode<AnimationTree>("AnimationTree"), GetNode<Sprite2D>("Sprite2D"));
		View.Start();

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

		stateManager = new();
		AddChild(stateManager);
		stateManager.ChangeState(new PlayerDefaultState(new(this)));

		// AddChild(shadowTimer);
		// shadowTimer.Start(0.05);
		// shadowTimer.Timeout += CreateShadow;

		_skills = new(new(this))
		{
			HasMana = Mana.Has,
			UseMana = Mana.Use,
		};

		_skills.Start();

		Experience.Change += entitySoundPlayer.PlayPickEXP;
	}

	public void Hit(HitPayload hitPayload)
	{
		Life.Hit(hitPayload.Damage);
		AnimatedSprite.Hit();
		camera.Hit(hitPayload.Force.X);
		EmitSignal(SignalName.Damaged, hitPayload);
	}

	public void PlayDefaultAttackAnimation()
	{
		View.Attack();
	}

	public void PlayForwardSlashAnimation()
	{
		View.Attack();
	}

	public void StartAttackOffset()
	{
		if (View.FlipH)
		{
			View.SpriteOffset = new(-2.0f, 0.0f);
			return;
		}
		View.SpriteOffset = new(2.0f, 0.0f);
	}

	public void StartAttack()
	{
		IsAttacking = true;
	}

	public void EndAttack()
	{
		IsAttacking = false;
	}

	public void ResetViewOffset()
	{
		View.SpriteOffset = Vector2.Zero;
	}

	public void CreateShadow()
	{
		var shadowInstance = shadowPacked.Instantiate<Shadow>();
		shadowInstance.Offset = AnimatedSprite.Offset;
		shadowInstance.Position = Position;
		shadowInstance.FlipH = Flipped();
		shadowInstance.InitialColor = new(1.0f, 0.7f, 1.0f, 0.4f);
		shadowInstance.Texture = AnimatedSprite.GetFrameTexture();
		GetTree().CurrentScene.CallDeferred("add_child", shadowInstance);
	}

	public void StartForwardSlash()
	{
		stateManager.ChangeState(new PlayerForwardSlash(new(this))
		{
			InitialDirection = LastDirectionHorizontal,
		});
	}

	public void ForwardSlashAttack()
	{
		var bodies = forwardSlashArea.GetOverlappingBodies();

		bool successfulHit = false;
		foreach (var body in bodies)
		{
			if (body is IHittable lifeHolder)
			{
				HitPayload hitPayload = new()
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

	public void CastFireball()
	{
		var fireballInstance = packedSceneDB.Fireball.Instantiate<Fireball>();
		fireballInstance.Position = Position;
		fireballInstance.Direction = LastDirectionHorizontal;

		entitySoundPlayer.PlayFireRelease();

		GetTree().CurrentScene.CallDeferred("add_child", fireballInstance);
	}

	public float Jump()
    {
		CreateJumpFallParticles();
		entitySoundPlayer.Jump();
		View.Jump();
		return JumpVelocity;
    }

	private void CreateJumpFallParticles()
	{
		var instance = jumpFallParticlesPacked.Instantiate<GpuParticles2D>();
		instance.Position = Position + Vector2.Down * 8f;
		instance.Restart();
		GetTree().CurrentScene.CallDeferred("add_child", instance);
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
				HitPayload hitPayload = new()
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
