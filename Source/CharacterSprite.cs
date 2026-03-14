using Godot;
using System;

public partial class CharacterSprite : Sprite2D
{
    public void Hit()
    {
        Tween tween = CreateTween().SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic);
        Modulate = new(1.0f, 0.0f, 0.0f);
        tween.TweenProperty(this, "modulate", Color.Color8(255, 255, 255), 0.2f);
    }
}