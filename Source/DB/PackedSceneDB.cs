using Godot;

public partial class PackedSceneDB : Node
{
    public PackedScene Fireball { get; private set; }
    public PackedScene JumpFallParticles { get; private set; }
    public PackedScene FireExplosion { get; private set; }

    public PackedScene FlyingCorpse { get; private set; }
    public PackedScene DroppableItem { get; private set; }
    public PackedScene Slash { get; private set; }
    public PackedScene Punch { get; private set; }
    public PackedScene EXP { get; private set; }
    public PackedScene GlowingParticlesExplosion { get; private set; }
    public PackedScene GlowingParticlesRespawnExplosion { get; private set; }
    public PackedScene DeathBigGlowExplosion { get; private set; }
    public PackedScene BigWhiteExplosion { get; private set; }
    public PackedScene BigSlash { get; private set; }

    public override void _Ready()
    {
        Fireball = GD.Load<PackedScene>("res://Scenes/fireball.tscn");
        JumpFallParticles = GD.Load<PackedScene>("res://Scenes/fall_particles.tscn");
        FireExplosion = GD.Load<PackedScene>("res://Scenes/fireball_explosion.tscn");
        FlyingCorpse = GD.Load<PackedScene>("res://Scenes/flying_corpse.tscn");
        DroppableItem = GD.Load<PackedScene>("res://Scenes/droppable_item.tscn");
        Slash = GD.Load<PackedScene>("res://Scenes/slash.tscn");
        Punch = GD.Load<PackedScene>("res://Scenes/punch.tscn");
        EXP = GD.Load<PackedScene>("res://Scenes/exp_particle.tscn");
        GlowingParticlesExplosion = GD.Load<PackedScene>("res://Scenes/ParticlesEffect/glow_explosion.tscn");
        GlowingParticlesRespawnExplosion = GD.Load<PackedScene>("res://Scenes/ParticlesEffect/respawn_glow_explosion.tscn");
        DeathBigGlowExplosion = GD.Load<PackedScene>("res://Scenes/ParticlesEffect/death_big_glow_explosion.tscn");
        BigWhiteExplosion = GD.Load<PackedScene>("res://Scenes/Effects/big_white_explosion.tscn");
        BigSlash = GD.Load<PackedScene>("res://Scenes/Effects/slash_effect.tscn");
    }
}