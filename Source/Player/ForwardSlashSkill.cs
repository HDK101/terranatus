using Godot;
using System;

public partial class ForwardSlashSkill(WeakReference<Player> playerRef) : ActiveSkill
{
    public override int ManaCost => 1;

    protected override void Action(CastSkillPayload payload)
    {
        if (playerRef.TryGetTarget(out Player player))
        {
            player.StartForwardSlash();
        }
    }
}