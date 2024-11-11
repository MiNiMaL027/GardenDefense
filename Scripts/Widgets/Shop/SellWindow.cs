using Enums;
using Widgets.Buttons;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Widgets.Shop
{
    public partial class SellWindow : Control
    {
        public List<sell_slot> SellSlots { get; set; } = new List<sell_slot>();
        public InventoryComponent InventoryComponent { get; set; }
        public GridContainer InventoryItemContainer { get; set; }
        public GridContainer SellItemContainer { get; set; }
        public HBoxContainer TypeContainer { get; set; }
        public HBoxContainer OrderContainer { get; set; }

        private Label CoinsLabel { get; set; }

        public TextureButton CloseButton { get; set; }

        public Button SellButton { get; set; }

        public ItemType? currentType;
        public OrderType currentOrderType = OrderType.Default;
        public bool CurrentButtonOrderCountSide = false;
        public bool CurrentButtonOrderPriceSide = false;
        public override void _Ready()
        {
            TypeContainer = GetNode<HBoxContainer>("Panel/HBoxContainer/VBoxContainer2/Type");
            OrderContainer = GetNode<HBoxContainer>("Panel/HBoxContainer/VBoxContainer2/Order");
            InventoryItemContainer = GetNode<GridContainer>("Panel/HBoxContainer/VBoxContainer2/ScrollContainer/InventoryItems");
            CloseButton = GetNode<TextureButton>("Panel/HBoxContainer/Control/TextureButton");
            SellItemContainer = GetNode<GridContainer>("Panel/HBoxContainer/VBoxContainer/GridContainer");
            CoinsLabel = GetNode<Label>("Panel/HBoxContainer/Panel/PanelContainer/HBoxContainer/Coins");
            SellButton = GetNode<Button>("Panel/HBoxContainer/Panel/Button");

            CloseButton.Pressed += CloseButton_Pressed;
            InventoryItemContainer.Resized += ChangeColumnsNumber;

            Init(this.GetPlayerController().GardenInventory);

            this.GetPlayerController().GoldChange += RefreshCoinsCount;

            RefreshCoinsCount(this.GetPlayerController().Gold);

            SellButton.Pressed += SellButton_Pressed;
        }

        private void SellButton_Pressed()
        {
            var children = SellItemContainer.GetChildren();
            foreach (var child in children)
            {
                if(child is sell_slot slot)
                {
                    var playerController = this.GetPlayerController();
                    playerController.Gold += slot.ItemDatabaseRow.SellPrice * slot.Amount;

                    InventoryComponent.RemoveItem(slot.ItemDatabaseRow.Id, slot.Amount);

                    SellItemContainer.RemoveChild(slot);

                    slot.QueueFree();
                }
            }
        }

        public override void _ExitTree()
        {
            base._ExitTree();

            this.GetPlayerController().GoldChange -= RefreshCoinsCount;
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

            ChangeColumnsNumber();

            if (Options.safeSelling)
            {
                SellButton.Visible = true;
                SellButton.Disabled = false;
            }
            else
            {
                SellButton.Visible = false;
                SellButton.Disabled = false;
            }          
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

        public void Order(OrderType OrderType = OrderType.Default)
        {
            RemoveSlots();

            switch (OrderType)
            {
                case OrderType.Default:
                    InitInventoryItems();
                    break;

                case OrderType.Price:
                    InitInventoryItems();
                    if (CurrentButtonOrderPriceSide)
                        SortGridContainer(new BaseSlot.Comparers.PriceAsc());
                    else
                        SortGridContainer(new BaseSlot.Comparers.PriceDesc());
                    break;
                case OrderType.Count:
                    InitInventoryItems();
                    if (CurrentButtonOrderCountSide)
                        SortGridContainer(new BaseSlot.Comparers.AmountAsc());
                    else
                        SortGridContainer(new BaseSlot.Comparers.AmountDesc());
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
                    slot.MoveSlotToSellContainer += Slot_MoveSlotToSellContainer1;
                }
            }
        }
        private void Slot_MoveSlotToSellContainer1(object sender, (sell_slot, int) e)
        {
            if (!e.Item1.InSellContainer)
            {
                AddSlotToSellContainer(e.Item1, e.Item2, SellItemContainer);

                if (Options.safeSelling)
                    return;

                var playerController = this.GetPlayerController();
                playerController.Gold += e.Item1.ItemDatabaseRow.SellPrice * e.Item2;

                InventoryComponent.RemoveItem(e.Item1.ItemDatabaseRow.Id, e.Item2);

            }
            else if (e.Item1.InSellContainer)
            {
                AddSlotToSellContainer(e.Item1, e.Item2, InventoryItemContainer);

                if (Options.safeSelling)
                    return;

                var playerController = this.GetPlayerController();
                playerController.Gold -= e.Item1.ItemDatabaseRow.SellPrice * e.Item2;
                                   
                InventoryComponent.AddItem(e.Item1.ItemDatabaseRow.Id, e.Item2);

                Order(currentOrderType);
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
                OrderContainer.GetChild<OrderButton>(i).ButtonClicked += SellWindow_ButtonClicked;
            }
        }

        private void SellWindow_ButtonClicked(object sender, ButtonEventData e)
        {
            currentOrderType = e.OrderType;

            if(e.OrderType == OrderType.Count)
                CurrentButtonOrderCountSide = e.ForLow;
            else if(e.OrderType == OrderType.Price)
                CurrentButtonOrderPriceSide = e.ForLow;

            Order(e.OrderType);
        }

        private void RefreshCoinsCount(int gold)
        {
            CoinsLabel.Text = gold.ToString();
        }

        private void AddSlotToSellContainer(sell_slot slot, int amount, GridContainer container)
        {
            for (int i = 0; i < container.GetChildCount(); i++)
            {
                var currentSlot = container.GetChild<sell_slot>(i);

                if (currentSlot.ItemDatabaseRow.Id == slot.ItemDatabaseRow.Id)
                {
                    currentSlot.Amount += amount;
                    slot.Amount -= amount;

                    return;
                }
            }

            var newSellSlot = Scenes.Widgets.Shop.SellSlot();

            container.AddChild(newSellSlot);

            newSellSlot.Init(slot.ItemDatabaseRow, amount, this);

            slot.Amount -= amount;

            newSellSlot.InSellContainer = !slot.InSellContainer;
            newSellSlot.MoveSlotToSellContainer += Slot_MoveSlotToSellContainer1;
        }

        private void ChangeColumnsNumber()
        {
            Vector2 itemSize = new Vector2(110, 110);

            if (Convert.ToInt32(InventoryItemContainer.GetParent<ScrollContainer>().Size.X / itemSize.X) <= 2)
            {
                InventoryItemContainer.Columns = 2;
                SellItemContainer.Columns = 2;
            }
            else
            {
                InventoryItemContainer.Columns = Convert.ToInt32(InventoryItemContainer.GetParent<ScrollContainer>().Size.X / itemSize.X);
                SellItemContainer.Columns = Convert.ToInt32(InventoryItemContainer.GetParent<ScrollContainer>().Size.X / itemSize.X);
            }
        }
    }
}
