using Godot;
using System;

public partial class ItemSlot : TextureRect
{
	private Label quantityLabel;
	private string quantityText;

    public override void _Ready()
	{
		quantityLabel = GetNode<Label>("Label");
		quantityLabel.Text = quantityText;
	}

	public void Update(Slot slot)
	{
		Texture = slot.Blueprint.Texture;
		quantityText = slot.Quantity.ToString();
	}
}
