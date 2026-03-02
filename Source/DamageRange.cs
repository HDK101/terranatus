using Godot;
using System;

[GlobalClass]
public partial class DamageRange : Resource
{
    [Export]
    public int Min;
    [Export]
    public int Max;

    public int Rand(RandomNumberGenerator rng)
    {
        return rng.RandiRange(Min, Max);
    }
}