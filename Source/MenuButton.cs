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

    private Control underLine;
    private Label label;
    private bool selected = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        underLine = GetNode<Control>("UnderLine");
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
        Tween tween = CreateTween().SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic).SetParallel();
        tween.TweenProperty(underLine, "modulate", new Color(1.0f, 1.0f, 1.0f, 1.0f), 0.5f);
        tween.TweenProperty(underLine, "size:x", 64.0f, 0.5f);
        selected = true;
    }

    public void Unselect()
    {
        Tween tween = CreateTween().SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic).SetParallel();
        tween.TweenProperty(underLine, "modulate", new Color(0.1f, 0.1f, 0.4f, 0.0f), 0.5f);
        tween.TweenProperty(underLine, "size:x", 0.0f, 0.5f);
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