using Godot;
using Godot.Collections;

[GlobalClass]
public partial class DamageBlueprint: Resource
{
    [Export]
    public Dictionary<DamageType, DamageRange> Damages { get; set; }

    public Dictionary<DamageType, double> CreatePayload()
    {
        RandomNumberGenerator randomNumberGenerator = new();
        randomNumberGenerator.Randomize();

        Dictionary<DamageType, double> payload = [];

        foreach (var damageEntry in Damages)
        {
            payload[damageEntry.Key] = damageEntry.Value.Rand(randomNumberGenerator);
        }

        return payload;
    }
}