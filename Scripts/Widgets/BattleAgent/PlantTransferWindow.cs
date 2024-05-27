using Enums;
using Godot;
using Items;
using System;
using System.Collections.Generic;
using System.Linq;
using Widgets.Shop;
namespace Widgets.BattleAgent
{
    public partial class PlantTransferWindow : Control
    {
        public InventoryComponent InventoryComponent { get; set; }
        public List<sell_slot> BattlePlantsSlots { get; set; } = new List<sell_slot>();
        public GridContainer InventoryItemContainer { get; set; }
        public GridContainer TranferItemContainer { get; set; }

        List<sell_slot> TransferSlots = new List<sell_slot>();

        public Button FightButton { get; set; }
        public Label lvlLabel { get; set; }

        int Lvl;
        int SlotCount;

        public override void _Ready()
        {
            InventoryItemContainer = GetNode<GridContainer>("PanelContainer/MarginContainer/VBoxContainer/HBoxContainer/VBoxContainer2/InventoryPlants");
            TranferItemContainer = GetNode<GridContainer>("PanelContainer/MarginContainer/VBoxContainer/HBoxContainer/VBoxContainer/TransferPlants");
            FightButton = GetNode<Button>("PanelContainer/MarginContainer/VBoxContainer/FightButton");
            lvlLabel = GetNode<Label>("PanelContainer/MarginContainer/VBoxContainer/Label");

            InventoryComponent = this.GetPlayerController().MainInventory;

            FightButton.Pressed += FightButton_Pressed;

            InitInventoryItems();
        }

        private void FightButton_Pressed()
        {
            if (TransferSlots.FirstOrDefault().isEmpty)
            {
                this.GetPlayerController().Hud.GardenWidget.InfoWindow.AddInfoPanel("You must pick one of your battle plants");
                return;
            }
            var pickedBattlePlants = new Dictionary<int, int>();
            foreach (var item in TransferSlots)
            {
                if (item.isEmpty)
                    continue;

                InventoryComponent.RemoveItem(item.ItemDatabaseRow.Id, item.Amount);

                pickedBattlePlants.Add(item.ItemDatabaseRow.Id, item.Amount);
            }

            var lvlScene = ResourceLoader.Load<PackedScene>($"res://Scenes/Worlds/Levels/{Lvl}.tscn").Instantiate<Battlefield>();
            QueueFree();
            Node parent = FindParent("BattleAgentWindow");
            if(parent != null && parent is BattleAgentWindow)
            {
                parent.QueueFree();
            }
            GameInstance.Instance.ChangeWorld(lvlScene);

            lvlScene.Init(Lvl, pickedBattlePlants);
        }

        public void Init(int lvl, int slotCount)
        {
            Lvl = lvl;
            lvlLabel.Text = $"lvl - {lvl}";
            SlotCount = slotCount;

            for (int i = 0; i < slotCount; i++)
            {
                var slot = Scenes.Widgets.Shop.SellSlot();
                TranferItemContainer.AddChild(slot);
                slot.Empty();

                TransferSlots.Add(slot);
            }
        }

        private void InitInventoryItems()
        {
            for (int i = 0; i < InventoryComponent.InventoryIdArray.Count; i++)
            {
                var item = DbService.GetItem(InventoryComponent.InventoryIdArray[i]);
                if (item.ItemType != ItemType.BattlePlant)
                    continue;

                sell_slot slot = Scenes.Widgets.Shop.SellSlot();

                BattlePlantsSlots.Add(slot);
                InventoryItemContainer.AddChild(slot);

                slot.Init(item, InventoryComponent.InventoryAmountArray[i], this, false);
                slot.MoveSlotToSellContainer += Slot_MoveSlotToSellContainer;
            }
        }

        private void Slot_MoveSlotToSellContainer(object sender, (sell_slot, int) e)
        {                        
            if (!e.Item1.InSellContainer)
            {
                if (TransferSlots.Where(s => s.isEmpty).Count() <= 0)
                {
                    this.GetPlayerController().Hud.GardenWidget.InfoWindow.AddInfoPanel("Don`t have empty slot");
                    return;
                }

                AddSlotToSellContainer(e.Item1, e.Item2, TranferItemContainer);

                if (e.Item1.isEmpty)
                    return;
            }
            else if (e.Item1.InSellContainer)
            {
                AddSlotToSellContainer(e.Item1, e.Item2, InventoryItemContainer);

                if (e.Item1.isEmpty)
                    return;
            }
        }

        private void AddSlotToSellContainer(sell_slot slot, int amount, GridContainer container)
        {
            for (int i = 0; i < container.GetChildCount(); i++)
            {
                var currentSlot = container.GetChild<sell_slot>(i);

                if (currentSlot.isEmpty)
                {
                    currentSlot.Init(slot.ItemDatabaseRow, amount, this, false);
                    slot.Amount -= amount;

                    currentSlot.InSellContainer = !slot.InSellContainer;
                    currentSlot.MoveSlotToSellContainer += Slot_MoveSlotToSellContainer;

                    return;
                }

                if (currentSlot.ItemDatabaseRow.Id == slot.ItemDatabaseRow.Id)
                {
                    currentSlot.Amount += amount;
                    slot.Amount -= amount;

                    return;
                }
            }

            var newSellSlot = Scenes.Widgets.Shop.SellSlot();

            container.AddChild(newSellSlot);

            newSellSlot.Init(slot.ItemDatabaseRow, amount, this, false);

            slot.Amount -= amount;

            newSellSlot.InSellContainer = !slot.InSellContainer;
            newSellSlot.MoveSlotToSellContainer += Slot_MoveSlotToSellContainer;
        }
    }


}
