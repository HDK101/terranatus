using System;
using Godot;

public partial class ItemNotification : TextureRect
{
	private AnimationPlayer animationPlayer;
	private Label quantityLabel;
	private TextureRect itemRect;

	public override void _Ready()
	{
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		quantityLabel = GetNode<Label>("HBoxContainer/Label");
		itemRect = GetNode<TextureRect>("HBoxContainer/TextureRect");
	}

	public void ShowItem(ItemBlueprint item, int quantity)
	{
		Show();
		quantityLabel.Text = $"{quantity} x ";
		itemRect.Texture = item.Texture;
		animationPlayer.Play("slide_in");

		GetTree().CreateTimer(3.0).Timeout += () =>
		{
			animationPlayer.Play("slide_out");
		};
	}
}

