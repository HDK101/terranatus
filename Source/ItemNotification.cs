using Godot;
using System;

public partial class ItemNotification : TextureRect
{
    private Label quantityLabel;
    private TextureRect itemRect;

    private Timer timer;
    private Vector2 screenSize;

    public override void _Ready()
    {
        screenSize = GetViewportRect().Size;
        quantityLabel = GetNode<Label>("HBoxContainer/Label");
        itemRect = GetNode<TextureRect>("HBoxContainer/TextureRect");
        timer = new()
        {
            OneShot = true,
        };
        AddChild(timer);
        timer.Timeout += () =>
        {
            var tween = CreateDefaultTween();
            tween.TweenProperty(this, "position:x", screenSize.X + Size.X, 0.5f);
        };
    }

    public void ShowItem(ItemBlueprint item, int quantity)
    {
        Show();
        quantityLabel.Text = $"{quantity} x ";
        itemRect.Texture = item.Texture;

        Position = new Vector2(screenSize.X + Size.X, Position.Y);

        var tween = CreateDefaultTween();
        tween.TweenProperty(this, "position:x", screenSize.X - Size.X, 0.5f);

        timer.Start(3.0);
    }

    private Tween CreateDefaultTween()
    {
        var tween = GetTree().CreateTween().SetTrans(Tween.TransitionType.Cubic).SetEase(Tween.EaseType.InOut);
        tween.SetPauseMode(Tween.TweenPauseMode.Process);
        return tween;
    }
}