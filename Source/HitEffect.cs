using Godot;
using System;

public partial class HitEffect : AnimatedSprite2D
{
	[Export]
	private bool scaleX;

	[Export]
	private bool scaleY;

	[Export]
	private float fadeMultiplier = 5.0f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AnimationFinished += QueueFree;
		Rotation = (float)(2.0 * Math.PI * Random.Shared.NextDouble());

		if (scaleX)
		{
			var tweenX = CreateDefaultTween();
			tweenX.TweenProperty(this, "scale:x", 0.0f, 0.2f);
		}
		if (scaleY)
		{
			var tweenY = CreateDefaultTween();
			tweenY.TweenProperty(this, "scale:y", 0.0f, 0.2f);
		}
		
		var tweenAlpha = CreateDefaultTween();
		tweenAlpha.TweenProperty(this, "modulate", Color.Color8(255, 255, 255, 0), 0.2f);
	}

	private Tween CreateDefaultTween()
	{
		var tween = GetTree().CreateTween().SetTrans(Tween.TransitionType.Cubic).SetEase(Tween.EaseType.InOut);
		tween.SetPauseMode(Tween.TweenPauseMode.Process);
		return tween;
	}
}
