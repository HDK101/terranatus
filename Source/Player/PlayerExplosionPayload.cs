using Godot;
using System.Collections.Generic;

public partial class PlayerExplosionPayload : RefCounted
{
    public Vector2 Position { get; set; }
    public float Radius { get; set; } = 64.0f;

    public Dictionary<DamageType, DamageRangeRef> Damages { get; set; }
}