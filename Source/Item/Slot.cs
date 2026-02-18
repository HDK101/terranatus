public class Slot
{
    public ItemBlueprint Blueprint => _item;
    public int Quantity => _quantity;

    private ItemBlueprint _item;
    private int _quantity;

    public void Insert(ItemBlueprint item, int quantity)
    {
        _item = item;
        _quantity = quantity;
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