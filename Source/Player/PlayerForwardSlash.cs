using Godot;
using System;

public partial class PlayerForwardSlash(WeakReference<Player> playerRef) : RefCounted, IState
{
    public float InitialDirection { get; init; }

    private readonly float FORWARD_SPEED = 192.0f;
    private Vector2 gravity = new(0.0f, -48.0f);

    private float direction = 0.0f;

    private IState state = null;

    public IState NextState()
    {
        return state;
    }

    public void PhysicsProcess(double delta)
    {
        if (playerRef.TryGetTarget(out var player))
        {
            player.Velocity = new Vector2(FORWARD_SPEED * direction, 0.0f) + gravity;
            player.MoveAndSlide();
            gravity += player.GetGravity() * (float)delta;
        }
    }

    public void Process(double delta)
    {
    }

    public void Start()
    {
        direction = InitialDirection;
        if (playerRef.TryGetTarget(out var player))
        {
            player.View.ShadowsActive = true;
            player.PlayForwardSlashAnimation();
            var tween = player.CreateTween();
            tween.SetPauseMode(Tween.TweenPauseMode.Process);
            tween.SetEase(Tween.EaseType.InOut);
            tween.SetTrans(Tween.TransitionType.Quint);
            tween.TweenProperty(this, nameof(direction), 0.0f, 0.25f);
            tween.TweenCallback(Callable.From(Finish));
        }
    }

    private void Finish()
    {
        if (playerRef.TryGetTarget(out var player))
        {
            player.View.ShadowsActive = false;
            player.Combat.ForwardSlashAttack();
        }

        state = new PlayerDefaultState(playerRef);
    }
}