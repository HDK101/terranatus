using Godot;

[GlobalClass]
public partial class MixedLootTableDrop : Resource
{
    [Export]
    public ArrayLootTableDrop CommonLootDrop { get; set; }

    [Export]
    public ArrayLootTableDrop RareLootDrop { get; set; }

    [Export]
    public ArrayLootTableDrop UniqueLootDrop { get; set; }

    [Export]
    public float DefaultChanceRareInDecimal { get; set; } = 0.10f;

    [Export]
    public float DefaultChanceUniqueInDecimal { get; set; } = 0.02f;

    public ItemDrop Drop(RandomNumberGenerator rng)
    {
        float commonLootChance = 1.0f - DefaultChanceRareInDecimal - DefaultChanceUniqueInDecimal;
        float rareLootThreshold = commonLootChance + DefaultChanceRareInDecimal;

        float randomDecimal = rng.Randf();

        if (randomDecimal >= commonLootChance && randomDecimal < rareLootThreshold)
        {
            return RareLootDrop.Drop(rng);
        }
        else if (randomDecimal >= rareLootThreshold)
        {
            return UniqueLootDrop.Drop(rng);
        }

        return CommonLootDrop.Drop(rng);
    }
}