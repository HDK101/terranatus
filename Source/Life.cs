using Godot;
using System;

public partial class Life(int initialValue = 1) : RefCounted
{
    [Signal]
    public delegate void DeathEventHandler();

    [Signal]
    public delegate void ChangeEventHandler();

    public int Value => _value;
    public int MaxValue => _maxValue;

    public bool IsDead => isDead;

    private int _value = initialValue;
    private int _maxValue = initialValue;

    private bool isDead = false;

    public void Hit(double damage)
    {
        _value = (int)Math.Max(0, _value - damage);
        EmitSignal(SignalName.Change);

        if (_value <= 0.0 && !isDead)
        {
            EmitSignal(SignalName.Death);
            isDead = true;
        }
    }
}