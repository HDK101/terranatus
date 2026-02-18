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
	}

    public override void _Process(double delta)
    {
		float deltaF = (float)delta;
		if (scaleX || scaleY)
		{
			float scaleValueX = scaleX ? Scale.X - deltaF * fadeMultiplier : 1.0f;
			float scaleValueY = scaleY ? Scale.Y - deltaF * fadeMultiplier : 1.0f;
	        Scale = new(scaleValueX, scaleValueY);
		}
		Modulate = new (Modulate, Modulate.A - deltaF * fadeMultiplier);
    }
}
