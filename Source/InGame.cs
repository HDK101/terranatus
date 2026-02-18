using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;

public partial class InGame : Node2D
{
	[Export]
	private Player player;

	[Export]
	private FloatingNumbers floatingNumbers;

	[Export]
	private Array<Enemy> enemies;

	[Export]
	private GameHud gameHud;

	private PackedScene packedFlyingCorpse;
	private PackedScene packedDroppableItem;
	private PackedScene packedSlash;
	private PackedScene packedPunch;

	private Texture2D appleTexture;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		packedFlyingCorpse = GD.Load<PackedScene>("res://Scenes/flying_corpse.tscn");
		packedDroppableItem = GD.Load<PackedScene>("res://Scenes/droppable_item.tscn");
		packedSlash = GD.Load<PackedScene>("res://Scenes/slash.tscn");
		packedPunch = GD.Load<PackedScene>("res://Scenes/punch.tscn");
		appleTexture = GD.Load<Texture2D>("res://Sprites/Items/item216.png");
		gameHud = GetNode<GameHud>("GameHUD");
		InitializeEnemies();
		InitializePlayer();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void CreateCorpse(Texture2D texture)
	{

	}

	private void InitializePlayer()
	{
		player.Attacked += OnPlayerAttack;
		player.Damaged += payload => CallDeferred(nameof(OnPlayerHit), payload);
		player.ItemPicked += gameHud.ItemNotification.ShowItem;
	}

	private void InitializeEnemies()
	{
		foreach (var enemy in enemies)
		{
			enemy.Damaged += payload => OnEnemyHit(enemy, payload);
			enemy.Player = player;
			enemy.Life.Death += () => CallDeferred(nameof(OnEnemyDeath), enemy);
		}
	}

	private void OnPlayerHit(HitPayload hitPayload)
	{
		GD.Print("On player hit");
		if (hitPayload.Attack == HitPayload.AttackType.PUNCH)
		{
			var punchInstance = packedPunch.Instantiate<HitEffect>();
			punchInstance.Position = hitPayload.Position;
			AddChild(punchInstance);
			GD.Print("On player punched");
		}
	}

	private void OnEnemyHit(Enemy enemy, HitPayload hitPayload)
	{
		gameHud.Combo.Add();
	}

	private void OnEnemyDeath(Enemy enemy)
	{
		var corpseInstance = packedFlyingCorpse.Instantiate<FlyingCorpse>();
		corpseInstance.Texture = enemy.CurrentTexture;
		corpseInstance.Position = enemy.Position;
		corpseInstance.RotationForceInRadians = Random.Shared.NextDouble() * Math.PI;
		double randomDir = -1.0 + Random.Shared.NextDouble() * 2.0;
		corpseInstance.Velocity = new(64f * (float)randomDir, -64f);
		AddChild(corpseInstance);

		var items = enemy.ToDropItems();
		foreach (var item in items)
		{
			var itemInstance = packedDroppableItem.Instantiate<DroppableItem>();
			itemInstance.PreStart(enemy.Position, item, 1);
			AddChild(itemInstance);
		}
		floatingNumbers.CreateEXP(enemy.Position, enemy.EXPReward);
		player.Experience.Gain(enemy.EXPReward);
	}

	private void OnPlayerAttack(HitPayload payload)
	{
		if (payload.Attack == HitPayload.AttackType.SLASH)
		{
			floatingNumbers.CreateDamage(payload.Position, payload.Damage);
			var slashInstance = packedSlash.Instantiate<HitEffect>();
			slashInstance.Position = payload.Position;
			CallDeferred("add_child", slashInstance);
		}
	}
}
