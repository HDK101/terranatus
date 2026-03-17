using Godot;

[GlobalClass]
public partial class EnemyBlueprint: Resource
{
    [Export]
    private PackedScene packedEnemy;

    public Enemy Create()
    {
        return packedEnemy.Instantiate<Enemy>();
    }
}