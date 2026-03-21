using Godot;
using System;
using System.Collections.Generic;

public partial class StartMenu : Node2D
{
	[Export] private Sprite2D SmokeBackground;
	[Export] private GpuParticles2D GlowingParticles;
	[Export] private Sprite2D Logo;

	[Export] private Control Play;
	[Export] private Control Options;
	[Export] private Control Quit;

	private static List<Control> _buttons;
	private int _currentIndex;

	public override void _Ready()
	{
		_buttons = new List<Control> { Play, Options, Quit };
		_currentIndex = 0;
		SelectButton(_currentIndex);

		var tween = CreateTween();
		tween.SetTrans(Tween.TransitionType.Linear);
		tween.SetEase(Tween.EaseType.InOut);
		tween.TweenInterval(2.0f);
		tween.SetParallel(true);
		tween.TweenProperty(SmokeBackground, "material:shader_parameter/progress", 1.0f, 3.0f).From(0.0f);
		tween.TweenProperty(Logo, "material:shader_parameter/progress", 1.0f, 3.0f).From(0.0f);
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_down"))
		{
			ChangeButton(1);
		}
		else if (@event.IsActionPressed("ui_up"))
		{
			ChangeButton(-1);
		}
		else if (@event.IsActionPressed("ui_accept"))
		{
			if (_buttons[_currentIndex] is IActionable actionable)
				actionable.Execute();
		}
	}

	private void ChangeButton(int direction)
	{
		if (_buttons[_currentIndex] is ISelectable oldSelectable)
			oldSelectable.Unselect();

		_currentIndex = (_currentIndex + direction + _buttons.Count) % _buttons.Count;

		SelectButton(_currentIndex);
	}

	private void SelectButton(int index)
	{
		if (_buttons[index] is ISelectable selectable)
			selectable.Select();
	}
}
