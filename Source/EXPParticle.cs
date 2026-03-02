using Godot;
using System;

public partial class EXPParticle : Area2D
{
	public RandomNumberGenerator RNG { get; set; }
	public Player Player { get; set; }
	public int EXP { get; set; }
	private readonly float DEFAULT_SPEED_THRESHOLD = 256.0f;
	private readonly float INITIAL_SPEED_BASE = 64.0f;

	private Vector2 initialDirection = Vector2.Zero;
	private float initialSpeed = 128.0f;

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;

		initialDirection = Vector2.FromAngle(RNG.RandfRange(0.0f, Mathf.Pi * 2));
		initialSpeed = INITIAL_SPEED_BASE + RNG.RandfRange(0.0f, INITIAL_SPEED_BASE);

		Tween tween = CreateTween().SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic);
		tween.TweenProperty(this, nameof(initialSpeed), 0.0f, 1.0f);
	}

	public override void _Process(double delta)
	{
		float distance = Position.DistanceTo(Player.Position);
		float mirrorDistance = DEFAULT_SPEED_THRESHOLD - Math.Abs(DEFAULT_SPEED_THRESHOLD - distance);
		float speed = Math.Max(64.0f, mirrorDistance);
		var direction = Position.DirectionTo(Player.Position);
		Translate((initialDirection * initialSpeed + direction * speed) * (float)delta);
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body == Player)
		{	
			Player.Experience.Gain(EXP);
			QueueFree();
		}
	}
}
