using Godot;
using System;
using System.Collections.Generic;

public partial class InGameSkillSlot : TextureRect
{
	public enum SkillButton
	{
		JUMP,
		ATTACK,
		SKILL_ONE,
		SKILL_TWO,
		SKILL_THREE,
	};

	[Export]
	public SkillButton Button;

	public record SkillButtonAction(SkillButton Button, string Action);
	private AnimationPlayer animationPlayer;
	private string currentAction;

	private float defaultY;

	private readonly Dictionary<SkillButton, string> buttonActions = new() {
		{ SkillButton.JUMP, "jump" },
		{ SkillButton.ATTACK, "attack" },
		{ SkillButton.SKILL_ONE, "skill_one" },
		{ SkillButton.SKILL_TWO, "skill_two" },
		{ SkillButton.SKILL_THREE, "skill_three" },
	};

	public override void _Ready()
	{
		defaultY = Position.Y;

		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		currentAction = buttonActions[Button];

		GetNode<Label>("Label").Text = Utils.InputAction.GetPrimaryInputKeyText(currentAction);
	}

	public override void _Input(InputEvent @event)
	{
		var action = buttonActions[Button];
		if (@event.IsActionPressed(action))
		{
			Use();
		}
	}

	public void Use()
	{
		Modulate = new Color(0.7f, 0.7f, 0.7f, 1.0f);
		Position += Vector2.Down * 4f;
		animationPlayer.Play("use");
		Tween tween = CreateTween().SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic).SetParallel();
		tween.TweenProperty(this, "modulate", new Color(1.0f, 1.0f, 1.0f, 1.0f), 0.2f);
		tween.TweenProperty(this, "position:y", defaultY, 0.2f);
	}
}
