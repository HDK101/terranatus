using Godot;
using System;

public partial class MusicPlayer : AudioStreamPlayer
{
	public void PlayFadeIn()
	{
		Tween tween = CreateTween().SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Circ);
		tween.TweenProperty(this, "volume_db", 0.0f, 0.25f);
		Play();
	}

	public void PlayFadeOut()
	{
		Tween tween = CreateTween().SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Circ);
		tween.TweenProperty(this, "volume_db", -30.0f, 1.0f);
		Play();
	}
}
