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
            };
            player.GetTree().CreateTimer(3.6).Timeout += () =>
            {
                nextState = new PlayerDefaultState(playerRef);
                player.View.StopRespawn();
                CreateExplosion(player);
                player.EmitSignal(Player.SignalName.Respawned);
            };
        }
    }

    private void CreateExplosion(Player player)
    {
        var explosionInstance = player.PackedSceneDB.GlowingParticlesRespawnExplosion.Instantiate<GpuParticles2D>();
        explosionInstance.OneShot = true;
        player.AddChild(explosionInstance);

        var bigExplosionInstance = player.PackedSceneDB.BigWhiteExplosion.Instantiate<BigWhiteExplosion>();
        player.AddChild(bigExplosionInstance);
    }
}