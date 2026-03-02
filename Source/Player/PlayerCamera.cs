using Godot;
using System;

public partial class PlayerCamera : Camera2D
{
    private float damageOffset = 0.0f;

    public override void _Process(double delta)
    {
        damageOffset *= 1.0f - (float)delta;
        Offset = new(damageOffset, 0.0f);
    }

    public void Hit(float damageOffset)
    {
        this.damageOffset = damageOffset;
    }
}