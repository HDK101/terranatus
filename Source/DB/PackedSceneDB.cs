using Godot;

public partial class PackedSceneDB: Node
{
    public PackedScene Fireball { get; private set; }
    public PackedScene JumpFallParticles { get; private set; }
    public PackedScene FireExplosion { get; private set; }

    public override void _Ready()
    {
        Fireball = GD.Load<PackedScene>("res://Scenes/fireball.tscn");
        JumpFallParticles = GD.Load<PackedScene>("res://Scenes/fall_particles.tscn");
        FireExplosion = GD.Load<PackedScene>("res://Scenes/fireball_explosion.tscn");
    }
}