using Godot;

public partial class PackedSceneDB: Node
{
    public PackedScene Fireball { get; set; }
    public PackedScene JumpFallParticles { get; set; }

    public override void _Ready()
    {
        Fireball = GD.Load<PackedScene>("res://Scenes/fireball.tscn");
        JumpFallParticles = GD.Load<PackedScene>("res://Scenes/fall_particles.tscn");
    }
}