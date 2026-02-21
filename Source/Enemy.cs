using System.Collections.Generic;
using Godot;

public abstract partial class Enemy: CharacterBody2D, ILifeHolder, IHittable
{
    [Signal]
    public delegate void DamagedEventHandler(HitPayload payload);
    public Player Player { get; set; }
    public Life Life { get; set; } = new(3.0);
    public EntitySoundPlayer EntitySoundPlayer { get; protected set; }
    public abstract Texture2D CurrentTexture { get; }

    public int EXPReward { get; protected set; }

    public override void _Ready()
    {
        EntitySoundPlayer = GetNode<EntitySoundPlayer>("EntitySoundPlayer");
        Start();
    }

    public virtual void Hit(HitPayload payload)
    {
        Life.Hit(payload.Damage);
        PostHit(payload);
        EmitSignal(SignalName.Damaged, payload);

        Modulate = Color.Color8(255, 128, 128);
        var tween = CreateDefaultTween();
        tween.TweenProperty(this, "modulate", Color.Color8(255, 255, 255), 0.5f);
    }
    public abstract void Start();
    public abstract void PostHit(HitPayload payload);

    public virtual List<ItemBlueprint> ToDropItems()
    {
        return [];
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