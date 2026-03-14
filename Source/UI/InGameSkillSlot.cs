using Godot;
using System.Collections.Generic;

public partial class InGameSkillSlot : TextureRect
{
    public enum SkillButton
    {
        JUMP,
        ATTACK,
        SKILL_ONE,
        SKILL_TWO,
        SKILL_THREE,
    };

    [Export]
    public SkillButton Button;

    private AnimationPlayer animationPlayer;
    private string currentAction;

    private float defaultY;

    private TextureRect consumableRect;
    private TextureRect consumableInnerRect;
    private Label quantityLabel;

    private readonly Dictionary<SkillButton, string> buttonActions = new() {
        { SkillButton.JUMP, "jump" },
        { SkillButton.ATTACK, "attack" },
        { SkillButton.SKILL_ONE, "skill_one" },
        { SkillButton.SKILL_TWO, "skill_two" },
        { SkillButton.SKILL_THREE, "skill_three" },
    };

    public override void _Ready()
    {
        defaultY = Position.Y;

        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        currentAction = buttonActions[Button];

        GetNode<Label>("Label").Text = Utils.InputAction.GetPrimaryInputKeyText(currentAction);

        consumableRect = GetNode<TextureRect>("ConsumableFrame");
        consumableInnerRect = GetNode<TextureRect>("ConsumableFrame/TextureRect");
        quantityLabel = GetNode<Label>("QuantityLabel");
    }

    public override void _Input(InputEvent @event)
    {
        var action = buttonActions[Button];
        if (@event.IsActionPressed(action))
        {
            Use();
        }
    }

    public void Update(QuickSlot quickSlot)
    {
        consumableRect.Visible = false;
        quantityLabel.Text = "";

        if (quickSlot.Type == QuickSlot.SlotType.SKILL)
        {
            Texture = quickSlot.InGameTexture;
        }
        else if (quickSlot.Type == QuickSlot.SlotType.CONSUMABLE)
        {
            consumableRect.Visible = true;
            consumableInnerRect.Texture = quickSlot.InGameTexture;
            quantityLabel.Text = quickSlot.Slot.Quantity.ToString();
        }
    }

    public void Use()
    {
        Modulate = new Color(0.7f, 0.7f, 0.7f, 1.0f);
        Position += Vector2.Down * 4f;
        animationPlayer.Play("use");
        Tween tween = CreateTween().SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic).SetParallel();
        tween.TweenProperty(this, "modulate", new Color(1.0f, 1.0f, 1.0f, 1.0f), 0.2f);
        tween.TweenProperty(this, "position:y", defaultY, 0.2f);
    }
}