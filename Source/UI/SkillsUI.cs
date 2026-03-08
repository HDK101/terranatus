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

	private int selectedSkillIndex = 0;

	private List<SkillSlot> moonPath = [];
	private List<SkillSlot> warriorPath = [];
	private List<SkillSlot> roguePath = [];
	private List<SkillSlot> magePath = [];

	private Dictionary<Path, int> pathSizes;

	public override async void _Ready()
	{
		await ToSignal(player, Node.SignalName.Ready);
		Start();
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_up"))
		{
			ChangePath((int)currentPath - 1);
		}
		else if (@event.IsActionPressed("ui_down"))
		{
			ChangePath((int)currentPath + 1);
		}

		if (@event.IsActionPressed("ui_left"))
		{
			ChangeSelectedSkill(selectedSkillIndex - 1);
		}
		else if (@event.IsActionPressed("ui_right"))
		{
			ChangeSelectedSkill(selectedSkillIndex + 1);
		}
	}

	public void ShowElement()
	{
		SetProcess(true);
		SetProcessInput(true);

		MenuElementUtils.SlideIn(this);
		Show();
	}

	public void HideElement()
	{
		MenuElementUtils.SlideOut(this).Chain().TweenCallback(Callable.From(Hide));

		SetProcess(false);
		SetProcessInput(false);
	}

	private void ChangeSelectedSkill(int index)
	{
		int pathSize = pathSizes[currentPath];
		selectedSkillIndex = (pathSize + (index % pathSize)) % pathSize;

		MoveSkillRect(selectedSkillIndex);
	}

	private void MoveSkillRect(int index)
	{
		List<SkillSlot> skillSlots = null;
		if (currentPath == Path.WARRIOR)
		{
			skillSlots = warriorPath;
		}
		else if (currentPath == Path.MAGE)
		{
			skillSlots = magePath;
		}
		else if (currentPath == Path.ROGUE)
		{
			skillSlots = roguePath;
		}
		else if (currentPath == Path.MOON)
		{
			skillSlots = moonPath;
		}
		Vector2 selectedPosition = skillSlots[index].GlobalPosition;
		Tween tween = CreateTween().SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic);
		tween.TweenProperty(selectedSkillRect, "position", selectedPosition, 0.25f);
	}

	private void ChangePath(int index)
	{
		int currentPathIndex = index;
		int pathLength = (int)Path.MAX;
		int newPathIndex = (pathLength + (currentPathIndex % pathLength)) % pathLength;

		currentPath = (Path)newPathIndex;

		selectedSkillIndex = 0;
		MoveSkillRect(0);
	}

	private void Start()
	{
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

		CallDeferred(nameof(InitSelectSkill));

		SetProcess(false);
		SetProcessInput(false);
	}

	private void InitSelectSkill()
	{
		selectedSkillRect.Position = moonPath[0].GlobalPosition;
		GD.Print(moonPath[0].Position);
	}
}
