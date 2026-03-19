using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

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

    private PackedSceneDB packedSceneDB;

    private Texture2D appleTexture;

    private SoundDB soundDB;

    private RandomNumberGenerator rng = new();
    private AudioStreamPlayer audioStreamPlayer;

    private GameStats gameStats;

    private readonly List<WarpGate> warpGates = [];

    public override void _Ready()
    {
        rng.Randomize();

        gameStats = GetNode<GameStats>("/root/GameStats");

        audioStreamPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
        audioStreamPlayer.Stream = new AudioStreamPolyphonic();
        audioStreamPlayer.Play();

        soundDB = GetNode<SoundDB>("/root/SoundDB");
        packedSceneDB = GetNode<PackedSceneDB>("/root/PackedSceneDB");
        appleTexture = GD.Load<Texture2D>("res://Sprites/Items/item216.png");
        gameHud = GetNode<GameHud>("GameHUD");
        InitializeWarpGates();
        InitializePlayer();

        gameHud.TransitionRect.FadeOut();

        player.Death += OnPlayerDeath; 
    }

    public void Delay()
    {
        GetTree().Paused = true;
        GetTree().CreateTimer(0.1).Timeout += () => GetTree().Paused = false;
    }

    public void OnPlayerDeath()
    {
        GetTree().CreateTimer(3.5).Timeout += gameHud.TransitionRect.FadeIn;

        GetTree().CreateTimer(5.5).Timeout += () =>
        {
            GetTree().ReloadCurrentScene();
        };
    }

    public void OnEnemyCreate(Enemy enemy)
    {
        enemy.Damaged += payload => OnEnemyHit(enemy, payload);
        enemy.Player = player;
        enemy.Life.Death += () => CallDeferred(nameof(OnEnemyDeath), enemy);
        CallDeferred("add_child", enemy);
    }

    private void InitializeWarpGates()
    {
        List<WarpGate> nodes = [.. GetTree().GetNodesInGroup("WARP").OfType<WarpGate>()];

        foreach (WarpGate warpGate in nodes)
        {
            warpGate.Player = player;
        }

        warpGates.AddRange(nodes);
    }

    private void CreateCorpse(Enemy enemy)
    {
        var corpseInstance = packedSceneDB.FlyingCorpse.Instantiate<FlyingCorpse>();
        corpseInstance.Texture = enemy.CurrentTexture;
        corpseInstance.Position = enemy.Position;
        corpseInstance.RotationForceInRadians = Random.Shared.NextDouble() * Math.PI;
        double randomDir = -1.0 + Random.Shared.NextDouble() * 2.0;
        corpseInstance.Velocity = new(64f * (float)randomDir, -64f);
        targetScene.AddChild(corpseInstance);
    }

    private void InitializePlayer()
    {
        WarpGate lastWarpGate = warpGates.Find(warpGate => warpGate.WarpLocation.IsSameWarp(gameStats.LastWarp));

        player.Respawn(lastWarpGate.Position - Vector2.One * 8.0f);

        player.Combat.Attacked += OnPlayerAttack;
        player.Damaged += payload => CallDeferred(nameof(OnPlayerHit), payload);
        player.ItemPicked += OnPickItem;
        player.Experience.Leveled += gameHud.LevelUpNotification.Notificate;
        player.SuccessfulHit += Delay;
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
            var punchInstance = packedSceneDB.Punch.Instantiate<HitEffect>();
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
        CreateGlowExplosion(enemy);

        var itemTuples = enemy.ToDropItems();
        foreach (var (blueprint, quantity) in itemTuples)
        {
            var itemInstance = packedSceneDB.DroppableItem.Instantiate<DroppableItem>();
            itemInstance.PreStart(enemy.Position, blueprint, quantity);
            targetScene.AddChild(itemInstance);
            itemInstance.Picked += () => PlaySound(soundDB.PickupRandomizer);
        }

        enemy.QueueFree();
    }

    private void CreateGlowExplosion(Enemy enemy)
    {
        GlowExplosion glowExplosion = packedSceneDB.GlowingParticlesExplosion.Instantiate<GlowExplosion>();
        glowExplosion.Position = enemy.Position;
        AddChild(glowExplosion);
    }

    private void CreateEXP(Enemy enemy)
    {
        int totalReward = enemy.EXPReward;

        while (totalReward > 0)
        {
            int currentReward = totalReward < 10 ? totalReward : 10;
            totalReward -= currentReward;
            EXPParticle expParticle = packedSceneDB.EXP.Instantiate<EXPParticle>();
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
            var slashInstance = packedSceneDB.Slash.Instantiate<HitEffect>();
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