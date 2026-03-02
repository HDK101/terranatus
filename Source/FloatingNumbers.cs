using Godot;

public partial class FloatingNumbers : CanvasLayer
{
    private PackedScene floatingNumberPacked;

    public override void _Ready()
    {
        floatingNumberPacked = GD.Load<PackedScene>("res://Scenes/floating_damage.tscn");
    }

    public void CreateDamage(Vector2 position, double value)
    {
        var instance = floatingNumberPacked.Instantiate<FloatingNumber>();
        instance.Position = position + Vector2.Up * 16f;
        instance.Start(value.ToString());
        AddChild(instance);
    }

    public void CreateEXP(Vector2 position, double value)
    {
        var instance = floatingNumberPacked.Instantiate<FloatingNumber>();
        instance.IsEXP = true;
        instance.Position = position + Vector2.Up * 32f;
        instance.Start(value.ToString());
        AddChild(instance);
    }
}