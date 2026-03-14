using Godot;
using System;

public partial class FireballExplosion : GpuParticles2D
{
    private PointLight2D pointLight;
    private AudioStreamPlayer2D audioStreamPlayer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Restart();
        GetNode<GpuParticles2D>("SmokeExplosion").Restart();
        GetNode<GpuParticles2D>("ShockwaveExplosion").Restart();
        pointLight = GetNode<PointLight2D>("PointLight2D");
        audioStreamPlayer = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");

        audioStreamPlayer.Play();

        Tween tween = CreateTween().SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic);
        tween.TweenProperty(pointLight, "texture_scale", 1f, 0.2f);
        tween.TweenProperty(pointLight, "texture_scale", 0f, 0.2f);

        GetTree().CreateTimer(2.0).Timeout += QueueFree;
    }
}