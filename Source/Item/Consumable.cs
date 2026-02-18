using Godot;

[GlobalClass]
public partial class Consumable: Resource
{
    [Export]
    public double HealthRegen { get; set; }

    [Export]
    public double ManaRegen { get; set; }
}