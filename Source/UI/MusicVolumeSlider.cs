using Godot;
using System;

public partial class MusicVolumeSlider : VBoxContainer, ISelectable
{
	[Signal]
	public delegate void ChangedEventHandler(float value);

	private HSlider hSlider;
	private Label label;

	public void Select()
	{
		SetProcessInput(true);
	}

	public void Unselect()
	{
		SetProcessInput(false);
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_left", true))
			hSlider.Value -= hSlider.Step;
		else if (@event.IsActionPressed("ui_right", true))
			hSlider.Value += hSlider.Step;
	}

	public override void _Ready()
	{
		SetProcessInput(false);

		hSlider = GetNode<HSlider>("HBoxContainer/HSlider");
		label = GetNode<Label>("HBoxContainer/Label");

		int busIndex = AudioServer.GetBusIndex("Music");
		hSlider.Value = Mathf.DbToLinear(AudioServer.GetBusVolumeDb(busIndex)) * 100f;
		label.Text = $"{hSlider.Value}%";

		hSlider.ValueChanged += (value) =>
		{
			label.Text = $"{value}%";
			EmitSignal(SignalName.Changed, value);
		};
	}
}
