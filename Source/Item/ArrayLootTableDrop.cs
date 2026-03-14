using Godot;
using Godot.Collections;
using System.Collections.Generic;

[GlobalClass]
public partial class ArrayLootTableDrop : Resource, ILootTableDrop
{
    [Export]
    private Array<ItemDrop> items;

    public ArrayLootTableDrop() { }

    public ArrayLootTableDrop(Array<ItemDrop> items)
    {
        this.items = items;
    }

    public ItemDrop Drop(RandomNumberGenerator rng)
    {
        List<float> weights = [];
        foreach (var item in items)
        {
            weights.Add(item.Weight);
        }

        int randomIndex = (int)rng.RandWeighted([.. weights]);
        return items[randomIndex];
    }

    public Array<ItemDrop> Items()
    {
        return items;
    }
}