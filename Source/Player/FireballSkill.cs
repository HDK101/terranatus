using Godot;
using System;

public partial class FireballSkill : ActiveSkill
{
    public override int ManaCost => 2;
    private readonly WeakReference<Player> playerRef;

    public FireballSkill(WeakReference<Player> playerRef)
    {
        MenuTexture = GD.Load<Texture2D>("res://Sprites/Skills/fireball_skill1.png");
        QuickSlotTexture = GD.Load<Texture2D>("res://Sprites/Skills/fireball_skill_ingame.png");
        this.playerRef = playerRef;
    }

    protected override void Action(CastSkillPayload payload)
    {
        if (playerRef.TryGetTarget(out Player player))
        {
            player.Combat.CastFireball();
        }
    }
}