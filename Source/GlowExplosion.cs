using Godot;
using System;

public partial class GlowExplosion : GpuParticles2D
{
    public override void _Ready()
    {
        OneShot = true;
        Restart();
        Finished += QueueFree;
    }
}