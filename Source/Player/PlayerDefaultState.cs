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
            Move(delta, player);
        }
    }

    private static void Move(double delta, Player player)
    {
        if (player.IsAttacking) return;

        Vector2 velocity = player.Velocity;

        if (!player.IsOnFloor())
        {
            velocity += player.GetGravity() * (float)delta;
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
        return;
    }

    public void Process(double delta)
    {
        if (playerRef.TryGetTarget(out var player))
        {
            MovementSetPositions(player);

            if (Input.IsActionJustPressed("attack"))
            {
                player.View.Attack();
            }

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

    private static void MovementSetPositions(Player player)
    {
        var direction = player.Direction;
        if (direction.X < 0.0f)
        {
            player.View.FlipH = true;
            player.HitArea.Position = new(-16f, 0.0f);
            player.ForwardSlashArea.Position = new(-20f, 0.0f);
            player.LastDirectionHorizontal = -1.0f;
        }
        else if (direction.X > 0.0f)
        {
            player.View.FlipH = false;
            player.HitArea.Position = new(16f, 0.0f);
            player.ForwardSlashArea.Position = new(20f, 0.0f);
            player.LastDirectionHorizontal = 1.0f;
        }

        if (player.IsOnFloor())
        {
            if (direction.X == 0.0f)
            {
                player.View.Idle();
            }
            else
            {
                player.View.Walk();
            }
        }
    }

    public void Start()
    {
    }
}