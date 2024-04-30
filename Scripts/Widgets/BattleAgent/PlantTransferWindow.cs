using Enums;
using Godot;
using Items;
using System;
using System.Collections.Generic;
using Widgets.Shop;

public partial class PlantTransferWindow : Control
{
    public InventoryComponent InventoryComponent { get; set; }
    public List<sell_slot> BattlePlantsSlots { get; set; } = new List<sell_slot>();
    public GridContainer InventoryItemContainer { get; set; }
    public GridContainer TranferItemContainer { get; set; }

    public Dictionary<int, int> PickedBattlePlants = new Dictionary<int, int>();

    public Button FightButton { get; set; }

    int Lvl;
    int SlotCount;

    public override void _Ready()
    {
        InventoryItemContainer = GetNode<GridContainer>("PanelContainer/MarginContainer/VBoxContainer/HBoxContainer/VBoxContainer2/InventoryPlants");
        TranferItemContainer = GetNode<GridContainer>("PanelContainer/MarginContainer/VBoxContainer/HBoxContainer/VBoxContainer/TransferPlants");
        FightButton = GetNode<Button>("PanelContainer/MarginContainer/VBoxContainer/FightButton");
        InventoryComponent = this.GetPlayerController().MainInventory;

        FightButton.Pressed += FightButton_Pressed;

        InitInventoryItems();
    }

    private void FightButton_Pressed()
    {
        foreach(var item in PickedBattlePlants)
        {
            InventoryComponent.RemoveItem(item.Key, item.Value);
        }

        var lvlScene = ResourceLoader.Load<PackedScene>($"res://Scenes/Levels/{Lvl}.tscn").Instantiate<Battlefield>();      

        this.GetGameInstance().RemoveChildren();
        this.GetGameInstance().AddChild(lvlScene);

        lvlScene.Init(Lvl, PickedBattlePlants);
    }

    public void Init(int lvl, int slotCount)
    {
        Lvl = lvl;
        SlotCount = slotCount;
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
            AddSlotToSellContainer(e.Item1, e.Item2, TranferItemContainer);

            var playerController = this.GetPlayerController();
            playerController.Gold += e.Item1.ItemDatabaseRow.SellPrice * e.Item2;
            var plantId = (e.Item1.ItemDatabaseRow as BattlePlantDataBaseRow).Id;
            if (PickedBattlePlants.ContainsKey(plantId))
                PickedBattlePlants[plantId] = PickedBattlePlants[plantId] + e.Item2;
            else
                PickedBattlePlants.Add(plantId, e.Item2);
        }
        else if (e.Item1.InSellContainer)
        {
            AddSlotToSellContainer(e.Item1, e.Item2, InventoryItemContainer);

            var playerController = this.GetPlayerController();
            playerController.Gold -= e.Item1.ItemDatabaseRow.SellPrice * e.Item2;

            var plantId = (e.Item1.ItemDatabaseRow as BattlePlantDataBaseRow).Id;
            if (PickedBattlePlants[plantId] > e.Item2)
                PickedBattlePlants[plantId] = PickedBattlePlants[plantId] - e.Item2;
            else
                PickedBattlePlants.Remove(plantId);
        }
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

        newSellSlot.Init(slot.ItemDatabaseRow, amount, this, false);

        slot.Amount -= amount;

        newSellSlot.InSellContainer = !slot.InSellContainer;
        newSellSlot.MoveSlotToSellContainer += Slot_MoveSlotToSellContainer;
    }
}

