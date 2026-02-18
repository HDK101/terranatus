using System.Collections.Generic;

public partial class PlayerBody
{
    public enum ItemType
    {
        WEAPON,
        ARMOR,
        RING,
    }

    private readonly Dictionary<ItemType, Slot> equipped = [];

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