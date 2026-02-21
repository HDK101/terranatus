using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Menu : Control
{
	[Signal]
	public delegate void ChangeEventHandler();

	[Signal]
	public delegate void AcceptEventHandler();

	private enum StateVisible
	{
		NONE,
		PROFILE,
		INVENTORY,
	}

	[Signal]
	public delegate void UIVisibleEventHandler();

	[Signal]
	public delegate void UIHiddenEventHandler();

	[Export]
	private Control profile;

	[Export]
	private MenuButtons menuButtons;

	[Export]
	private InventoryUI inventory;

	[Export]
	private ColorRect background;

	[Export]
	private Control itemDetails;

	private int selectedIndex = 0;
	private bool active = false;
	private StateVisible stateVisible = StateVisible.NONE;


    public override void _Ready()
    {
		menuButtons.InventoryButton.Trigger += () => {
			ChangeState(StateVisible.INVENTORY);
			EmitSignal(SignalName.Accept);
		};
    }

    public override void _Input(InputEvent @event)
    {
		if (stateVisible != StateVisible.PROFILE) return;

        if (@event.IsActionPressed("ui_down"))
		{
			menuButtons.Buttons[selectedIndex].Unselect();
			menuButtons.Buttons[GetNormalizedIndex(1)].Select();
			EmitSignal(SignalName.Change);
		}
		if (@event.IsActionPressed("ui_up"))
		{
			menuButtons.Buttons[selectedIndex].Unselect();
			menuButtons.Buttons[GetNormalizedIndex(-1)].Select();
			EmitSignal(SignalName.Change);
		}
    }

	public int GetNormalizedIndex(int amount)
	{
		selectedIndex += amount;
		
		if (selectedIndex < 0)
		{
			selectedIndex = menuButtons.Buttons.Count - 1;
		}

		if (selectedIndex >= menuButtons.Buttons.Count)
		{
			selectedIndex %= menuButtons.Buttons.Count;
		}

		return selectedIndex;
	}

	public void ShowProfile()
	{
		var tweenProfile = CreateDefaultTween();
		var tweenButtons = CreateDefaultTween();
		tweenProfile.TweenProperty(profile, "position:x", 4.0f, 0.5f);
		tweenButtons.TweenProperty(menuButtons, "position:x", 130.0f, 0.5f);
	}

	public void HideProfile()
	{
		var tweenProfile = CreateDefaultTween();
		var tweenButtons = CreateDefaultTween();
		tweenProfile.TweenProperty(profile, "position:x", -104.0f, 0.5f);
		tweenButtons.TweenProperty(menuButtons, "position:x", 390.0f, 0.5f);
	}

	public void ShowInventory()
	{
		var tween = CreateDefaultTween();
		tween.TweenProperty(inventory, "position:x", 156.0f, 0.5f).Finished += () => inventory.Active = true;
	}
	public void HideInventory()
	{
		var tween = CreateDefaultTween();
		tween.TweenProperty(inventory, "position:x", 328.0f, 0.5f);
	}

	public void FadeInBackground()
	{
		var tween = CreateDefaultTween();
		tween.TweenProperty(background, "color:a", 0.2f, 0.5f);
	}
	public void FadeOutBackground()
	{
		var tween = CreateDefaultTween();
		tween.TweenProperty(background, "color:a", 0.0f, 0.5f);
	}

	public void ShowItemDetails()
	{
		var tween = CreateDefaultTween();
		tween.TweenProperty(itemDetails, "position:x", 4.0f, 0.5f);
	}

	public void HideItemDetails()
	{
		var tween = CreateDefaultTween();
		tween.TweenProperty(itemDetails, "position:x", -140.0f, 0.5f);
	}

	public void ShowUI()
	{
		ChangeState(StateVisible.PROFILE);
		EmitSignal(SignalName.UIVisible);
	}

	public void HideUI()
	{
		ChangeState(StateVisible.NONE);
		EmitSignal(SignalName.UIHidden);
	}

	private void ChangeState(StateVisible state)
	{
		if (state == StateVisible.PROFILE)
		{
			active = true;
			ShowProfile();
			FadeInBackground();
			HideInventory();
			HideItemDetails();
			menuButtons.ShowButtons();
		} else if (state == StateVisible.INVENTORY)
		{
			active = false;
			ShowInventory();
			ShowItemDetails();
			HideProfile();
			menuButtons.HideButtons();
		} else
		{
			active = false;
			HideProfile();
			HideInventory();
			HideItemDetails();
			FadeOutBackground();
			menuButtons.HideButtons();
		}

		stateVisible = state;
	}

	private Tween CreateDefaultTween()
	{
		var tween = GetTree().CreateTween().SetTrans(Tween.TransitionType.Cubic).SetEase(Tween.EaseType.InOut);
		tween.SetPauseMode(Tween.TweenPauseMode.Process);
		return tween;
	}
}
