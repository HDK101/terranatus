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

    [ExportGroup("In game")]
    [Export]
    private Player player;

    [ExportGroup("UI")]
    [Export]
    public Control Inventory { get; private set; }
    [Export]
    public Control Skill { get; private set; }
    [Export]
    public Control Character { get; private set; }

    [Export]
    public ColorRect Background { get; private set; }

    [Export]
    public Control TopBar { get; private set; }

    [Export]
    public Control BottomBar { get; private set; }

    [Export]
    private MenuButton inventoryButton;
    [Export]
    private MenuButton skillsButton;
    [Export]
    private MenuButton characterButton;
    [Export]
    private MenuButton systemButton;

    [Export]
    private MenuStatBar healthBar;

    [Export]
    private MenuStatBar manaBar;

    private Dictionary<Tab, MenuButton> buttons;

    private Tab currentTab;

    private bool blocked = false;

    public override void _Ready()
    {
        inventoryButton.Unselect();
        skillsButton.Unselect();
        characterButton.Unselect();
        systemButton.Unselect();

        InitializeHealthAndManaBars();

        buttons = new()
        {
            {Tab.INVENTORY, inventoryButton},
            {Tab.SKILL, skillsButton},
            {Tab.CHARACTER, characterButton},
            {Tab.SYSTEM, systemButton},
        };

        SetProcess(false);
        SetProcessInput(false);
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
        GetTree().CreateTimer(0.4).Timeout += () => blocked = false;

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

    public void OnPause(bool paused)
    {
        if (paused)
        {
            ShowElement();
            return;
        }
        HideElement();
    }

    public void ShowElement()
    {
        Vector2 viewportSize = GetViewport().GetVisibleRect().Size;

        SetProcessInput(true);
        Show();
        ChangeTab(Tab.INVENTORY);
        FadeInBackground();
        Tween barTween = CreateTween().SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic).SetParallel();
        barTween.TweenProperty(TopBar, "position:y", 0.0f, 0.2f);
        barTween.TweenProperty(BottomBar, "position:y", viewportSize.Y - BottomBar.Size.Y, 0.2f);
        EmitSignal(SignalName.UIVisible);

        SetProcess(true);
        SetProcessInput(true);
    }

    public void HideElement()
    {
        Vector2 viewportSize = GetViewport().GetVisibleRect().Size;
        var tween = FadeOutBackground();
        HideCurrentTab();

        Tween barTween = CreateTween().SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic).SetParallel();
        barTween.TweenProperty(TopBar, "position:y", -TopBar.Size.Y, 0.2f);
        barTween.TweenProperty(BottomBar, "position:y", viewportSize.Y, 0.2f);

        tween.TweenCallback(Callable.From(Hide));
        tween.TweenCallback(Callable.From(() => SetProcessInput(false)));

        EmitSignal(SignalName.UIHidden);

        SetProcess(false);
        SetProcessInput(false);
    }

    private void InitializeHealthAndManaBars()
    {
        player.Life.Change += UpdateLife;
        player.Mana.Change += UpdateMana;
        UpdateLife();
        UpdateMana();
    }

    private void UpdateLife()
    {
        healthBar.Update(player.Life.Value, player.Life.MaxValue);
    }

    private void UpdateMana()
    {
        manaBar.Update(player.Mana.Value, player.Mana.MaxValue);
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