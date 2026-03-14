using Godot;
using System;

public partial class Life(double initialValue = 1.0) : RefCounted
{
    [Signal]
    public delegate void DeathEventHandler();

    [Signal]
    public delegate void ChangeEventHandler();

    public double Value => _value;
    public double MaxValue => _maxValue;

    public bool IsDead => isDead;

    private double _value = initialValue;
    private double _maxValue = initialValue;

    private bool isDead = false;

    public void Hit(double damage)
    {
        _value = Math.Max(0.0, _value - damage);
        EmitSignal(SignalName.Change);

        if (_value <= 0.0 && !isDead)
        {
            EmitSignal(SignalName.Death);
            isDead = true;
        }
    }
}