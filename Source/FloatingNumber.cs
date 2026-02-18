using Godot;
using System;

public partial class FloatingNumber : RichTextLabel
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetTree().CreateTimer(2.0).Timeout += QueueFree;
	}

	public void Start(string value)
	{
		Text = $"[floating]{value}[/floating]";
	}
}
