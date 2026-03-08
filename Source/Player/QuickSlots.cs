using Godot;

public partial class QuickSlots : RefCounted
{
    public QuickSlot SlotOne { get; } = new();
    public QuickSlot SlotTwo { get; } = new();
    public QuickSlot SlotThree { get; } = new();
}