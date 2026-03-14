using Godot;

public partial class QuickSlot : RefCounted
{
    [Signal]
    public delegate void ConsumableUseEventHandler(Slot slot);

    [Signal]
    public delegate void ChangeEventHandler();

    [Signal]
    public delegate void UsedEventHandler();

    public enum SlotType
    {
        UNSET,
        SKILL,
        CONSUMABLE,
    }

    public ActiveSkill ActiveSkill
    {
        get => activeSkill; set
        {
            activeSkill = value;

            if (slot is not null)
            {
                slot.QuantityChange -= OnSlotQuantityChange;
            }

            slot = null;
            Type = SlotType.SKILL;
            EmitSignal(SignalName.Change);
        }
    }

    public Slot Slot
    {
        get => slot; set
        {
            slot = value;
            slot.QuantityChange += OnSlotQuantityChange;
            activeSkill = null;
            Type = SlotType.CONSUMABLE;
            EmitSignal(SignalName.Change);
        }
    }

    public Texture2D InGameTexture
    {
        get
        {
            return Type switch
            {
                SlotType.CONSUMABLE => slot.Blueprint.Texture,
                SlotType.SKILL => activeSkill.QuickSlotTexture,
                _ => null,
            };
        }
    }

    public Texture2D MenuTexture
    {
        get
        {
            return Type switch
            {
                SlotType.CONSUMABLE => slot.Blueprint.Texture,
                SlotType.SKILL => activeSkill.MenuTexture,
                _ => null,
            };
        }
    }

    public SlotType Type { get => type; set => type = value; }


    private ActiveSkill activeSkill;
    private Slot slot;

    private SlotType type = SlotType.UNSET;

    public void Use()
    {
        switch (Type)
        {
            case SlotType.SKILL:
                activeSkill.Cast(new());
                EmitSignal(SignalName.Used);
                break;
            case SlotType.CONSUMABLE:
                EmitSignal(SignalName.ConsumableUse, slot);
                EmitSignal(SignalName.Used);
                if (slot.Quantity == 0)
                {
                    Type = SlotType.UNSET;
                    activeSkill = null;
                    slot = null;
                }
                break;
        }
    }

    private void OnSlotQuantityChange()
    {
        EmitSignal(SignalName.Change);
    }
}