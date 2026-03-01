using System;
using Godot;

public abstract class ActiveSkill
{
	public delegate void WasCastEventHandler();

	public Func<int, bool> HasMana { get; set; }
	public Action<int> UseMana { get; set; }

	public abstract int ManaCost { get; }

	public void Cast(CastSkillPayload payload)
	{
		if (HasMana(ManaCost))
		{
			Action(payload);
			UseMana(ManaCost);
		}
	}

	protected abstract void Action(CastSkillPayload payload);
}
