using Godot;
using System.Collections.Generic;

public abstract partial class Enemy : CharacterBody2D, ILifeHolder, IHittable
{
    [Signal]
    public delegate void DamagedEventHandler(HitPayload payload);
    public Player Player { get; set; }
    public Life Life { get; set; } = new(3);
    public EntitySoundPlayer EntitySoundPlayer { get; protected set; }
    public abstract Texture2D CurrentTexture { get; }

    public int EXPReward { get; protected set; }
    protected MixedLootTableDrop LootTableDrop { get; set; }
    protected QuantityLootDrop QuantityLootDrop { get; set; }
    protected RandomNumberGenerator RandomNumberGenerator { get; set; }

    protected EnemyLifeBar lifeBar;


    public override void _Ready()
    {
        var packedLifeBar = GD.Load<PackedScene>("res://Scenes/enemy_life_bar.tscn");
        lifeBar = packedLifeBar.Instantiate<EnemyLifeBar>();
        AddChild(lifeBar);
        lifeBar.Update(Life.Value, Life.MaxValue);
        EntitySoundPlayer = GetNode<EntitySoundPlayer>("EntitySoundPlayer");
        RandomNumberGenerator = new();
        RandomNumberGenerator.Randomize();
        Start();
    }

    public virtual void Hit(HitPayload payload)
    {
        Life.Hit(payload.Damage);
        PostHit(payload);
        lifeBar.Update(Life.Value, Life.MaxValue);
        EmitSignal(SignalName.Damaged, payload);

        Modulate = Color.Color8(255, 128, 128);
        var tween = CreateDefaultTween();
        tween.TweenProperty(this, "modulate", Color.Color8(255, 255, 255), 0.5f);
    }
    public abstract void Start();
    public abstract void PostHit(HitPayload payload);

    public virtual List<(ItemBlueprint blueprint, int quantity)> ToDropItems()
    {
        int quantity = QuantityLootDrop.Random(RandomNumberGenerator);
        List<(ItemBlueprint blueprint, int quantity)> drops = [];
        for (int i = 0; i < quantity; i++)
        {
            var drop = LootTableDrop.Drop(RandomNumberGenerator);
            var itemQuantity = drop.RandomQuantity(RandomNumberGenerator);
            drops.Add((drop.Blueprint, itemQuantity));
        }
        return drops;
    }

    protected bool IsPlayerNear(float distance)
    {
        return Player.Position.DistanceTo(Position) <= distance;
    }

    protected Tween CreateDefaultTween()
    {
        var tween = GetTree().CreateTween().SetTrans(Tween.TransitionType.Cubic).SetEase(Tween.EaseType.InOut);
        tween.SetPauseMode(Tween.TweenPauseMode.Process);
        return tween;
    }
}