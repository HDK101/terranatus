using Godot;
using System;

public partial class GameHud : CanvasLayer
{
	public ItemNotification ItemNotification => _itemNotification;
	public HealthBar HealthBar => _healthBar;
	public BlackBars BlackBars => _blackBars;
	public Menu Menu => _menu;
	public Combo Combo => _combo;

	private ItemNotification _itemNotification;
	private HealthBar _healthBar;

	[Export]
	private Combo _combo;

	[Export]
	private DialogBox _dialogBox;

	[Export]
	private BlackBars _blackBars;

	[Export]
	private Menu _menu;

	[Export]
	private Player player;

    public override void _Ready()
	{
		DialogDB dialogDB = GetNode<DialogDB>("/root/DialogDB");
		DialogTree tree = dialogDB.RetrieveTree("BASIC_TREE");

		_menu.UIVisible += _blackBars.ShowBars;
		_menu.UIHidden += _blackBars.HideBars;
		_healthBar = GetNode<HealthBar>("MainUI/HealthBar");
		_itemNotification = GetNode<ItemNotification>("MainUI/ItemNotification");

		player.Life.Change += () =>
		{
			_healthBar.Update(player.Life.Value, player.Life.MaxValue);
			_healthBar.Hit();
		};

		GetTree().CreateTimer(5.0).Timeout += () =>
		{
			_dialogBox.PlayTree(tree);
		};
	}
}
