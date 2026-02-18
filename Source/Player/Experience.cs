using System;
using Godot;

public partial class Experience: RefCounted
{
    [Signal]
    public delegate void LeveledEventHandler();

    public int Level { get; private set; } = 1;
    public int EXP { get; private set; } = 0;

    public void Gain(int amount)
    {
        EXP += amount;
        GD.Print(EXP);

        int requiredExp = GetEXPCeil();

        while (EXP >= requiredExp)
        {
            EXP -= requiredExp;
            Level += 1;
            EmitSignal(SignalName.Leveled);
        }
    }

    public int GetEXPCeil()
    {
        int[] thresholds = [5, 10, 15];
        int extra = 0;

        foreach (int t in thresholds)
            extra += Math.Max(Level - t, 0);

        return (Level + 1 + extra) * 200;
    }
}