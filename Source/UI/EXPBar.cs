using System;
using Godot;

public partial class EXPBar : TextureProgressBar
{
	private Label label;

	public void Update(double value, double maxValue)
	{
		Value = value;
		MaxValue = maxValue;
		label.Text = $"EXP: {value}/{maxValue}";
	}

    public override void _Ready()
	{
		label = GetNode<Label>("Label");
	}
}
