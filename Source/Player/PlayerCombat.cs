using Godot;

public partial class PlayerCombat: Node2D
{
    [Signal]
    public delegate void AttackedEventHandler(AttackType type);

    [Signal]
    public delegate void UsedForwardSlashEventHandler();

    [Signal]
    public delegate void HasCastFireballEventHandler();

    [Signal]
    public delegate void SuccessfulHitEventHandler();


    public PlayerDirection Direction { get; set; }
    public PlayerBody Body { get; set; }
    public Area2D HitArea { get; private set; }
    public Area2D ForwardSlashArea { get; private set; }
    public Area2D ItemArea { get; private set; }
    
    private PackedSceneDB packedSceneDB;

    public override void _Ready()
    {
        HitArea = GetNode<Area2D>("HitArea");
        ForwardSlashArea = GetNode<Area2D>("ForwardSlashArea");
        ItemArea = GetNode<Area2D>("ItemArea");
        packedSceneDB = GetNode<PackedSceneDB>("/root/PackedSceneDB");
    }

    public void CastAttackHit()
    {
        //EntitySoundPlayer.PlayAttackSound(AttackType.SLASH);
        EmitSignal(SignalName.Attacked, (int)AttackType.SLASH);

        var bodies = HitArea.GetOverlappingBodies();

        var weapon = Body.Retrieve(PlayerBody.ItemType.WEAPON);
        double damage = 0;

        if (weapon is not null)
        {
            var payload = weapon.MeleeWeapon.DamageBlueprint.CreatePayload();

            foreach (var damageEntry in payload)
            {
                damage += damageEntry.Value;
            }
        }

        bool successfulHit = false;
        foreach (var body in bodies)
        {
            if (body is IHittable lifeHolder)
            {
                HitPayload hitPayload = new()
                {
                    Damage = damage,
                    Force = new(Direction.ValueDirection * 64.0f, -64.0f),
                    Position = body.Position,
                    Attack = AttackType.SLASH,
                };
                lifeHolder.Hit(hitPayload);
                EmitSignal(SignalName.Attacked, hitPayload);
                successfulHit = true;
            }
        }
        if (successfulHit) EmitSignal(SignalName.SuccessfulHit);
    }

    public void ForwardSlashAttack()
    {
        var bodies = ForwardSlashArea.GetOverlappingBodies();

        bool successfulHit = false;
        foreach (var body in bodies)
        {
            if (body is IHittable lifeHolder)
            {
                HitPayload hitPayload = new()
                {
                    Damage = 10.0,
                    Force = new(Direction.ValueDirection * 128.0f, -128.0f),
                    Position = body.Position,
                    Attack = AttackType.SLASH,
                };
                lifeHolder.Hit(hitPayload);
                EmitSignal(SignalName.Attacked, hitPayload);
                successfulHit = true;
            }
        }
        if (successfulHit) EmitSignal(SignalName.SuccessfulHit);

        var bigSlashInstance = packedSceneDB.BigSlash.Instantiate<SlashEffect>();
        AddChild(bigSlashInstance);
        bigSlashInstance.FlipH = Direction.Flipped();
        bigSlashInstance.Play();

        //EntitySoundPlayer.PlayForwardSlash();
        EmitSignal(SignalName.UsedForwardSlash);
    }

    public void CastFireball()
    {
        var fireballInstance = packedSceneDB.Fireball.Instantiate<Fireball>();
        fireballInstance.Position = GlobalPosition;
        fireballInstance.Direction = Direction.ValueDirection;

        //EntitySoundPlayer.PlayFireRelease();

        GetTree().CurrentScene.CallDeferred("add_child", fireballInstance);
    }

    private double CalculateDamage()
    {
        var weapon = Body.Retrieve(PlayerBody.ItemType.WEAPON);
        double damage = 0;

        if (weapon is not null)
        {
            var payload = weapon.MeleeWeapon.DamageBlueprint.CreatePayload();

            foreach (var damageEntry in payload)
            {
                damage += damageEntry.Value;
            }
        }

        return damage;
    }
}