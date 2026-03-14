using Godot;

[GlobalClass]
public partial class MixedLootTableDrop : Resource
{
    [Export]
    public ArrayLootTableDrop CommonLootDrop;

    [Export]
    public ArrayLootTableDrop RareLootDrop;

    [Export]
    public float DefaultChanceRareInDecimal = 0.10f;

    [Export]
    public float DefaultChanceUniqueInDecimal = 0.02f;

    public ItemDrop Drop(RandomNumberGenerator rng)
    {
        float commonLootChance = 1.0f - DefaultChanceRareInDecimal - DefaultChanceUniqueInDecimal;
        float rareLootThreshold = commonLootChance + DefaultChanceRareInDecimal;

        float randomDecimal = rng.Randf();

        if (randomDecimal >= commonLootChance && randomDecimal < rareLootThreshold)
        {

        }
        else if (randomDecimal >= rareLootThreshold)
        {

        }

        return CommonLootDrop.Drop(rng);
    }
}