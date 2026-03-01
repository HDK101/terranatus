using Godot;
using System;
using System.Collections.Generic;

public partial class Fireball : Area2D
{
	public float Direction { get; set; } = 1.0f;

    private AnimatedSprite2D animatedSprite;
	private readonly float EXPLOSION_RADIUS = 64.0f;
	private RandomNumberGenerator randomNumberGenerator;

	public override void _Ready()
	{
		randomNumberGenerator = new();
		randomNumberGenerator.Randomize();
		animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

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
		QueueFree();
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
