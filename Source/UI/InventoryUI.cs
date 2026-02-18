using System.Collections.Generic;
using Godot;

public partial class InventoryUI : Control
{
	public bool Active = false;

	private enum State
	{
		HEADER,
		ITEMS,
	}

	private enum Category
	{
		WEAPONS = 0,
		CONSUMABLES = 1,
		ARMORS = 2,
	}

	private int GridColumns => gridContainer.Columns;

	private Category CurrentCategory => (Category)categoryIndex;

	private int categoryIndex = 0;

	[Export]
	private Player player;

	[Export]
	private Label headerLabel;

	[Export]
	private TextureRect leftArrow;

	[Export]
	private TextureRect rightArrow;

	[Export]
	private GridContainer gridContainer;

	[Export]
	private TextureRect selectedOverlay;

	private List<Slot> slots;
	private State state = State.HEADER;
	private int selectedSlotIndex = 0;

	public override void _Input(InputEvent inputEvent)
	{
		if (!Active) return;

		switch (state)
		{
			case State.HEADER:
				HandleHeader(inputEvent);
				break;
			case State.ITEMS:
				HandleItems(inputEvent);
				break;
		}
	}

	public override async void _Ready()
	{
		player.Ready += Initialize;
	}

	private void ShowItems()
	{
		foreach (var slot in gridContainer.GetChildren())
		{
			slot.QueueFree();
		}

		foreach (var slot in slots)
		{
			TextureRect textureRect = new()
			{
				Texture = slot.Blueprint.Texture
			};
			gridContainer.AddChild(textureRect);
		}
	}

	private void MoveSelectOverlay()
	{
		if (selectedSlotIndex < 0 || selectedSlotIndex >= Inventory.DEFAULT_PAGE_SIZE) return;

		var child = gridContainer.GetChild(selectedSlotIndex);

		if (child is Control control)
		{
			selectedOverlay.GlobalPosition = control.GlobalPosition;
		}
	}

	private void SelectHeader()
	{
		state = State.HEADER;
		leftArrow.Show();
		rightArrow.Show();
		selectedOverlay.Hide();
	}

	private void SelectItems()
	{
		state = State.ITEMS;
		selectedSlotIndex = 0;
		leftArrow.Hide();
		rightArrow.Hide();
		selectedOverlay.Show();
	}

	private void Initialize()
	{
		slots = player.Inventory.ListWeaponsPagination();
		ShowItems();
	}

	private void HandleHeader(InputEvent inputEvent)
	{
		if (inputEvent.IsActionPressed("ui_up") || inputEvent.IsActionPressed("ui_down"))
		{
			SelectItems();
		}
		else if (inputEvent.IsActionPressed("ui_left"))
		{
			ChangeCategory(categoryIndex - 1);
		}
		else if (inputEvent.IsActionPressed("ui_right"))
		{
			ChangeCategory(categoryIndex + 1);
		}
	}

	private void ChangeCategory(int index)
	{
		categoryIndex = index;
		if (index < 0)
		{
			categoryIndex = (int)Category.ARMORS;
		}
		else if (categoryIndex > (int)Category.ARMORS)
		{
			categoryIndex = (int)Category.WEAPONS;
		}

		headerLabel.Text = GetCategoryText();
	}

	private void HandleItems(InputEvent inputEvent)
	{
		bool movedVertical = false;
		bool movedHorizontal = false;

		if (inputEvent.IsActionPressed("ui_up"))
		{
			selectedSlotIndex -= GridColumns;
			movedVertical = true;
		}
		else if (inputEvent.IsActionPressed("ui_down"))
		{
			selectedSlotIndex += GridColumns;
			movedVertical = true;
		}
		else if (inputEvent.IsActionPressed("ui_left"))
		{
			selectedSlotIndex -= 1;
			movedHorizontal = true;
		}
		else if (inputEvent.IsActionPressed("ui_right"))
		{
			selectedSlotIndex += 1;
			movedHorizontal = true;
		}

		GD.Print(selectedSlotIndex);

		if (movedHorizontal)
		{
			if (selectedSlotIndex < 0)
			{
				selectedSlotIndex = slots.Count - 1;
			}
			else if (selectedSlotIndex >= slots.Count)
			{
				selectedSlotIndex = 0;
			}
		}

		if (movedVertical && (selectedSlotIndex <= 0 || selectedSlotIndex >= slots.Count))
		{
			selectedSlotIndex = 0;
			SelectHeader();
			return;
		}
		MoveSelectOverlay();
	}

	private string GetCategoryText()
	{
		switch (CurrentCategory)
		{
			case Category.WEAPONS:
				return "Weapons";
			case Category.ARMORS:
				return "Armors";
			case Category.CONSUMABLES:
				return "Consumables";
		}
		return "";
	}
}
