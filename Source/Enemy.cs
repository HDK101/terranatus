using System.Collections.Generic;
using Godot;

public abstract partial class Enemy: CharacterBody2D, ILifeHolder, IHittable
{
    [Signal]
    public delegate void DamagedEventHandler(HitPayload payload);
    public Player Player { get; set; }
    public Life Life { get; set; } = new(3.0);
    public abstract Texture2D CurrentTexture { get; }

    public int EXPReward { get; protected set; }

    public virtual void Hit(HitPayload payload)
    {
        Life.Hit(payload.Damage);
        PostHit(payload);
        EmitSignal(SignalName.Damaged, payload);
    }
    public abstract void PostHit(HitPayload payload);

    public virtual List<ItemBlueprint> ToDropItems()
    {
        return [];
    }

	protected bool IsPlayerNear(float distance)
    {
        return Player.Position.DistanceTo(Position) <= distance;
    }
}