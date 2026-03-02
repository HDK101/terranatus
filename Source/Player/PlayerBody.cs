using Godot;
using System.Collections.Generic;

public partial class PlayerBody : RefCounted
{
    public enum ItemType
    {
        WEAPON,
        ARMOR,
        RING,
    }

    private readonly Dictionary<ItemType, Slot> equipped = [];

    public PlayerBody()
    {
        equipped[ItemType.WEAPON] = new Slot();
        equipped[ItemType.ARMOR] = new Slot();
        equipped[ItemType.RING] = new Slot();
    }

    public void Equip(ItemType slot, ItemBlueprint blueprint)
    {
        if (equipped.TryGetValue(slot, out Slot itemSlot))
        {
            itemSlot.Insert(blueprint, 1);
        }
    }

    public ItemBlueprint Retrieve(ItemType slot)
    {
        return equipped.GetValueOrDefault(slot, null)?.Blueprint;
    }
}