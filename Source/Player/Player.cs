using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public enum State
	{
		IDLE,
		MOVING,
        ATTACKING,
    }

	[Signal]
	public delegate void ItemPickedEventHandler(ItemBlueprint blueprint, int quantity);

	[Signal]
	public delegate void AttackedEventHandler(HitPayload payload);

	[Signal]
	public delegate void DamagedEventHandler(HitPayload payload);

	public Life Life { get; init; } = new(5.0);
	public Inventory Inventory { get; } = new();
	public Experience Experience { get; } = new();
	public PlayerBody Body => _body;

	public const float Speed = 100.0f;
	public const float JumpVelocity = -175.0f;

	private CharacterSprite animatedSprite;
	private readonly PlayerBody _body = new();
	private PlayerCamera camera;

	private Vector2 direction;

	private State currentState = State.IDLE;

	private Area2D hitArea;
	private Area2D itemArea;

	private float lastDirectionHorizontal = 0.0f;

	private ItemBlueprint shortSwordBlueprint;

	public override void _Ready()
	{
		ItemDB itemDB = GetNode<ItemDB>("/root/ItemDB");
		ItemBlueprint apple = itemDB.Retrieve("APPLE");
		shortSwordBlueprint = GD.Load<ItemBlueprint>("res://Resources/Items/short_sword.tres");
		animatedSprite = GetNode<CharacterSprite>("AnimatedSprite2D");
		hitArea = GetNode<Area2D>("HitArea");
		itemArea = GetNode<Area2D>("ItemArea");
		camera = GetNode<PlayerCamera>("Camera2D");
		animatedSprite.AnimationFinished += () =>
		{
			if (animatedSprite.Animation == "naked_attack_sword")
			{
				currentState = State.IDLE;
			}
		};

		animatedSprite.FrameChanged += () =>
		{
			if (animatedSprite.Animation == "naked_attack_sword" && animatedSprite.Frame == 3)
			{
				Attack();
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
		Experience.Leveled += () => GD.Print("Leveled!");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		if (currentState == State.ATTACKING)
		{
			velocity.X = 0;
			Velocity = velocity;
			MoveAndSlide();
			return;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	public override void _Process(double delta)
	{
		if (direction.X < 0.0)
		{
			animatedSprite.FlipH = true;
			hitArea.Position = new(-16f, 0.0f);
			lastDirectionHorizontal = 1.0f;
		}
		if (direction.X > 0.0)
		{
			animatedSprite.FlipH = false;
			hitArea.Position = new(16f, 0.0f);
			lastDirectionHorizontal = -1.0f;
		}

		ProcessState();
		ProcessAttack();
	}

	public void Hit(HitPayload hitPayload)
    {
        Life.Hit(hitPayload.Damage);
		animatedSprite.Hit();
		camera.Hit(hitPayload.Force.X);
		EmitSignal(SignalName.Damaged, hitPayload);
    }

	private void ProcessAttack()
	{
		if (currentState != State.ATTACKING)
		{
			if (Input.IsActionPressed("attack"))
			{
				animatedSprite.Play("naked_attack_sword");
				currentState = State.ATTACKING;
			}
		}
	}

	private void ProcessState()
	{
		if (currentState == State.ATTACKING)
		{
			return;
		}

		if (direction.X != 0.0)
		{
			currentState = State.MOVING;
		}
		else
		{
			currentState = State.IDLE;
		}

		if (currentState == State.IDLE)
		{
			animatedSprite.Play("naked_idle");
		}
		else if (currentState == State.MOVING)
		{
			animatedSprite.Play("naked_walk");
		}
	}

	private void Attack()
	{
		var bodies = hitArea.GetOverlappingBodies();

		foreach (var body in bodies)
		{
			if (body is IHittable lifeHolder)
			{
				HitPayload hitPayload = new ()
				{
					Damage = 1.0,
					Force = new(-lastDirectionHorizontal * 64.0f, -64.0f),
					Position = body.Position,
					Attack = HitPayload.AttackType.SLASH,
				};
				lifeHolder.Hit(hitPayload);
				EmitSignal(SignalName.Attacked, hitPayload);
			}
		}
	}

	private void OnDeath()
	{
		SetProcess(false);
		SetPhysicsProcess(false);
		animatedSprite.Hide();
	}
}
