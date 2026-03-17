using Godot;
using System;

public partial class TransitionRect : ColorRect
{
	private ShaderMaterial shaderMaterial;

	public override void _Ready()
	{
		shaderMaterial = (ShaderMaterial)Material;
	}

	public void FadeIn()
	{
		Tween tween = CreateTween().SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Linear);
		tween.TweenMethod(Callable.From((float value) => SetProgress(value)), 0.0f, 1.0f, 1.5f);
	}

	public void FadeOut()
	{
		Tween tween = CreateTween().SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Linear);
		tween.TweenMethod(Callable.From((float value) => SetProgress(value)), 1.0f, 0.0f, 1.5f);
	}

	public void SetProgress(float value)
	{
		shaderMaterial.SetShaderParameter("progress", value);
	}
}
