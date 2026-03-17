using Godot;
using System;
using System.Drawing;

public partial class WarpGate : StaticBody2D
{

	public Player Player { get; set; }

	private readonly float distanceThreshold = 64.0f;
	private GpuParticles2D particles;
	private PointLight2D pointLight;

	private bool isNear = false;

    public override void _Ready()
	{
		particles = GetNode<GpuParticles2D>("GPUParticles2D");
		pointLight = GetNode<PointLight2D>("PointLight2D");
	}

	public override void _Process(double delta)
	{
		float distance = Player.Position.DistanceTo(Position);
		bool isWithinThreshold = distance <= distanceThreshold;
		particles.Emitting = isWithinThreshold;

		if (isWithinThreshold)
		{
			if (!isNear)
			{
				Tween tween = CreateTween().SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic);
				tween.TweenProperty(pointLight, "energy", 0.05f, 0.5f);
				isNear = true;
			}
		}
		else
		{
			if (isNear)
			{
				Tween tween = CreateTween().SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic);
				tween.TweenProperty(pointLight, "energy", 0.0f, 0.5f);
				isNear = false;
			}
		}
	}
}
