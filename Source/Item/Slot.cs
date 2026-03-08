using Godot;

public partial class Slot : RefCounted
{
    [Signal]
    public delegate void DepletedEventHandler();
    [Signal]
    public delegate void QuantityChangeEventHandler();

    public ItemBlueprint Blueprint => _item;
    public int Quantity => _quantity;

    private ItemBlueprint _item;
    private int _quantity;

    public void Insert(ItemBlueprint item, int quantity)
    {
        _item = item;
        _quantity = quantity;
    }

    public void Increase(int amount = 1)
    {
        _quantity += amount;
        EmitSignal(SignalName.QuantityChange);
    }

    public void Decrease(int amount = 1)
    {
        if (_quantity - amount < 0) return;

        _quantity -= amount;

        if (_quantity == 0) EmitSignal(SignalName.Depleted);
        EmitSignal(SignalName.QuantityChange);
    }

    public bool IsEmpty()
    {
        return _item is null;
    }

    public void Pop()
    {
        _item = null;
        _quantity = 0;
    }
}