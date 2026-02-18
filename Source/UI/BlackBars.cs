using Godot;
using System;

public partial class BlackBars : Control
{
	private AnimationPlayer animationPlayer;

    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

	public void ShowBars()
	{
		animationPlayer.Play("slide_in");
	}
	public void HideBars()
	{
		animationPlayer.Play("slide_out");
	}
}
