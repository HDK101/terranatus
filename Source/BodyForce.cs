using Godot;
using System;

public partial class BodyForce : Node
{
    public Vector2 Force { get; set; }

    public void Apply(Vector2 force)
    {
        this.Force = force;
    }

    public override void _PhysicsProcess(double delta)
    {
        float deltaF = (float)delta;
        Force *= (1.0f - deltaF);

        if (Force.LengthSquared() < 1e-4f)
            Force = Vector2.Zero;
    }

    public void Reset()
    {
        Force = Vector2.Zero;
    }
}