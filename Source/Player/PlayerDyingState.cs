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
            player.View.Dying();
            player.EntitySoundPlayer.PlayEnergyCharging();
            player.GetTree().CreateTimer(BASE_DEATH_TIME).Timeout += () =>
            {
                player.EntitySoundPlayer.PlayEnergyCharge();
                player.View.HideCharacter();
            };
            player.GetTree().CreateTimer(BASE_DEATH_TIME + 0.1f).Timeout += player.View.DeathExplosion;
        }
    }

}