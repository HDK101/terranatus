using Godot;
using System;

public partial class SkillSlot : TextureRect
{
    private Label levelLabel;

    public override void _Ready()
    {
        levelLabel = GetNode<Label>("LevelLabel");
    }

    public void Update(int level)
    {
        levelLabel.Text = level.ToString();
    }
}