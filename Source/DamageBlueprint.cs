using Godot;
using Godot.Collections;

[GlobalClass]
public partial class DamageBlueprint: Resource
{
    [Export]
    public Dictionary<DamageType, DamageRange> Damages { get; set; }
}