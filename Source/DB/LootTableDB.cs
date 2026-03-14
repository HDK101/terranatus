using Godot;
using System;


public partial class LootTableDB : Node
{
    public ArrayLootTableDrop Common;
    public ArrayLootTableDrop Rare;

    public override void _Ready()
    {
        ItemDB itemDB = GetNode<ItemDB>("/root/ItemDB");

        ItemDrop shortSword = new()
        {
            Blueprint = itemDB.Retrieve("SHORT_SWORD")
        };

        Common = new([
            shortSword,
        ]);

        Rare = new([
            shortSword,
        ]);
    }
}