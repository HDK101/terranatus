using Godot;
using System;

public partial class ForwardSlashSkill : ActiveSkill
{
    private readonly WeakReference<Player> playerRef;

    public ForwardSlashSkill(WeakReference<Player> playerRef)
    {
        this.playerRef = playerRef;
        MenuTexture = GD.Load<Texture2D>("res://Sprites/Skills/forward_slash_skill_slot.png");
        QuickSlotTexture = GD.Load<Texture2D>("res://Sprites/Skills/forward_slash_skill_ingame.png");
    }

    public override int ManaCost => 1;

    protected override void Action(CastSkillPayload payload)
    {
        if (playerRef.TryGetTarget(out Player player))
        {
            player.StartForwardSlash();
        }
    }
}