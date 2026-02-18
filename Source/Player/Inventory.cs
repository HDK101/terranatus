using System.Collections.Generic;
using System.Linq;

public partial class Inventory
{
    public static readonly int DEFAULT_PAGE_SIZE = 24;

    public List<Slot> Weapons => _weapons;
    public List<Slot> Consumables => _consumables;

    private readonly List<Slot> _weapons = [];
    private readonly List<Slot> _consumables = [];

    public void Add(ItemBlueprint blueprint, int quantity)
    {
        Slot slot = new();
        slot.Insert(blueprint, quantity);
        if (blueprint.Type == ItemTypes.WEAPON)
        {
            _weapons.Add(slot);
        } else if (blueprint.Type == ItemTypes.CONSUMABLE)
        {
            _consumables.Add(slot);
        }
    }

    public List<Slot> ListWeaponsPagination(int page = 0, int size = 24)
    {
        int skipAmount = page * size;
        return [.. _weapons.Skip(skipAmount).Take(size)];
    }
}