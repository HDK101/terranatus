using Godot;
using Godot.Collections;
using System.Collections.Generic;
using System.Linq;

[GlobalClass]
public partial class MonsterSpawner : Node2D
{
	[Signal]
	public delegate void SpawnedEventHandler(Enemy enemy);

	public List<Enemy> Enemies { get; } = [];

	[Export]
	public Array<EnemyBlueprint> Blueprints { get; set; }

	[Export]
	public int MaxMonsters { get; set; } = 1;

	[Export]
	public Vector2 Size;

	[Export(PropertyHint.Range, "0.1, 20.0, 0.1")]
	private float interval = 1.0f;

	private RandomNumberGenerator randomNumberGenerator = new();

	private Timer timer;

	public override void _Ready()
	{
		timer = new()
		{
			Autostart = true,
			WaitTime = interval,
		};
		timer.Timeout += SpawnEnemy;
		AddChild(timer);

		if (Engine.IsEditorHint()) return;

		randomNumberGenerator.Randomize();

		for (int i = 0; i < MaxMonsters; i++)
		{
			SpawnEnemy();
		}
	}

	private void OnEnemyDeath()
	{
		var aliveEnemies = Enemies.Where(enemy => enemy is null);
		Enemies.Clear();
		Enemies.AddRange(aliveEnemies);
	}

	private void SpawnEnemy()
	{
		GD.Print(Enemies.Count);
		GD.Print(Enemies.Count < MaxMonsters);
		if (Enemies.Count >= MaxMonsters) return;

		float x = randomNumberGenerator.RandfRange(0.0f, Size.X);
		float y = randomNumberGenerator.RandfRange(0.0f, Size.Y);

		Enemy enemy = Blueprints.PickRandom().Create();
		Enemies.Add(enemy);
		enemy.Position = Position + new Vector2(x, y) - Size / 2.0f;
		enemy.Life.Death += OnEnemyDeath;
		EmitSignal(SignalName.Spawned, enemy);
	}
}
