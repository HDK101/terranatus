using Godot;
using System;

public partial class Profile : VBoxContainer
{
    private AnimationPlayer animationPlayer;

    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public void ShowProfile()
    {
        animationPlayer.Play("slide_in");
    }

    public void HideProfile()
    {
        animationPlayer.Play("slide_out");
    }
}