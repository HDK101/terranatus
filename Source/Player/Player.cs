using Godot;
using System;

public partial class Player : CharacterBody2D
{
    [Signal]
    public delegate void DeathEventHandler();

    [Signal]
    public delegate void SuccessfulHitEventHandler();

    [Signal]
    public delegate void ItemPickedEventHandler(ItemBlueprint blueprint, int quantity);

    [Signal]
    public delegate void DamagedEventHandler(HitPayload payload);

    [Signal]
    public delegate void RespawnedEventHandler();

    public const float Speed = 100.0f;
    public const float JumpVelocity = -175.0f;

    public Life Life { get; init; } = new(10);
    public Mana Mana { get; init; } = new(100);
    public Inventory Inventory { get; } = new();
    public Experience Experience { get; } = new();
    public QuickSlots QuickSlots { get; } = new();
    public PlayerBody Body { get; } = new();
    public Vector2 Direction { get; set; }
    public Skills Skills { get; private set; }

    public PlayerDirection LookDirection { get; init; } = new();

    public PlayerCombat Combat { get; private set; }
    public PlayerView View { get; private set; }
    public EntitySoundPlayer EntitySoundPlayer { get; private set; }
    public PackedSceneDB PackedSceneDB { get; private set; }

    public bool IsWalking => Velocity.X != 0;
    public bool IsFalling => Velocity.Y < 0;
    public bool IsJumping => Velocity.Y > 0;
    public bool IsAttacking { get; private set; } = false;

    private PlayerCamera camera;
    private StateManager stateManager;

    private Timer shadowTimer = new()
    {
        OneShot = false,
        Autostart = true,
    };

    private ItemBlueprint shortSwordBlueprint;

    public override void _Ready()
    {
        PackedSceneDB = GetNode<PackedSceneDB>("/root/PackedSceneDB");

        ItemDB itemDB = GetNode<ItemDB>("/root/ItemDB");
        ItemBlueprint apple = itemDB.Retrieve("APPLE");
        shortSwordBlueprint = GD.Load<ItemBlueprint>("res://Resources/Items/short_sword.tres");
        EntitySoundPlayer = GetNode<EntitySoundPlayer>("EntitySoundPlayer");
        camera = GetNode<PlayerCamera>("Camera2D");

        StartView();

        StartCombat();

        Body.Equip(PlayerBody.ItemType.WEAPON, shortSwordBlueprint);

        Inventory.Add(shortSwordBlueprint, 1);
        Inventory.Add(shortSwordBlueprint, 1);
        Inventory.Add(apple, 1);

        Inventory.ConsumableUseFunc = (blueprint) =>
        {
            GD.Print("USE!!!");
            GD.Print(blueprint);
            return true;
        };

        Life.Death += OnDeath;

        CreateStateManager();
        CreateSkills();
        CreateQuickSlots();

        Experience.Change += EntitySoundPlayer.PlayPickEXP;
    }

    private void StartCombat()
    {
        Combat = GetNode<PlayerCombat>("Combat");
        Combat.ItemArea.BodyEntered += (body) =>
        {
            if (body is DroppableItem droppableItem)
            {
                var item = droppableItem.CurrentItem;
                Inventory.Add(item.Blueprint, item.Quantity);
                body.QueueFree();
                EmitSignal(SignalName.ItemPicked, item.Blueprint, item.Quantity);
            }
        };
        Combat.Direction = LookDirection;
        Combat.Body = Body;
        Combat.AttackedType += EntitySoundPlayer.PlayAttackSound;
        Combat.UsedForwardSlash += EntitySoundPlayer.PlayForwardSlash;
        Combat.HasCastFireball += EntitySoundPlayer.PlayFireRelease;
    }

    private void StartView()
    {
        View = GetNode<PlayerView>("View");
        View.Direction = LookDirection;
        View.AnimationTree = GetNode<AnimationTree>("AnimationTree");
    }

    public void Hit(HitPayload hitPayload)
    {
        if (Life.IsDead) return;

        Life.Hit(hitPayload.Damage);
        View.Sprite.Hit();
        camera.Hit(hitPayload.Force.X);
        EmitSignal(SignalName.Damaged, hitPayload);
    }

    public void PlayForwardSlashAnimation()
    {
        View.Jump();
    }

    public void StartAttack()
    {
        IsAttacking = true;
    }

    public void EndAttack()
    {
        IsAttacking = false;
    }

    public void StartForwardSlash()
    {
        stateManager.ChangeState(new PlayerForwardSlash(new(this))
        {
            InitialDirection = LookDirection.ValueDirection,
        });
    }

    public float Jump()
    {
        View.CreateJumpFallParticles();
        EntitySoundPlayer.Jump();
        if (!IsAttacking) View.Jump();
        return JumpVelocity;
    }

    public void Attack()
    {
        View.Attack();
    }

    public void CastAttackHit()
    {
        Combat.CastAttackHit();
    }

    public void ForwardSlashAttack()
    {
        Combat.ForwardSlashAttack();
    }

    public void SetShadowActive(bool active)
    {
        View.ShadowsActive = active;
    }

    public void Respawn(Vector2 position)
    {
        Position = position;
        stateManager.ChangeState(new PlayerRespawningState(new(this)));
    }

    private void OnDeath()
    {
        stateManager.ChangeState(new PlayerDyingState(new(this)));
        EmitSignal(SignalName.Death);
    }

    private void CreateSkills()
    {
        Skills = new(new(this))
        {
            HasMana = Mana.Has,
            UseMana = Mana.Use,
        };

        Skills.Start();
    }

    private void CreateStateManager()
    {
        stateManager = new();
        AddChild(stateManager);
    }

    private void CreateQuickSlots()
    {
        QuickSlots.SlotOne.ConsumableUse += Inventory.UseConsumable;
        QuickSlots.SlotTwo.ConsumableUse += Inventory.UseConsumable;
        QuickSlots.SlotThree.ConsumableUse += Inventory.UseConsumable;

        QuickSlots.SlotOne.ActiveSkill = Skills.Fireball;
        QuickSlots.SlotTwo.ActiveSkill = Skills.ForwardSlash;
        QuickSlots.SlotThree.Slot = Inventory.Consumables[0];
    }
}