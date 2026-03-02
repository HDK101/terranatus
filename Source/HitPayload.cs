using Godot;

public partial class HitPayload : RefCounted
{
    public required double Damage { get; set; }
    public Vector2 Force { get; set; }
    public Vector2 Position { get; set; }
    public AttackType Attack { get; set; }
}