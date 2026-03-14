using Godot;
using System;
using System.Collections.Generic;

public partial class QuickSlotSelect : ColorRect, MenuElement
{
    public ActiveSkill CurrentSkill { get; set; }

    [Export]
    public QuickSlotAssign SlotOne { get; private set; }

    [Export]
    public QuickSlotAssign SlotTwo { get; private set; }

    [Export]
    public QuickSlotAssign SlotThree { get; private set; }

    [Export]
    private Control slotSelectRect;

    private List<QuickSlotAssign> quickSlots;
    private int quickSlotIndex = 0;

    private MenuAudioPlayer menuAudioPlayer;

    public override void _Ready()
    {
        menuAudioPlayer = new();
        AddChild(menuAudioPlayer);
        quickSlots = [SlotOne, SlotTwo, SlotThree];
        SetProcess(false);
        SetProcessInput(false);
    }

    public void Input(InputEvent inputEvent)
    {
        if (inputEvent.IsActionPressed("ui_left"))
        {
            ChangeIndex(-1);
        }
        else if (inputEvent.IsActionPressed("ui_right"))
        {
            ChangeIndex(1);
        }

        if (inputEvent.IsActionPressed("ui_accept"))
        {
            Assign();
        }
    }

    public void ShowElement()
    {
        quickSlotIndex = 0;
        Show();
        slotSelectRect.GlobalPosition = quickSlots[0].GlobalPosition;
        slotSelectRect.Hide();
        MenuElementUtils.FadeIn(this, 0.1f).Chain().TweenCallback(Callable.From(StartSlotSelectRect));

        SetProcess(true);
        SetProcessInput(true);
    }

    public void HideElement()
    {
        MenuElementUtils.FadeOut(this).Chain().TweenCallback(Callable.From(Hide));
        SetProcess(false);
        SetProcessInput(false);
    }

    private void Assign()
    {
        quickSlots[quickSlotIndex].Assign();
        HideElement();
        menuAudioPlayer.PlayAccept();
    }

    private void ChangeIndex(int amount)
    {
        quickSlotIndex = (quickSlots.Count + ((quickSlotIndex + amount) % quickSlots.Count)) % quickSlots.Count;
    
        menuAudioPlayer.PlayChange();

        Tween tween = CreateTween().SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic);
        tween.TweenProperty(slotSelectRect, "global_position", quickSlots[quickSlotIndex].GlobalPosition, 0.5f);
    }

    private void StartSlotSelectRect()
    {
        slotSelectRect.Show();
        slotSelectRect.GlobalPosition = SlotOne.GlobalPosition;
        MenuElementUtils.FadeIn(slotSelectRect, 0.2f);
    }
}