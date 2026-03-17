using Godot;
using System;


public partial class LootTableDB : Node
{
    public ArrayLootTableDrop Common { get; private set; }
    public ArrayLootTableDrop Rare { get; private set; }

    public MixedLootTableDrop DefaultLootDrop { get; private set; }

    public override void _Ready()
    {
        ItemDB itemDB = GetNode<ItemDB>("/root/ItemDB");

        ItemDrop apple = new()
        {
            Blueprint = itemDB.Retrieve(ItemIds.Apple)
        };

        ItemDrop shortSword = new()
        {
            Blueprint = itemDB.Retrieve(ItemIds.ShortSword)
        };

        Common = new([
            shortSword,
            apple,
        ]);

        Rare = new([
            shortSword,
            apple
        ]);

        DefaultLootDrop = new()
        {
            CommonLootDrop = Common,
            RareLootDrop = Rare,
            UniqueLootDrop = Rare,
        };
    }
}