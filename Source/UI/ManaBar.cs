using Godot;
using System;

public partial class ManaBar : TextureProgressBar
{
    private float offset = 0.0f;
    private Vector2 initialPosition;
    private Label label;

    public void Update(double value, double maxValue)
    {
        Value = value;
        MaxValue = maxValue;
        label.Text = $"{value}/{maxValue}";
    }

    public void Hit()
    {
        offset = -8f;
    }

    public override void _Ready()
    {
        initialPosition = Position;
        label = GetNode<Label>("Label");
    }

    public override void _Process(double delta)
    {
        offset *= Mathf.Exp(-3.0f * (float)delta);

        Position = new(initialPosition.X + offset * (float)Random.Shared.NextDouble(), initialPosition.Y);
    }
}