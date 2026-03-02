using Godot;
using System;
using System.Collections.Generic;

public partial class Fireball : Area2D
{
    public float Direction { get; set; } = 1.0f;

    private AnimatedSprite2D animatedSprite;
    private readonly float EXPLOSION_RADIUS = 64.0f;
    private RandomNumberGenerator randomNumberGenerator;

    private GpuParticles2D fireParticles;
    private GpuParticles2D smokeParticles;

    private PackedScene explosionPacked;

    public override void _Ready()
    {
        var packedSceneDb = GetNode<PackedSceneDB>("/root/PackedSceneDB");
        explosionPacked = packedSceneDb.FireExplosion;
        randomNumberGenerator = new();
        randomNumberGenerator.Randomize();
        animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        fireParticles = GetNode<GpuParticles2D>("FireballParticles");
        smokeParticles = GetNode<GpuParticles2D>("SmokeParticles");

        BodyEntered += OnBodyEnter;
    }

    public override void _Process(double delta)
    {
        animatedSprite.Rotate((float)delta * 32.0f);
    }

    public override void _PhysicsProcess(double delta)
    {
        Translate(Vector2.Right * Direction * 256.0f * (float)delta);
    }

    private void OnBodyEnter(Node2D body)
    {
        Dictionary<DamageType, DamageRangeRef> damages = [];
        damages[DamageType.FIRE] = new DamageRangeRef(0, 10);

        Explode(new()
        {
            Damages = damages,
            Radius = EXPLOSION_RADIUS,
            Position = Position,
        });

        var instance = explosionPacked.Instantiate<FireballExplosion>();

        RemoveChild(fireParticles);
        RemoveChild(smokeParticles);

        CallDeferred(nameof(DetachParticles));
        CallDeferred(nameof(CreateExplosion));

        QueueFree();
    }

    private void DetachParticles()
    {
        fireParticles.Emitting = false;
        smokeParticles.Emitting = false;

        GetTree().CurrentScene.AddChild(fireParticles);
        GetTree().CurrentScene.AddChild(smokeParticles);

        fireParticles.Position = Position;
        smokeParticles.Position = Position;
    }

    private void CreateExplosion()
    {
        var instance = explosionPacked.Instantiate<FireballExplosion>();
        GetTree().CurrentScene.AddChild(instance);
        instance.Position = Position;
    }

    private void Explode(PlayerExplosionPayload payload)
    {
        var nodes = GetTree().GetNodesInGroup("ENEMY");

        List<Enemy> enemies = [];

        foreach (var node in nodes)
        {
            if (node is Enemy enemy)
            {
                if (enemy.Position.DistanceTo(Position) <= payload.Radius)
                    enemies.Add(enemy);
            }
        }

        foreach (var enemy in enemies)
        {
            int totalDamage = 0;

            foreach (var damage in payload.Damages)
            {
                totalDamage += damage.Value.Rand(randomNumberGenerator);
            }

            Vector2 force = enemy.Position.DirectionTo(Position) * 24.0f;
            enemy.Hit(new()
            {
                Force = force,
                Damage = totalDamage,
            });
        }
    }
}