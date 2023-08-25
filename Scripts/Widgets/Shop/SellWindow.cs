using Enums;
using Farm.Scripts.Comparers;
using Farm.Scripts.Enums;
using Farm.Scripts.Widgets.Buttons;
using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public partial class SellWindow : Control
{
	public List<sell_slot> SellSlots { get; set; } = new List<sell_slot>();
	public InventoryComponent InventoryComponent { get; set; }
	public GridContainer InventoryItemContainer { get; set; }
	public HBoxContainer TypeContainer { get; set; }
	public HBoxContainer OrderContainer { get; set; }

	public TextureButton CloseButton { get; set; }

	public ItemType? currentType;
	public OrderType currentOrderType = OrderType.New;
	public override void _Ready()
	{
		TypeContainer = GetNode<HBoxContainer>("Panel/HBoxContainer/VBoxContainer2/Type");
		OrderContainer = GetNode<HBoxContainer>("Panel/HBoxContainer/VBoxContainer2/Order");
		InventoryItemContainer = GetNode<GridContainer>("Panel/HBoxContainer/VBoxContainer2/ScrollContainer/InventoryItems");
		CloseButton = GetNode<TextureButton>("Panel/HBoxContainer/Control/TextureButton");

        CloseButton.Pressed += CloseButton_Pressed;

		Init(this.GetPlayerController().InventoryComponentSeeds);
	}

    private void CloseButton_Pressed()
    {
		this.GetPlayerController().Hud.CloseSellWindow();
    }

    public void Init(InventoryComponent inventoryComponent)
	{ 
		TypeButtons();
		OrderButtons();

		InventoryComponent = inventoryComponent;

		Order(currentOrderType);

	}

	private void RemoveSlots()
	{
		SellSlots.Clear();
        Godot.Collections.Array<Node> children = InventoryItemContainer.GetChildren();
        foreach (Node child in children)
        {
            child.QueueFree();
        }
    }

	public void Order(OrderType OrderType = OrderType.New)
	{
		RemoveSlots();

		switch (OrderType)
		{
			case OrderType.New:
				InitInventoryItems();
				break;

			case OrderType.Price:
				InitInventoryItems();
				SortGridContainer(new PriceComparers());
                break;
			case OrderType.Count:
				InitInventoryItems();
				SortGridContainer(new AmountComparers());
				break;
		}
	}

	private void InitInventoryItems()
	{
        for (int i = 0; i < InventoryComponent.InventoryIdArray.Count; i++)
        {
            var item = DbService.GetItem(InventoryComponent.InventoryIdArray[i]);
            if (currentType == item.ItemType || currentType == ItemType.Misc || currentType == null)
            {
                sell_slot slot = Scenes.Widgets.Shop.SellSlot();
                SellSlots.Add(slot);
                InventoryItemContainer.AddChild(slot);
                slot.Init(item, InventoryComponent.InventoryAmountArray[i], this);
            }
        }
    }

    private void SortGridContainer(IComparer<sell_slot> Comparer)
    {
        sell_slot[] children = InventoryItemContainer.GetChildren().Cast<sell_slot>().ToArray();

        Array.Sort(children, Comparer);

        for (int i = 0; i < children.Length; i++)
        {
            InventoryItemContainer.MoveChild(children[i], i);
        }
    }

	private void TypeButtons()
	{
		for(int i = 0;TypeContainer.GetChildCount() > i; i++)
		{
            TypeContainer.GetChild<CategoriesButton>(i).ButtonClicked += TypeButton_ButtonClicked; ;
		}
	}

    private void TypeButton_ButtonClicked(object sender, ButtonEventData e)
    {
		currentType = e.ItemType;
		Order(currentOrderType);
    }

	private void OrderButtons()
	{
        for (int i = 0; OrderContainer.GetChildCount() > i; i++)
        {
            OrderContainer.GetChild<OrderButton>(i).ButtonClicked += SellWindow_ButtonClicked;
        }
    }

    private void SellWindow_ButtonClicked(object sender, ButtonEventData e)
    {
        currentOrderType = e.OrderType;
		Order(e.OrderType);
    }
}
