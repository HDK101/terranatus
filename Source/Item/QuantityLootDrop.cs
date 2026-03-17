using Godot;

public partial class QuantityLootDrop: RefCounted
{
    public int Min { get; set; } = 1;
    public int Max { get; set; } = 1;

    public int Random(RandomNumberGenerator rng)
    {
        return rng.RandiRange(Min, Max);
    }
}