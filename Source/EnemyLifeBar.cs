using Godot;
using System;

public partial class EnemyLifeBar : Sprite2D
{
    private Sprite2D progress;

    public override void _Ready()
    {
        progress = GetNode<Sprite2D>("Progress");
    }

    public void Update(double value, double maxValue)
    {
        progress.SetInstanceShaderParameter("ratio", (float)(value / maxValue));
    }
}