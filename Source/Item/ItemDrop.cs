using Godot;

[GlobalClass]
public partial class ItemDrop : Resource
{
    [Export]
    public float Weight { get; set; } = 1.0f;

    [Export]
    public int Min { get; set; } = 1;

    [Export]
    public int Max { get; set; } = 1;

    [Export]
    public ItemBlueprint Blueprint { get; set; }

    public int RandomQuantity(RandomNumberGenerator rng)
    {
        return rng.RandiRange(Min, Max);
    }
}