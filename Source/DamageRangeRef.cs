using Godot;

public partial class DamageRangeRef(int min, int max): RefCounted
{
    public int Rand(RandomNumberGenerator rng)
    {
        return rng.RandiRange(min, max);
    }
}