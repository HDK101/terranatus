using Godot;
using Godot.Collections;

public partial class MenuV2 : Control, MenuElement
{
    public enum Tab
    {
        INVENTORY,
        SKILL,
        CHARACTER,
        SYSTEM,
        MAX,
    }

    [Signal]
    public delegate void UIVisibleEventHandler();

    [Signal]
    public delegate void UIHiddenEventHandler();


    [Signal]
    public delegate void ChangeEventHandler();

    [Signal]
    public delegate void AcceptEventHandler();

    [Export]
    public Control Inventory { get; private set; }
    [Export]
    public Control Skill { get; private set; }
    [Export]
    public Control Character { get; private set; }

    [Export]
    public ColorRect Background { get; private set; }

    [Export]
    private MenuButton inventoryButton;
    [Export]
    private MenuButton skillsButton;
    [Export]
    private MenuButton characterButton;
    [Export]
    private MenuButton systemButton;

    private Dictionary<Tab, MenuButton> buttons;

    private Tab currentTab;

    private bool blocked = false;

    public override void _Ready()
    {
        inventoryButton.Unselect();
        skillsButton.Unselect();
        characterButton.Unselect();
        systemButton.Unselect();

        buttons = new()
        {
            {Tab.INVENTORY, inventoryButton},
            {Tab.SKILL, skillsButton},
            {Tab.CHARACTER, characterButton},
            {Tab.SYSTEM, systemButton},
        };
    }

    public override void _Input(InputEvent @event)
    {
        if (blocked) return;

        if (@event.IsActionPressed("previous_menu"))
        {
            int tabIndex = (int)currentTab;
            tabIndex -= 1;
            int maxSize = (int)Tab.MAX;
            tabIndex = (maxSize + (tabIndex % maxSize)) % maxSize;

            ChangeTab((Tab)tabIndex);
        }
        else if (@event.IsActionPressed("next_menu"))
        {
            int tabIndex = (int)currentTab;
            tabIndex += 1;
            int maxSize = (int)Tab.MAX;
            tabIndex %= maxSize;

            ChangeTab((Tab)tabIndex);
        }
    }

    public void ChangeTab(Tab tab)
    {
        blocked = true;
        GetTree().CreateTimer(0.7).Timeout += () => blocked = false;

        if (tab != currentTab) HideCurrentTab();

        buttons[currentTab].Unselect();
        currentTab = tab;
        buttons[tab].Select();

        switch (currentTab)
        {
            case Tab.INVENTORY:
                ((MenuElement)Inventory).ShowElement();
                break;
            case Tab.CHARACTER:
                ((MenuElement)Character).ShowElement();
                break;
            case Tab.SKILL:
                ((MenuElement)Skill).ShowElement();
                break;
        }
    }

    public void HideCurrentTab()
    {
        switch (currentTab)
        {
            case Tab.INVENTORY:
                ((MenuElement)Inventory).HideElement();
                break;
            case Tab.CHARACTER:
                ((MenuElement)Character).HideElement();
                break;
            case Tab.SKILL:
                ((MenuElement)Skill).HideElement();
                break;
        }
    }

    public void ShowElement()
    {
        SetProcessInput(true);
        Show();
        ChangeTab(Tab.INVENTORY);
        FadeInBackground();
    }

    public void HideElement()
    {
        var tween = FadeOutBackground();
        ChangeTab(Tab.INVENTORY);
        tween.TweenCallback(Callable.From(Hide));
        tween.TweenCallback(Callable.From(() => SetProcessInput(false)));
    }

    private void FadeInBackground()
    {
        var tween = CreateTween().SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic);
        tween.TweenProperty(Background, "color:a", 0.9f, 0.5f);
    }
    private Tween FadeOutBackground()
    {
        var tween = CreateTween().SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic);
        tween.TweenProperty(Background, "color:a", 0.0f, 0.5f);
        return tween;
    }
}