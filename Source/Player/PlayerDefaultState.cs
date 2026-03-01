using System;
using Godot;

public class PlayerDefaultState(WeakReference<Player> playerRef) : IState
{
    private IState nextState = null;

    public IState NextState()
    {
        return nextState;
    }

    public void PhysicsProcess(double delta)
    {
        if (playerRef.TryGetTarget(out var player))
        {
            Vector2 velocity = player.Velocity;

            if (!player.IsOnFloor())
            {
                velocity += player.GetGravity() * (float)delta;
            }

            if (player.CurrentState == Player.State.ATTACKING)
            {
                velocity.X = 0;
                player.Velocity = velocity;
                player.MoveAndSlide();
                return;
            }

            if (Input.IsActionJustPressed("jump") && player.IsOnFloor())
            {
                var jumpVelocity = player.Jump();
                velocity.Y = jumpVelocity;
            }

            player.Direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
            if (player.Direction != Vector2.Zero)
            {
                velocity.X = player.Direction.X * Player.Speed;
            }
            else
            {
                velocity.X = Mathf.MoveToward(player.Velocity.X, 0, Player.Speed);
            }

            player.Velocity = velocity;
            player.MoveAndSlide();
        }
    }

    public void Process(double delta)
    {
        if (playerRef.TryGetTarget(out var player))
        {
            var animatedSprite = player.AnimatedSprite;
            var direction = player.Direction;

            if (direction.X < 0.0)
            {
                animatedSprite.FlipH = true;
                player.HitArea.Position = new(-16f, 0.0f);
                player.ForwardSlashArea.Position = new(-20f, 0.0f);
                player.LastDirectionHorizontal = -1.0f;
            }
            if (direction.X > 0.0)
            {
                animatedSprite.FlipH = false;
                player.HitArea.Position = new(16f, 0.0f);
                player.ForwardSlashArea.Position = new(20f, 0.0f);
                player.LastDirectionHorizontal = 1.0f;
            }

            player.ProcessState();
            player.ProcessAttack();
            player.PlayAnimation();

            if (Input.IsActionJustPressed("skill_one") && player.IsOnFloor() && player.CurrentState != Player.State.ATTACKING)
            {
                player.Skills.ForwardSlash.Cast(new());
            }

            if (Input.IsActionJustPressed("skill_two") && player.CurrentState != Player.State.ATTACKING)
            {
                player.Skills.Fireball.Cast(new());
            }
        }
    }

    public void Start()
    {
    }
}