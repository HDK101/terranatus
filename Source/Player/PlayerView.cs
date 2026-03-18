using Godot;
using System;

public partial class PlayerView() : Node2D
{
    public AnimationTree AnimationTree { get; set; }
    public Sprite2D Sprite { get; set; }
    public PlayerDirection Direction { get; set; }
    public bool FlipH { get => Sprite.FlipH; set => Sprite.FlipH = value; }
    public Vector2 SpriteOffset { get => Sprite.Offset; set => Sprite.Offset = value; }
    public bool ShadowsActive { get; set; } = false;

    public GpuParticles2D RespawnParticles { get; private set; }
    public GpuParticles2D EnergyOrbParticles { get; private set; }
    public LightsEffect LightsEffect { get; private set; }


    private AnimationNodeStateMachinePlayback stateMachine;

    private PackedScene shadowPacked;

    private PackedSceneDB packedSceneDB;

    private Timer shadowTimer = new();

    public override void _Ready()
    {
        shadowPacked = GD.Load<PackedScene>("res://Scenes/shadow.tscn");
        packedSceneDB = GetNode<PackedSceneDB>("/root/PackedSceneDB");
        AddChild(shadowTimer);
        StartShadowTimer();

        RespawnParticles = GetNode<GpuParticles2D>("RespawnParticles");
        EnergyOrbParticles = GetNode<GpuParticles2D>("EnergyOrb");
        LightsEffect = GetNode<LightsEffect>("LightsEffect");
    }

    public override void _Process(double delta)
    {
    }

    public void CreateShadow()
    {
        if (!ShadowsActive) return;

        var shadowInstance = shadowPacked.Instantiate<Shadow>();
        shadowInstance.Offset = Sprite.Offset;
        shadowInstance.Position = Position;
        shadowInstance.FlipH = Direction.Flipped();
        shadowInstance.InitialColor = new(1.0f, 0.7f, 1.0f, 0.4f);
        shadowInstance.Hframes = Sprite.Hframes;
        shadowInstance.Vframes = Sprite.Vframes;
        shadowInstance.Frame = Sprite.Frame;
        shadowInstance.Texture = Sprite.Texture;
        GetTree().CurrentScene.CallDeferred("add_child", shadowInstance);
    }

    public void StartIdle()
    {
        stateMachine = (AnimationNodeStateMachinePlayback)AnimationTree.Get("parameters/StateMachine/playback");
        stateMachine.Start("Idle");
    }

    public void StartSleeping()
    {
        stateMachine = (AnimationNodeStateMachinePlayback)AnimationTree.Get("parameters/StateMachine/playback");
        stateMachine.Start("Sleeping");
    }

    public void Dying()
    {
        stateMachine.Travel("Dying");
        RespawnParticles.Restart();
    }

    public void Attack()
    {
        stateMachine.Travel("AttackSword");
    }

    public void Idle()
    {
        stateMachine.Travel("Idle");
    }

    public void Walk()
    {
        stateMachine.Travel("Walk");
    }

    public void Respawning()
    {
        stateMachine.Travel("Respawning");
    }

    public void Jump()
    {
        stateMachine.Travel("Jump");
    }

    public void Fall()
    {
        stateMachine.Travel("Fall");
    }

    public void HideCharacter()
    {
        Sprite.Hide();
    }

    public void StartShadowTimer()
    {
        shadowTimer.Start(0.05);
        shadowTimer.Timeout += CreateShadow;
    }

    public void CreateJumpFallParticles()
    {
        var instance = packedSceneDB.JumpFallParticles.Instantiate<GpuParticles2D>();
        instance.Position = GlobalPosition + Vector2.Down * 8f;
        instance.Restart();
        GetTree().CurrentScene.CallDeferred("add_child", instance);
    }

    public void DeathExplosion()
    {
        var explosionInstance = packedSceneDB.GlowingParticlesRespawnExplosion.Instantiate<GpuParticles2D>();
        explosionInstance.OneShot = true;

        var bigExplosionInstance = packedSceneDB.DeathBigGlowExplosion.Instantiate<GpuParticles2D>();
        bigExplosionInstance.OneShot = true;

        AddChild(explosionInstance);
        AddChild(bigExplosionInstance);

        RespawnParticles.Emitting = false;
    }

    internal void StartRespawn()
    {
        RespawnParticles.Restart();
        LightsEffect.Play();
    }
}