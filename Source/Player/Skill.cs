using Godot;

public abstract partial class Skill : RefCounted
{
    [Signal]
    public delegate void LevelUpEventHandler(int level);

    public Texture2D MenuTexture { get; protected set; }
    public Texture2D QuickSlotTexture { get; protected set; }

    public int Level { get; set; } = 0;

    public void IncreaseLevel()
    {
        Level += 1;
        EmitSignal(SignalName.LevelUp, Level);
    }
}