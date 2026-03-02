using Godot;
using Godot.Collections;
using System;

public partial class InGame : Node2D
{
    [Export]
    private Node2D targetScene;

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
    private PackedScene packedEXP;

    private Texture2D appleTexture;

    private SoundDB soundDB;

    private RandomNumberGenerator rng = new();
    private AudioStreamPlayer audioStreamPlayer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        rng.Randomize();

        audioStreamPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
        audioStreamPlayer.Stream = new AudioStreamPolyphonic();
        audioStreamPlayer.Play();

        soundDB = GetNode<SoundDB>("/root/SoundDB");
        packedFlyingCorpse = GD.Load<PackedScene>("res://Scenes/flying_corpse.tscn");
        packedDroppableItem = GD.Load<PackedScene>("res://Scenes/droppable_item.tscn");
        packedSlash = GD.Load<PackedScene>("res://Scenes/slash.tscn");
        packedPunch = GD.Load<PackedScene>("res://Scenes/punch.tscn");
        packedEXP = GD.Load<PackedScene>("res://Scenes/exp_particle.tscn");
        appleTexture = GD.Load<Texture2D>("res://Sprites/Items/item216.png");
        gameHud = GetNode<GameHud>("GameHUD");
        InitializeEnemies();
        InitializePlayer();
    }

    public void Delay()
    {
        GetTree().Paused = true;
        GetTree().CreateTimer(0.1).Timeout += () => GetTree().Paused = false;
    }

    private void CreateCorpse(Enemy enemy)
    {
        var corpseInstance = packedFlyingCorpse.Instantiate<FlyingCorpse>();
        corpseInstance.Texture = enemy.CurrentTexture;
        corpseInstance.Position = enemy.Position;
        corpseInstance.RotationForceInRadians = Random.Shared.NextDouble() * Math.PI;
        double randomDir = -1.0 + Random.Shared.NextDouble() * 2.0;
        corpseInstance.Velocity = new(64f * (float)randomDir, -64f);
        targetScene.AddChild(corpseInstance);
    }

    private void InitializePlayer()
    {
        player.Attacked += OnPlayerAttack;
        player.Damaged += payload => CallDeferred(nameof(OnPlayerHit), payload);
        player.ItemPicked += OnPickItem;
        player.Experience.Leveled += gameHud.LevelUpNotification.Notificate;
        player.SuccessfulHit += Delay;
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

    private void OnPickItem(ItemBlueprint itemBlueprint, int quantity)
    {
        gameHud.ItemNotification.ShowItem(itemBlueprint, quantity);
        PlaySound(soundDB.PickupRandomizer);
    }

    private void OnPlayerHit(HitPayload hitPayload)
    {
        if (hitPayload.Attack == AttackType.PUNCH)
        {
            var punchInstance = packedPunch.Instantiate<HitEffect>();
            punchInstance.Position = hitPayload.Position;
            targetScene.AddChild(punchInstance);
        }
    }

    private void OnEnemyHit(Enemy enemy, HitPayload hitPayload)
    {
        gameHud.Combo.Add();
    }

    private void OnEnemyDeath(Enemy enemy)
    {
        CreateCorpse(enemy);
        PlaySound2D(soundDB.EnemyDeathRandomizer, enemy.Position);

        CreateEXP(enemy);

        var items = enemy.ToDropItems();
        foreach (var item in items)
        {
            var itemInstance = packedDroppableItem.Instantiate<DroppableItem>();
            itemInstance.PreStart(enemy.Position, item, 1);
            targetScene.AddChild(itemInstance);
            itemInstance.Picked += () => PlaySound(soundDB.PickupRandomizer);
        }

        enemy.QueueFree();
    }

    private void CreateEXP(Enemy enemy)
    {
        int totalReward = enemy.EXPReward;

        while (totalReward > 0)
        {
            int currentReward = totalReward < 10 ? totalReward : 10;
            totalReward -= currentReward;
            EXPParticle expParticle = packedEXP.Instantiate<EXPParticle>();
            expParticle.Position = enemy.Position;
            expParticle.Player = player;
            expParticle.EXP = currentReward;
            expParticle.RNG = rng;
            targetScene.AddChild(expParticle);
        }

        floatingNumbers.CreateEXP(enemy.Position, enemy.EXPReward);
    }

    private void OnPlayerAttack(HitPayload payload)
    {
        if (payload.Attack == AttackType.SLASH)
        {
            floatingNumbers.CreateDamage(payload.Position, payload.Damage);
            var slashInstance = packedSlash.Instantiate<HitEffect>();
            slashInstance.Position = payload.Position;
            targetScene.AddChild(slashInstance);
        }
    }

    private void PlaySound2D(AudioStream stream, Vector2 position)
    {
        var audioStreamPlayer = new AudioStreamPlayer2D()
        {
            Stream = stream,
            GlobalPosition = position,
        };
        audioStreamPlayer.Finished += audioStreamPlayer.QueueFree;
        targetScene.AddChild(audioStreamPlayer);
        audioStreamPlayer.Play();
    }

    private void PlaySound(AudioStream stream)
    {
        if (audioStreamPlayer.GetStreamPlayback() is AudioStreamPlaybackPolyphonic polyphonic)
        {
            polyphonic.PlayStream(stream);
        }
    }
}