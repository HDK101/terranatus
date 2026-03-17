using Godot;
using System;

public partial class LightEffect : Polygon2D
{
	public void Play()
	{
		Tween tween = CreateTween();

		tween.TweenProperty(this, "color:a", 1.0f, 2.0f);
	}

	public void Stop()
	{
		Tween tween = CreateTween();

		tween.TweenProperty(this, "color:a", 0.0f, 0.5f);
	}

	public override void _Process(double delta)
	{
		Rotate((float)(delta * Math.PI / 2.0));
	}
}
