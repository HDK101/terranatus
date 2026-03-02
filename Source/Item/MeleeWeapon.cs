using Godot;

[GlobalClass]
public partial class MeleeWeapon : Resource
{
    [Export]
    public DamageBlueprint DamageBlueprint { get; set; }
}