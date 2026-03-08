using Godot;
using System;
using System.Collections.Generic;

public partial class MenuButtons : Control
{
    [Export]
    public MenuButton InventoryButton { get; set; }
    [Export]
    public MenuButton SkillsButtons { get; set; }
    [Export]
    public MenuButton CharacterButton { get; set; }
    [Export]
    public MenuButton SystemButton { get; set; }

    public List<MenuButton> Buttons { get; private set; }

    public override void _Ready()
    {
        Buttons = [InventoryButton, SkillsButtons, CharacterButton, SystemButton];
    }

    public void ShowButtons()
    {
        InventoryButton.Select();
    }

    public void HideButtons()
    {
        foreach (var button in Buttons)
        {
            button.Unselect();
        }
    }
}