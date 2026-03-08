using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Inventory : RefCounted
{
    [Signal]
    public delegate void ConsumablesChangeEventHandler();

    public Func<ItemBlueprint, bool> ConsumableUseFunc { get; set; }

    public static readonly int DEFAULT_PAGE_SIZE = 24;

    public List<Slot> Weapons => _weapons;
    public List<Slot> Consumables => _consumables;
    public List<Slot> Armors => _armors;

    private readonly List<Slot> _weapons = [];
    private readonly List<Slot> _consumables = [];
    private readonly List<Slot> _armors = [];

    public void Add(ItemBlueprint blueprint, int quantity)
    {
        if (blueprint.Type == ItemTypes.WEAPON)
        {
            InsertOrIncreaseSlot(blueprint, quantity, _weapons);
        }
        else if (blueprint.Type == ItemTypes.CONSUMABLE)
        {
            InsertOrIncreaseSlot(blueprint, quantity, _consumables);
        }
        else if (blueprint.Type == ItemTypes.ARMOR)
        {
            InsertOrIncreaseSlot(blueprint, quantity, _armors);
        }
    }

    public List<Slot> ListWeaponsPagination(int page = 0, int size = 24)
    {
        int skipAmount = page * size;
        return [.. _weapons.Skip(skipAmount).Take(size)];
    }

    public List<Slot> ListConsumablesPagination(int page = 0, int size = 24)
    {
        int skipAmount = page * size;
        return [.. _consumables.Skip(skipAmount).Take(size)];
    }

    public List<Slot> ListArmorsPagination(int page = 0, int size = 24)
    {
        int skipAmount = page * size;
        return [.. _armors.Skip(skipAmount).Take(size)];
    }

    public void UseConsumable(Slot slot)
    {
        bool success = ConsumableUseFunc(slot.Blueprint);

        if (success)
        {
            slot.Decrease();
            FilterConsumablesDepletedItems();
        }
    }

    private void FilterConsumablesDepletedItems()
    {
        var consumables = _consumables.Where(consumable => consumable.Quantity > 0).ToList();
        _consumables.Clear();
        _consumables.AddRange(consumables);
        EmitSignal(SignalName.ConsumablesChange);
    }

    private void InsertOrIncreaseSlot(ItemBlueprint blueprint, int quantity, List<Slot> slots)
    {
        Slot possibleSlot = slots.Find(slot => slot.Blueprint == blueprint);

        if (possibleSlot is not null)
        {
            int previousQuantity = possibleSlot.Quantity;
            int attemptedQuantity = possibleSlot.Quantity + quantity;

            if (attemptedQuantity > blueprint.MaxQuantity)
            {
                int difference = attemptedQuantity - blueprint.MaxQuantity;
                int toFillSlot = blueprint.MaxQuantity - previousQuantity;

                Slot anotherSlot = new();
                possibleSlot.Increase(toFillSlot);
                anotherSlot.Insert(blueprint, difference);

                slots.Add(anotherSlot);
            }
            else
            {
                possibleSlot.Increase(quantity);
            }

            return;
        }
        Slot slot = new();
        slot.Insert(blueprint, quantity);

        slots.Add(slot);
    }
}