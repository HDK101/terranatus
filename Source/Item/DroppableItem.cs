using System;
using Godot;

public partial class DroppableItem: RigidBody2D
{
    public class Item
    {
        public int Quantity { get; set; }
        public ItemBlueprint Blueprint { get; set; }
    }

    [Signal]
    public delegate void PickedEventHandler();

    public Item CurrentItem => _item;

    private Sprite2D sprite;

    private Texture2D texture;
    private float torqueForce = 0.0f;
    private Vector2 impulse;

    private Item _item;

    public override void _Ready()
    {
        sprite = GetNode<Sprite2D>("Sprite2D");
        Start();
    }

    public void PreStart(Vector2 position, ItemBlueprint item, int quantity)
    {
        _item = new()
        {
            Blueprint = item,
            Quantity = quantity,
        };
        Position = position;
        texture = item.Texture;
        float randomDir = (float)(-1.0 + Random.Shared.NextDouble() * 2.0);
        impulse = new (randomDir * 64.0f, -64.0f);
        torqueForce = (float)Random.Shared.NextDouble() * 32.0f;
    }

    public void Pick()
    {
        EmitSignal(SignalName.Picked);
    }

    public void Start()
    {
        ApplyTorque(torqueForce);
        ApplyCentralImpulse(impulse);
        sprite.Texture = texture;
    }
}