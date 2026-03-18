using Godot;
using System;

public partial class PlayerRespawningState(WeakReference<Player> playerRef) : IState
{
    private IState nextState = null;

    public IState NextState()
    {
        return nextState;
    }

    public void PhysicsProcess(double delta)
    {
    }

    public void Process(double delta)
    {
    }

    public void Start()
    {
        if (playerRef.TryGetTarget(out Player player))
        {
            player.View.StartRespawn();
            player.View.StartSleeping();
            player.EntitySoundPlayer.PlayEnergyCharging();
            player.GetTree().CreateTimer(3.5).Timeout += () =>
            {
                player.EntitySoundPlayer.PlayEnergyCharge();
                player.View.Respawning();
                player.LightsEffect.Stop();
            };
            player.GetTree().CreateTimer(3.6).Timeout += () =>
            {
                var explosionInstance = player.PackedSceneDB.GlowingParticlesRespawnExplosion.Instantiate<GpuParticles2D>();
                explosionInstance.OneShot = true;
                player.AddChild(explosionInstance);

                var bigExplosionInstance = player.PackedSceneDB.BigWhiteExplosion.Instantiate<BigWhiteExplosion>();
                player.AddChild(bigExplosionInstance);

                nextState = new PlayerDefaultState(playerRef);
                player.RespawnParticles.Emitting = false;
                player.EnergyOrbParticles.Emitting = false;
                player.EmitSignal(Player.SignalName.Respawned);
            };
        }
    }
}