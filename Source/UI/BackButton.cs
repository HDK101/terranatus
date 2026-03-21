using Godot;
using System;

public partial class BackButton : HBoxContainer, ISelectable
{
	[Signal]
	public delegate void PressedEventHandler();

	private TextureRect arrow;

	public override void _Ready()
	{
		arrow = GetNode<TextureRect>("Arrow");
		arrow.Hide();
	}

	public void Select()
	{
		arrow.Show();
	}

	public void Unselect()
	{
		arrow.Hide();
	}
}
