using Godot;
using System;

[GlobalClass]
[Tool]
public partial class MenuButton : Control
{
    [Export]
    public string Text { get; set; }
    public Action Trigger { get; set; }

    private TextureRect arrow;
    private Label label;
    private bool selected = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        arrow = GetNode<TextureRect>("Arrow");
        label = GetNode<Label>("Label");
        label.Text = Text;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_select") && selected)
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