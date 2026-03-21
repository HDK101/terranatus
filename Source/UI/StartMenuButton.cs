using Godot;
using System;

public partial class StartMenuButton : Control, ISelectable, IActionable
{
	[Export] private TextureRect lineRect;

	public Action Action { get; set; }

	public void Select()
	{
		GD.Print($"{Name} Selected");
		var tween = lineRect.CreateTween();
		tween.SetTrans(Tween.TransitionType.Cubic);
		tween.SetEase(Tween.EaseType.Out);
		tween.SetParallel(true);
		tween.TweenProperty(lineRect, "custom_minimum_size:x", 48f, 0.3f).From(0f);
	}

	public void Unselect()
	{
		GD.Print($"{Name} Unselected");
		var tween = lineRect.CreateTween();
		tween.SetTrans(Tween.TransitionType.Cubic);
		tween.SetEase(Tween.EaseType.Out);
		tween.SetParallel(true);
		tween.TweenProperty(lineRect, "custom_minimum_size:x", 0f, 0.3f);
	}

	public void Execute()
	{
		Action?.Invoke();
	}
}
