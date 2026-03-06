using Godot;
using System;

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
        Vector2 velocity = player.Velocity;
        Vector2 direction = player.Direction;

        if (!player.IsOnFloor())
        {
            velocity += player.GetGravity() * (float)delta;
        }

        if (Input.IsActionJustPressed("jump") && player.IsOnFloor())
        {
            var jumpVelocity = player.Jump();
            velocity.Y = jumpVelocity;
        }

        if (!player.IsAttacking)
        {
            direction.X = Input.GetAxis("move_left", "move_right");
            player.Direction = direction;
            if (direction != Vector2.Zero)
            {
                velocity.X = player.Direction.X * Player.Speed;
            }
            else
            {
                velocity.X = Mathf.MoveToward(player.Velocity.X, 0, Player.Speed);
            }
        }
        else
        {
            velocity.X = 0.0f;
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
            UpdateView(player);

            Inputs(player);
        }
    }

    private void Inputs(Player player)
    {
        if (Input.IsActionJustPressed("attack"))
        {
            player.Attack();
        }

        if (Input.IsActionJustPressed("skill_one") && player.IsOnFloor() && !player.IsAttacking)
        {
            player.Skills.ForwardSlash.Cast(new());
        }

        if (Input.IsActionJustPressed("skill_two") && !player.IsAttacking)
        {
            player.Skills.Fireball.Cast(new());
        }
    }

    private void UpdateView(Player player)
    {
        var direction = player.Direction;

        if (direction.X < 0.0f)
        {
            player.View.FlipH = true;
        }
        else if (direction.X > 0.0f)
        {
            player.View.FlipH = false;
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

            return;
        }

        if (player.Velocity.Y > 0 && !player.IsAttacking)
        {
            player.View.Fall();
        }
    }

    private static void MovementSetPositions(Player player)
    {
        var direction = player.Direction;
        float lastDirectionHorizontal = player.LastDirectionHorizontal;
        if (direction.X < 0.0f)
        {
            lastDirectionHorizontal = -1.0f;
        }
        else if (direction.X > 0.0f)
        {
            lastDirectionHorizontal = 1.0f;
        }

        player.LastDirectionHorizontal = lastDirectionHorizontal;
        player.HitArea.Position = new(20f * lastDirectionHorizontal, 0.0f);
        player.ForwardSlashArea.Position = new(20f * lastDirectionHorizontal, 0.0f);
    }

    public void Start()
    {
    }
}