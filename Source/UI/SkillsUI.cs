using Godot;
using System;
using System.Collections.Generic;

public partial class SkillsUI : Control, MenuElement
{
    public enum Path
    {
        MOON = 0,
        WARRIOR = 1,
        ROGUE = 2,
        MAGE = 3,
        MAX = 4,
    }

    public enum State
    {
        LIST,
        QUICK_SLOT,
    }

    [Signal]
    public delegate void AssignEventHandler(SkillButton button, ActiveSkill activeSkill);

    [Export]
    private SkillSlot fireball;
    [Export]
    private SkillSlot forwardSlash;
    [Export]
    private Player player;

    [Export]
    private TextureRect selectedSkillRect;

    private Path currentPath = Path.MOON;


    private List<SkillSlot> moonPath = [];
    private List<SkillSlot> warriorPath = [];
    private List<SkillSlot> roguePath = [];
    private List<SkillSlot> magePath = [];

    private int selectedSkillIndex = 0;
    private Dictionary<Path, int> pathSizes;

    private ActiveSkill currentSkill;

    private QuickSlotSelect quickSlotSelect;

    private List<SkillSlot> currentSkillSlots = null;

    private State state = State.LIST;

    private MenuAudioPlayer menuAudioPlayer;

    public override async void _Ready()
    {
        menuAudioPlayer = new();
        AddChild(menuAudioPlayer);

        await ToSignal(player, Node.SignalName.Ready);
        Start();
    }

    public override void _Input(InputEvent @event)
    {
        switch (state)
        {
            case State.LIST:
                Input(@event);
                break;
            case State.QUICK_SLOT:
                quickSlotSelect.Input(@event);
                break;
        }
    }

    public void Input(InputEvent inputEvent)
    {
        if (inputEvent.IsActionPressed("ui_up"))
        {
            ChangePath((int)currentPath - 1);
        }
        else if (inputEvent.IsActionPressed("ui_down"))
        {
            ChangePath((int)currentPath + 1);
        }

        if (inputEvent.IsActionPressed("ui_left"))
        {
            ChangeSelectedSkill(selectedSkillIndex - 1);
        }
        else if (inputEvent.IsActionPressed("ui_right"))
        {
            ChangeSelectedSkill(selectedSkillIndex + 1);
        }

        if (inputEvent.IsActionPressed("ui_accept"))
        {
            ShowQuickSlotSelect();
        }
    }

    public void ShowElement()
    {
        SetProcess(true);
        SetProcessInput(true);

        state = State.LIST;

        selectedSkillRect.Hide();
        MenuElementUtils.SlideIn(this).Chain().TweenCallback(Callable.From(InitSelectSkill));
        Show();
    }

    public void HideElement()
    {
        MenuElementUtils.SlideOut(this).Chain().TweenCallback(Callable.From(Hide));

        quickSlotSelect.HideElement();

        state = State.LIST;

        SetProcess(false);
        SetProcessInput(false);
    }

    private void ShowQuickSlotSelect()
    {
        quickSlotSelect.CurrentSkill = currentSkillSlots[selectedSkillIndex].Skill;
        quickSlotSelect.ShowElement();

        quickSlotSelect.SlotOne.UpdateTexture(player.QuickSlots.SlotOne);
        quickSlotSelect.SlotTwo.UpdateTexture(player.QuickSlots.SlotTwo);
        quickSlotSelect.SlotThree.UpdateTexture(player.QuickSlots.SlotThree);

        state = State.QUICK_SLOT;

        menuAudioPlayer.PlayAccept();
    }

    private void ChangeSelectedSkill(int index)
    {
        int pathSize = pathSizes[currentPath];
        selectedSkillIndex = (pathSize + (index % pathSize)) % pathSize;

        MoveSkillRect(selectedSkillIndex);
    }

    private void MoveSkillRect(int index)
    {
        if (currentPath == Path.WARRIOR)
        {
            currentSkillSlots = warriorPath;
        }
        else if (currentPath == Path.MAGE)
        {
            currentSkillSlots = magePath;
        }
        else if (currentPath == Path.ROGUE)
        {
            currentSkillSlots = roguePath;
        }
        else if (currentPath == Path.MOON)
        {
            currentSkillSlots = moonPath;
        }
        Vector2 selectedPosition = currentSkillSlots[index].GlobalPosition;
        Tween tween = CreateTween().SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic);
        tween.TweenProperty(selectedSkillRect, "global_position", selectedPosition, 0.15f);
    }

    private void ChangePath(int index)
    {
        int currentPathIndex = index;
        int pathLength = (int)Path.MAX;
        int newPathIndex = (pathLength + (currentPathIndex % pathLength)) % pathLength;

        currentPath = (Path)newPathIndex;

        selectedSkillIndex = 0;
        MoveSkillRect(0);

        menuAudioPlayer.PlayChange();
    }

    private void Start()
    {
        quickSlotSelect = GetNode<QuickSlotSelect>("QuickSlotSelect");

        forwardSlash.Skill = player.Skills.ForwardSlash;
        fireball.Skill = player.Skills.Fireball;

        warriorPath = [forwardSlash];
        moonPath = [forwardSlash];
        roguePath = [fireball];
        magePath = [fireball];

        var skills = player.Skills;
        skills.Fireball.LevelUp += fireball.Update;
        skills.ForwardSlash.LevelUp += forwardSlash.Update;

        forwardSlash.Update(skills.ForwardSlash.Level);
        fireball.Update(skills.Fireball.Level);

        pathSizes = new()
        {
            {Path.MOON, moonPath.Count},
            {Path.WARRIOR, warriorPath.Count},
            {Path.ROGUE, roguePath.Count},
            {Path.MAGE, magePath.Count},
        };

        quickSlotSelect.SlotOne.Assigned += () => AssignSkillToSlot(player.QuickSlots.SlotOne);
        quickSlotSelect.SlotTwo.Assigned += () => AssignSkillToSlot(player.QuickSlots.SlotTwo);
        quickSlotSelect.SlotThree.Assigned += () => AssignSkillToSlot(player.QuickSlots.SlotThree);

        SetProcess(false);
        SetProcessInput(false);
    }

    private void AssignSkillToSlot(QuickSlot slot)
    {
        slot.ActiveSkill = currentSkillSlots[selectedSkillIndex].Skill;
        state = State.LIST;
    }

    private void InitSelectSkill()
    {
        selectedSkillRect.Show();
        selectedSkillRect.Modulate = new Color(selectedSkillRect.Modulate, 0.0f);
        Tween tween = CreateTween().SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic);

        tween.TweenProperty(selectedSkillRect, "modulate:a", 1.0f, 0.1f);
        MoveSkillRect(0);
    }
}