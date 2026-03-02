using Godot;
using System;

public partial class FireballSkill(WeakReference<Player> playerRef) : ActiveSkill
{
    public override int ManaCost => 2;

    protected override void Action(CastSkillPayload payload)
    {
        if (playerRef.TryGetTarget(out Player player))
        {
            player.CastFireball();
        }
    }
}