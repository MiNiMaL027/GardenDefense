using Enums;
using Comparers;
using Widgets.Buttons;
using Godot;
using Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Widgets.Inventory
{
    public partial class InventoryWidget : Control
    {
        public List<InventorySlot> InventorySlots { get; set; } = new List<InventorySlot>();
        public InventoryComponent InventoryComponent { get; set; }
        public GridContainer gridContainer { get; set; }
        public HBoxContainer TypeContainer { get; set; }
        public HBoxContainer OrderContainer { get; set; }
        public TextureButton CloseButton { get; set; }

        public ItemType? currentType;
        public OrderType currentOrderType = OrderType.New;
        public bool CurrentOrderBittonSide;

        public override void _Ready()
        {
            gridContainer = GetNode<GridContainer>("Panel/VBoxContainer/ScrollContainer/GridContainer");
            CloseButton = GetNode<TextureButton>("Panel/TextureButton");
            TypeContainer = GetNode<HBoxContainer>("Panel/VBoxContainer/Type");
            OrderContainer = GetNode<HBoxContainer>("Panel/VBoxContainer/Order");

            CloseButton.Pressed += CloseButton_Pressed;
        }

        private void CloseButton_Pressed()
        {
            this.GetPlayerController().Hud.GardenWidget.CloseInventory();
        }

        public virtual void SetInventory(InventoryComponent inventoryComponentToSet)
        {
            RemoveSlots();

            OrderButtons();
            TypeButtons();

            InventoryComponent = inventoryComponentToSet;
            InventoryComponent.ItemAdded += ItemAddedListener;
            InventoryComponent.ItemRemoved += ItemRemovedListener;

            Order();
        }
        private void ItemAddedListener(int id, int amount, int indexInArray)
        {
            if (indexInArray < InventorySlots.Count) //if slot exists just update it
            {
                InventorySlot slot = InventorySlots[indexInArray];
                slot.Amount += amount;
            }
            else
            {
                var item = DbService.GetItem(InventoryComponent.InventoryIdArray[indexInArray]);
                InventorySlot slot = Scenes.Widgets.Inventory.InventorySlot();
                InventorySlots.Add(slot);
                gridContainer.AddChild(slot);
                slot.Init(item, amount, this);
            }

            Order();
        }
        private void ItemRemovedListener(int id, int amount, int indexInArray)
        {
            InventorySlot removedSlot = InventorySlots.FirstOrDefault(x => x.ItemId == id);
            removedSlot.Amount -= amount;

            if (removedSlot.Amount <= 0)
            {
                InventorySlots.Remove(removedSlot);
            }
        }

        private void RemoveSlots()
        {
            InventorySlots.Clear();
            Godot.Collections.Array<Node> children = gridContainer.GetChildren();
            foreach(Node child in children)
            {
                child.QueueFree();
            }
        }

        public override void _ExitTree()
        {
            base._ExitTree();
            if (InventoryComponent != null)
            {
                InventoryComponent.ItemAdded -= ItemAddedListener;
                InventoryComponent.ItemRemoved -= ItemRemovedListener;
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
                    if(CurrentOrderBittonSide)
                        SortGridContainer(new PriceComparers());
                    else
                        SortGridContainer(new PriceDescComparers());
                    break;
                case OrderType.Count:
                    InitInventoryItems();
                    if(CurrentOrderBittonSide)
                        SortGridContainer(new AmountComparers());
                    else
                        SortGridContainer(new AmountDecsComparers());
                    break;
            }
        }

        private void SortGridContainer(IComparer<InventorySlot> Comparer)
        {
            InventorySlot[] children = gridContainer.GetChildren().Cast<InventorySlot>().ToArray();

            Array.Sort(children, Comparer);

            for (int i = 0; i < children.Length; i++)
            {
                gridContainer.MoveChild(children[i], i);
            }
        }

        private void TypeButtons()
        {
            for (int i = 0; TypeContainer.GetChildCount() > i; i++)
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
                OrderContainer.GetChild<OrderButton>(i).ButtonClicked += InventoryWidget_ButtonClicked;
            }
        }

        private void InventoryWidget_ButtonClicked(object sender, ButtonEventData e)
        {
            currentOrderType = e.OrderType;
            CurrentOrderBittonSide = e.ForLow;
            Order(currentOrderType);
        }

        private void InitInventoryItems()
        {
            for (int i = 0; i < InventoryComponent.InventoryIdArray.Count; i++)
            {
                var item = DbService.GetItem(InventoryComponent.InventoryIdArray[i]);
                if (currentType == item.ItemType || currentType == ItemType.Misc || currentType == null)
                {
                    InventorySlot slot = Scenes.Widgets.Inventory.InventorySlot();
                    InventorySlots.Add(slot);
                    gridContainer.AddChild(slot);
                    slot.Init(item, InventoryComponent.InventoryAmountArray[i], this);
                }
            }
        }
    }
}
