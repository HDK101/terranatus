using Godot;
using System;

public class PlayerDyingState(WeakReference<Player> playerRef) : IState
{
    private readonly float BASE_DEATH_TIME = 3.5f;

    public IState NextState()
    {
        return null;
    }

    public void PhysicsProcess(double delta)
    {
        if (playerRef.TryGetTarget(out Player player))
        {
            player.Velocity = Vector2.Zero;
            player.MoveAndSlide();
        }
    }

    public void Process(double delta)
    {
    }

    public void Start()
    {
        if (playerRef.TryGetTarget(out Player player))
        {
            player.RespawnParticles.Restart();
            player.View.Dying();
            player.EntitySoundPlayer.PlayEnergyCharging();
            player.GetTree().CreateTimer(BASE_DEATH_TIME).Timeout += () => {
                player.EntitySoundPlayer.PlayEnergyCharge();
                player.View.HideCharacter();
            };
            player.GetTree().CreateTimer(BASE_DEATH_TIME + 0.1f).Timeout += () =>
            {
                var explosionInstance = player.PackedSceneDB.GlowingParticlesRespawnExplosion.Instantiate<GpuParticles2D>();
                explosionInstance.OneShot = true;
                var bigExplosionInstance = player.PackedSceneDB.DeathBigGlowExplosion.Instantiate<GpuParticles2D>();
                bigExplosionInstance.OneShot = true;
                player.AddChild(explosionInstance);
                player.AddChild(bigExplosionInstance);
                player.RespawnParticles.Emitting = false;
            };
        }
    }

}