using Godot;
using System;

public partial class MenuStatBar : HBoxContainer
{
    private TextureProgressBar textureProgressBar;
    private Label label;

    public override void _Ready()
    {
        textureProgressBar = GetNode<TextureProgressBar>("TextureProgressBar");
        label = GetNode<Label>("Value");
    }

    public void Update(double value, double maxValue)
    {
        textureProgressBar.Value = value;
        textureProgressBar.MaxValue = maxValue;
        label.Text = $"{value}/{maxValue}";
    }
}