using Godot;
using System;

public partial class MenuButton : TextureRect
{
	public Action Trigger { get; set; }

	private TextureRect arrow;
	private bool selected = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		arrow = GetNode<TextureRect>("Arrow");
	}

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("game_ui_select") && selected)
		{
			Trigger?.Invoke();
		}
    }

	public void Select()
	{
		arrow.Show();
		selected = true;
	}

	public void Unselect()
	{
		arrow.Hide();
		selected = false;
	}
}
