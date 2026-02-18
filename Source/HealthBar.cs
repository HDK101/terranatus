using System;
using Godot;

public partial class HealthBar : TextureProgressBar
{
	private float offset = 0.0f;
	private Vector2 initialPosition;

	public void Update(double value, double maxValue)
	{
		Value = value;
		MaxValue = maxValue;
	}

	public void Hit()
	{
		offset = -8f;
	}

    public override void _Ready()
	{
		initialPosition = Position;
	}

    public override void _Process(double delta)
	{
		offset *= Mathf.Exp(-3.0f * (float)delta);

		Position = new(initialPosition.X + offset * (float)Random.Shared.NextDouble(), initialPosition.Y);
	}
}
