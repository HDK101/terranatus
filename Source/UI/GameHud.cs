using Godot;
using System;

public partial class GameHud : CanvasLayer
{
    public ItemNotification ItemNotification => _itemNotification;
    public HealthBar HealthBar => _healthBar;
    public ManaBar ManaBar => _manaBar;
    public EXPBar EXPBar => _expBar;
    public MenuV2 Menu => _menu;
    public Combo Combo => _combo;
    public LevelUpNotification LevelUpNotification => _levelUpNotification;

    private ItemNotification _itemNotification;
    private HealthBar _healthBar;
    private ManaBar _manaBar;
    private EXPBar _expBar;

    [Export]
    private LevelUpNotification _levelUpNotification;

    [Export]
    private Combo _combo;

    [Export]
    private Dialog _dialogBox;

    [Export]
    private MenuV2 _menu;

    [Export]
    private Player player;

    [Export]
    private GameHudAudioPlayer audioPlayer;

    [Export]
    private InGameQuickSlots inGameQuickSlots;

    public async override void _Ready()
    {
        DialogDB dialogDB = GetNode<DialogDB>("/root/DialogDB");
        DialogTree tree = dialogDB.RetrieveTree("BASIC_TREE");

        _menu.UIVisible += audioPlayer.PlayPause;
        _menu.UIHidden += audioPlayer.PlayPause;
        _healthBar = GetNode<HealthBar>("MainUI/HealthBar");
        _manaBar = GetNode<ManaBar>("MainUI/ManaBar");
        _expBar = GetNode<EXPBar>("MainUI/EXPBar");
        _itemNotification = GetNode<ItemNotification>("MainUI/ItemNotification");

        _healthBar.Update(player.Life.Value, player.Life.MaxValue);
        _manaBar.Update(player.Mana.Value, player.Mana.MaxValue);
        _expBar.Update(player.Experience.EXP, player.Experience.GetEXPCeil());
        
        player.Life.Change += () =>
        {
            _healthBar.Update(player.Life.Value, player.Life.MaxValue);
            _healthBar.Hit();
        };

        player.Mana.Change += () =>
        {
            _manaBar.Update(player.Mana.Value, player.Mana.MaxValue);
            _manaBar.Hit();
        };

        player.Experience.Change += () => _expBar.Update(player.Experience.EXP, player.Experience.GetEXPCeil());

        GetTree().CreateTimer(5.0).Timeout += () =>
        {
            _dialogBox.PlayTree(tree);
        };

        _menu.Change += audioPlayer.PlayChange;
        _menu.Accept += audioPlayer.PlayAccept;

        player.QuickSlots.SlotOne.Change += () => inGameQuickSlots.SlotOne.Update(player.QuickSlots.SlotOne);
        player.QuickSlots.SlotTwo.Change += () => inGameQuickSlots.SlotTwo.Update(player.QuickSlots.SlotTwo);
        player.QuickSlots.SlotThree.Change += () => inGameQuickSlots.SlotThree.Update(player.QuickSlots.SlotThree);

        player.QuickSlots.SlotOne.Used += inGameQuickSlots.SlotOne.Use;
        player.QuickSlots.SlotTwo.Used += inGameQuickSlots.SlotTwo.Use;
        player.QuickSlots.SlotThree.Used += inGameQuickSlots.SlotThree.Use;

        await ToSignal(player, Node.SignalName.Ready);

        inGameQuickSlots.SlotOne.Update(player.QuickSlots.SlotOne);
        inGameQuickSlots.SlotTwo.Update(player.QuickSlots.SlotTwo);
        inGameQuickSlots.SlotThree.Update(player.QuickSlots.SlotThree);
    }
}