using Godot;
using System;
using System.Collections.Generic;

public partial class Slime : Enemy
{
    public const float Speed = 300.0f;
    public const float JumpVelocity = -400.0f;
    public override Texture2D CurrentTexture
    {
        get
        {
            var frames = animatedSprite.SpriteFrames;
            string animation = animatedSprite.Animation;
            int frame = animatedSprite.Frame;

            return frames.GetFrameTexture(animation, frame);
        }
    }

    private enum State
    {
        IDLE,
        JUMP,
        ATTACK,
    }

    private State state = State.IDLE;

    private BodyForce bodyForce;

    private Vector2 velocity = new();

    private bool stunned = false;

    private AnimatedSprite2D animatedSprite;
    private AnimationPlayer animationPlayer;

    private float lastDirection = 1.0f;
    private Timer attackTimer;
    private Timer moveTimer;

    private RandomNumberGenerator randomNumberGenerator;

    private ItemBlueprint apple;

    public override void PostHit(HitPayload payload)
    {
        bodyForce.Force = payload.Force;
        stunned = true;
        GetTree().CreateTimer(0.2).Timeout += () =>
        {
            stunned = false;
        };

        EntitySoundPlayer.PlayHurt();

        var tween = CreateDefaultTween();
        Scale = new(0.9f, 0.7f);
        tween.TweenProperty(this, "scale", Vector2.One, 0.5f);
    }

    public override void Start()
    {
        ItemDB itemDB = GetNode<ItemDB>("/root/ItemDB");
        apple = itemDB.Retrieve("APPLE");

        randomNumberGenerator = new();
        randomNumberGenerator.Randomize();

        attackTimer = GetNode<Timer>("AttackTimer");
        moveTimer = GetNode<Timer>("MoveTimer");
        animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        bodyForce = new();
        AddChild(bodyForce);

        attackTimer.Timeout += Attack;
        moveTimer.Timeout += RandomlyJump;
        moveTimer.Start(3.0);

        EXPReward = 870;

        lifeBar.Position = new(0f, -8f);
    }

    public override List<ItemBlueprint> ToDropItems()
    {
        return [apple];
    }

    public override void _Process(double delta)
    {
        if (IsPlayerNear(32f) && attackTimer.IsStopped())
        {
            state = State.ATTACK;
            attackTimer.Start(2.0);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        Velocity = velocity + bodyForce.Force;
        MoveAndSlide();

        // Add the gravity.
        if (!IsOnFloor())
        {
            velocity += GetGravity() * (float)delta;
        }
        else
        {
            if (!stunned)
            {
                bodyForce.Reset();
            }
            velocity = Vector2.Zero;
        }
    }

    private void RandomlyJump()
    {
        if (state == State.ATTACK) return;

        var tween = CreateDefaultTween();
        Scale = new(0.8f, 1.2f);
        tween.TweenProperty(this, "scale", Vector2.One, 0.5f);

        float selectedDirection = randomNumberGenerator.RandfRange(-128.0f, 128.0f);

        animatedSprite.FlipH = selectedDirection > 0;

        velocity.X = selectedDirection;
        velocity.Y = -128.0f;
    }

    private void Attack()
    {
        bool isRight = Position.X < Player.Position.X;

        animatedSprite.FlipH = isRight;
        if (isRight) animationPlayer.Play("attack_right");
        else animationPlayer.Play("attack_left");
        if (IsPlayerNear(32f))
        {
            Player.Hit(new HitPayload()
            {
                Damage = 1,
                Attack = AttackType.PUNCH,
                Position = Player.Position,
                Force = new(isRight ? 8f : -8f, 0.0f),
            });
            EntitySoundPlayer.PlayAttackSound(AttackType.PUNCH);
        }
        state = State.IDLE;
    }
}