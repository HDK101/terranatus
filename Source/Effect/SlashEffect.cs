using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
[Tool]
public partial class SlashEffect : Line2D
{
    [ExportToolButton("Calculate points")]
    public Callable CalculatePointsButton => Callable.From(CalculatePoints);

    [ExportToolButton("Play")]
    public Callable PlayButton => Callable.From(Play);

    public bool FlipH
    {
        get => _flipH; set
        {
            _flipH = value;

            if (_flipH)
            {
                Scale = new(-1, 1);
                return;
            }

            Scale = Vector2.One;
        }
    }

    private bool _flipH = false;

    private ShaderMaterial shaderMaterial;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Modulate = new(Modulate, 0.0f);
        shaderMaterial = (ShaderMaterial)Material;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.

    public void Play()
    {
        Tween tween = CreateTween().SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Linear).SetParallel();
        Modulate = new(Modulate, 1.0f);
        Rotation = 0.0f;
        tween.TweenProperty(this, "modulate:a", 1.0f, 0.1f);
        tween.TweenProperty(this, "rotation", (float)(Math.PI / 2.0f), 0.4f);
        tween.TweenMethod(Callable.From((float offset) => shaderMaterial.SetShaderParameter("offset", offset)), 0.0f, 1.0f, 0.2f);

        if (!Engine.IsEditorHint())
        {
            GetTree().CreateTimer(0.5f).Timeout += QueueFree;
        }
    }

    public void CalculatePoints()
    {
        List<Vector2> pointsList = [];
        int maxPoints = 64;
        float radius = 24f;

        for (int i = 0; i <= maxPoints; i++)
        {
            float ratio = i / (float)maxPoints;

            float x = radius * MathF.Sin(ratio * (float)Math.PI * 1.5f);
            float y = radius * MathF.Cos(ratio * (float)Math.PI * 1.5f);

            pointsList.Add(new(x, y));
        }

        Points = [.. pointsList];
    }
}