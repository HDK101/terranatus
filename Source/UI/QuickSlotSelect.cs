using Godot;
using System;

public partial class QuickSlotSelect : ColorRect
{
    public ActiveSkill currentSkill;

    [Export]
    private SkillButton skillButton;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public void ShowUI(ActiveSkill activeSkill)
    {
        currentSkill = activeSkill;
    }
}