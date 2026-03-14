using Godot;

public partial class QuickSlotAssign : TextureRect
{
    [Signal]
    public delegate void AssignedEventHandler();

    [Export]
    private SkillButton button;

    private TextureRect consumableFrame;
    private TextureRect consumableInnerTexture;

    public override void _Ready()
    {
        GetNode<Label>("Label").Text = Utils.InputAction.GetPrimaryInputKeyText(Utils.Skills.ButtonActions[button]);
        consumableFrame = GetNode<TextureRect>("ConsumableFrame");
        consumableInnerTexture = GetNode<TextureRect>("ConsumableFrame/TextureRect");
    }

    public void Assign()
    {
        EmitSignal(SignalName.Assigned);
    }

    public void UpdateTexture(QuickSlot slot)
    {
        consumableFrame.Hide();
        if (slot.Type == QuickSlot.SlotType.SKILL)
        {
            Texture = slot.MenuTexture;
        }
        else if (slot.Type == QuickSlot.SlotType.CONSUMABLE)
        {
            consumableFrame.Show();
            consumableInnerTexture.Texture = slot.MenuTexture;
        }
    }
}