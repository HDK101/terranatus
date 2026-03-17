using Godot;
using Godot.Collections;
using System;

public partial class LightsEffect : Node2D
{
	[Export]
	private Array<LightEffect> lights;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		int offsetIndex = 0;
		foreach (var light in lights)
		{
			light.Rotation = offsetIndex * (float)(Math.PI * (2.0 / lights.Count));
			offsetIndex += 1;
		}
	}

	public void Play()
	{
		foreach (var light in lights)
		{
			light.Play();
		}
	}

	public void Stop()
	{
		foreach (var light in lights)
		{
			light.Stop();
		}
	}
}
