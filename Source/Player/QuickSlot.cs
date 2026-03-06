public class QuickSlot
{
    public enum SlotType
    {
        SKILL,
        CONSUMABLE,
    }

    public ActiveSkill ActiveSkill { get => activeSkill; set => activeSkill = value; }
    public Slot Slot { get => slot; set => slot = value; }


    private ActiveSkill activeSkill;
    private Slot slot;

    public void Use()
    {
        
    }
}