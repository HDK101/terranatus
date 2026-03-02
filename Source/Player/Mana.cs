using Godot;
using System;

public partial class Mana(int initialValue = 1) : RefCounted
{
    [Signal]
    public delegate void ChangeEventHandler();

    public int Value => _value;
    public int MaxValue => _maxValue;

    private int _value = initialValue;
    private int _maxValue = initialValue;

    public void Use(int amount)
    {
        if (!Has(amount)) return;

        _value -= amount;
        EmitSignal(SignalName.Change);
    }

    public bool Has(int amount) => Value >= amount;
}