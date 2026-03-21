using Godot;
using System;
using System.Collections.Generic;

public partial class SoundOptions : Control
{
	public BackButton BackButton => backButton;

	[Export]
	private MusicVolumeSlider musicVolumeSlider;

	[Export]
	private SfxVolumeSlider sfxVolumeSlider;

	[Export]
	private BackButton backButton;

	private ISelectable currentSelectable;
	private List<ISelectable> selectables;
	private int currentIndex = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		selectables = [musicVolumeSlider, sfxVolumeSlider, backButton];
		SelectIndex(0);

		musicVolumeSlider.Changed += (value) =>
		{
			int busIndex = AudioServer.GetBusIndex("Music");
			AudioServer.SetBusVolumeDb(busIndex, Mathf.LinearToDb((float)value / 100f));
		};

		sfxVolumeSlider.Changed += (value) =>
		{
			int busIndex = AudioServer.GetBusIndex("SFX");
			AudioServer.SetBusVolumeDb(busIndex, Mathf.LinearToDb((float)value / 100f));
		};
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_up"))
			SelectIndex(currentIndex - 1);
		else if (@event.IsActionPressed("ui_down"))
			SelectIndex(currentIndex + 1);
		else if (@event.IsActionPressed("ui_accept")) {}
			//currentSelectable.DoAction();
	}

	private void SelectIndex(int index)
	{
		currentSelectable?.Unselect();
		currentIndex = (index % selectables.Count + selectables.Count) % selectables.Count;
		currentSelectable = selectables[currentIndex];
		currentSelectable.Select();
	}
}
