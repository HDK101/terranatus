using Godot;
using System;

public partial class LevelUpNotification : NinePatchRect
{
	public float CurrentSize { get; set; } = 96.0f;

	private float initialPositionX;

	public override void _Process(double delta)
	{
		Size = new (CurrentSize, Size.Y);
		Position = new(160.0f - CurrentSize / 2.0f, 120.0f);
	}

	public void Notificate()
	{
		var tween = CreateDefaultTween();
		tween.TweenProperty(this, nameof(CurrentSize), 160.0f, 1.0f);
		tween.TweenProperty(this, "modulate:a", 1.0f, 0.5f);

		GetTree().CreateTimer(2.0).Timeout += () =>
		{
			var tween = CreateDefaultTween();
			tween.TweenProperty(this, nameof(CurrentSize), 96.0f, 1.0f);
			tween.TweenProperty(this, "modulate:a", 0.0f, 1.0f);
		};
	}

	private Tween CreateDefaultTween()
	{
		var tween = CreateTween().SetTrans(Tween.TransitionType.Quint).SetEase(Tween.EaseType.InOut);
		tween.SetPauseMode(Tween.TweenPauseMode.Process);
		tween.SetParallel(true);
		return tween;
	}
}
