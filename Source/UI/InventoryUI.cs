using Godot;
using System.Collections.Generic;

public partial class InventoryUI : Control, MenuElement
{
    #region Enums

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

    #endregion

    #region Category
    private Category CurrentCategory => (Category)categoryIndex;

    private int categoryIndex = 0;
    #endregion

    [ExportCategory("In game")]
    [Export]
    private Player player;

    [ExportCategory("UI")]
    [Export]
    private Label headerLabel;

    [ExportCategory("UI")]
    [Export]
    private TextureRect leftArrow;

    [ExportCategory("UI")]
    [Export]
    private TextureRect rightArrow;

    [ExportCategory("UI")]
    [Export]
    private GridContainer gridContainer;

    [ExportCategory("UI")]
    [Export]
    private TextureRect selectedOverlay;

    #region Nodes

    private List<Slot> slots;
    private MenuAudioPlayer menuAudioPlayer;

    #endregion

    #region State

    private State state = State.HEADER;
    private int selectedSlotIndex = 0;

    #endregion

    #region Packed
    private PackedScene packedItemSlot;
    #endregion


    public override void _Input(InputEvent inputEvent)
    {
        if (!Visible) return;

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
        packedItemSlot = GD.Load<PackedScene>("res://Scenes/UI/item_slot.tscn");
        player.Ready += Initialize;

        menuAudioPlayer = new();
        AddChild(menuAudioPlayer);

        player.Inventory.ConsumablesChange += () =>
        {
            if (CurrentCategory == Category.CONSUMABLES)
            {
                slots = player.Inventory.ListConsumablesPagination();
                ShowItems();

                if (slots.Count == 0)
                {
                    ChangeCategory((int)Category.WEAPONS);
                }
            }
        };

        SetProcess(false);
        SetProcessInput(false);
    }

    private void ShowItems()
    {
        foreach (var slot in gridContainer.GetChildren())
        {
            slot.QueueFree();
        }

        foreach (var slot in slots)
        {
            ItemSlot itemSlot = packedItemSlot.Instantiate<ItemSlot>();
            itemSlot.Update(slot);
            gridContainer.AddChild(itemSlot);
        }
    }

    private void MoveSelectOverlay()
    {
        if (slots.Count == 0 || selectedSlotIndex < 0 || selectedSlotIndex >= Inventory.DEFAULT_PAGE_SIZE) return;

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

    private void ChangeSlots(List<Slot> slotsTarget)
    {
        slots = slotsTarget;
        ShowItems();
    }

    private void HandleHeader(InputEvent inputEvent)
    {
        if (inputEvent.IsActionPressed("ui_up") || inputEvent.IsActionPressed("ui_down"))
        {
            SelectItems();
            menuAudioPlayer.PlayChange();
        }
        else if (inputEvent.IsActionPressed("ui_left"))
        {
            ChangeCategory(categoryIndex - 1);
            menuAudioPlayer.PlayChange();
        }
        else if (inputEvent.IsActionPressed("ui_right"))
        {
            ChangeCategory(categoryIndex + 1);
            menuAudioPlayer.PlayChange();
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

        switch (CurrentCategory)
        {
            case Category.ARMORS:
                ChangeSlots(player.Inventory.ListArmorsPagination());
                break;
            case Category.CONSUMABLES:
                ChangeSlots(player.Inventory.ListConsumablesPagination());
                break;
            case Category.WEAPONS:
                ChangeSlots(player.Inventory.ListWeaponsPagination());
                break;
        }

        headerLabel.Text = GetCategoryText();
    }

    private void HandleItems(InputEvent inputEvent)
    {
        bool movedVertical = false;
        bool movedHorizontal = false;

        if (inputEvent.IsActionPressed("ui_accept") && CurrentCategory == Category.CONSUMABLES)
        {
            player.Inventory.UseConsumable(slots[selectedSlotIndex]);
            menuAudioPlayer.PlayChange();
        }

        if (inputEvent.IsActionPressed("ui_up"))
        {
            selectedSlotIndex -= gridContainer.Columns;
            movedVertical = true;
            menuAudioPlayer.PlayChange();
        }
        else if (inputEvent.IsActionPressed("ui_down"))
        {
            selectedSlotIndex += gridContainer.Columns;
            movedVertical = true;
            menuAudioPlayer.PlayChange();
        }
        else if (inputEvent.IsActionPressed("ui_left"))
        {
            selectedSlotIndex -= 1;
            movedHorizontal = true;
            menuAudioPlayer.PlayChange();
        }
        else if (inputEvent.IsActionPressed("ui_right"))
        {
            selectedSlotIndex += 1;
            movedHorizontal = true;
            menuAudioPlayer.PlayChange();
        }

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
        return CurrentCategory switch
        {
            Category.WEAPONS => "Weapons",
            Category.ARMORS => "Armors",
            Category.CONSUMABLES => "Consumables",
            _ => "",
        };
    }

    public void ShowElement()
    {
        SetProcess(true);
        SetProcessInput(true);

        MenuElementUtils.SlideIn(this);
        Show();
    }

    public void HideElement()
    {
        MenuElementUtils.SlideOut(this).Chain().TweenCallback(Callable.From(Hide));

        SetProcess(false);
        SetProcessInput(false);
    }
}