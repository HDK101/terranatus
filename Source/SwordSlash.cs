using Godot;
using System;

public partial class SwordSlash : AnimatedSprite2D
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var tween = CreateTween().SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Quint);
        tween.TweenProperty(this, "modulate:a", 0.0f, 0.2f);
        tween.TweenCallback(Callable.From(QueueFree));
    }
}