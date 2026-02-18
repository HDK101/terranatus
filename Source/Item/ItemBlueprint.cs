using Godot;
using System;

[GlobalClass]
public partial class ItemBlueprint : Resource
{
	[Export]
	public string Id { get; set; }

	[Export]
	public ItemTypes Type { get; set; }

	[Export]
	public MeleeWeapon MeleeWeapon { get; set; }

	[Export]
	public Consumable Consumable { get; set; }

	[Export]
	public int MaxQuantity { get; set; } = 1;

	[Export]
	public Texture2D Texture { get; set; }
}
