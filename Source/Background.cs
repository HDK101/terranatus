using Godot;
using System;

public partial class Background : ColorRect
{
	private AnimationPlayer animationPlayer;

	public override void _Ready()
	{
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
	}

	public void FadeIn()
	{
		animationPlayer.Play("fade_in");
	}

	public void FadeOut()
	{
		animationPlayer.Play("fade_out");
	}
}
