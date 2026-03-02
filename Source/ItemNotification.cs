using Godot;
using System;

public partial class ItemNotification : TextureRect
{
    private Label quantityLabel;
    private TextureRect itemRect;

    public override void _Ready()
    {
        quantityLabel = GetNode<Label>("HBoxContainer/Label");
        itemRect = GetNode<TextureRect>("HBoxContainer/TextureRect");
    }

    public void ShowItem(ItemBlueprint item, int quantity)
    {
        Show();
        quantityLabel.Text = $"{quantity} x ";
        itemRect.Texture = item.Texture;
        var tween = CreateDefaultTween();

        tween.TweenProperty(this, "position:x", Position.X - Size.X, 0.5f);

        GetTree().CreateTimer(3.0).Timeout += () =>
        {
            var tween = CreateDefaultTween();
            tween.TweenProperty(this, "position:x", Position.X + Size.X, 0.5f);
        };
    }

    private Tween CreateDefaultTween()
    {
        var tween = GetTree().CreateTween().SetTrans(Tween.TransitionType.Cubic).SetEase(Tween.EaseType.InOut);
        tween.SetPauseMode(Tween.TweenPauseMode.Process);
        return tween;
    }
}