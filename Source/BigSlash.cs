using Godot;
using System;

public partial class BigSlash : AnimatedSprite2D
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // var tween = CreateTween();
        // tween.SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic);

        // Modulate = new(Modulate, 0.8f);

        // tween.TweenProperty(this, "modulate:a", 0.0f, 0.2f);
        // tween.TweenCallback(Callable.From(QueueFree));
        AnimationFinished += QueueFree;
    }
}