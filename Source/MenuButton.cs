using Godot;
using System;

[GlobalClass]
[Tool]
public partial class MenuButton : Control
{
    public enum Button
    {
        INVENTORY,
        CHARACTER,
        SKILLS,
        SYSTEM,
    }
    
    [Export]
    public Button AssignedButton { get; set; }

    public Action Trigger { get; set; }

    private TextureRect arrow;
    private Label label;
    private bool selected = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        arrow = GetNode<TextureRect>("Arrow");
        label = GetNode<Label>("Label");
        InitializeText();
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

    private void InitializeText()
    {
        switch (AssignedButton)
        {
            case Button.INVENTORY:
                label.Text = "Inventory";
                break;
            case Button.CHARACTER:
                label.Text = "Character";
                break;
            case Button.SKILLS:
                label.Text = "Skills";
                break;
            case Button.SYSTEM:
                label.Text = "System";
                break;
        }
    }
}