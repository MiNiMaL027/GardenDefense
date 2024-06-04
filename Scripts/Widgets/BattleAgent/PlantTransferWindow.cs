using Controllers;
using Enums;
using Godot;
using Items;
using Pawns;
using Pawns.BattlePlants;
using Pawns.Monsters;
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
        public GridContainer MonstersContainer { get; set; }

        List<sell_slot> TransferSlots = new List<sell_slot>();

        public Button FightButton { get; set; }
        public Label lvlLabel { get; set; }
        public List<PackedScene> AvailableMonsters {  get; set; }
        public HBoxContainer IconsContainer { get; set; }

        int Lvl;
        int SlotCount;

        public override void _Ready()
        {
            InventoryItemContainer = GetNode<GridContainer>("PanelContainer/MarginContainer/HBoxContainer/VBoxContainer/HBoxContainer/VBoxContainer2/InventoryPlants");
            TranferItemContainer = GetNode<GridContainer>("PanelContainer/MarginContainer/HBoxContainer/VBoxContainer/HBoxContainer/VBoxContainer/TransferPlants");          
            MonstersContainer = GetNode<GridContainer>("PanelContainer/MarginContainer/HBoxContainer/PanelContainer/VBoxContainer2/GridContainer");
            FightButton = GetNode<Button>("PanelContainer/MarginContainer/HBoxContainer/VBoxContainer/FightButton");
            lvlLabel = GetNode<Label>("PanelContainer/MarginContainer/HBoxContainer/VBoxContainer/Label");
            

            InventoryComponent = this.GetPlayerController().GardenInventory;

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

            AvailableMonsters = ResourceLoader.Load<PackedScene>($"res://Scenes/Worlds/Levels/{Lvl}.tscn").Instantiate<Battlefield>().availableMonstersToSpawn.ToList();
            InitMonsters(AvailableMonsters);
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
                var hbox = slot.GetNode<HBoxContainer>("HBoxContainer2");               
                
                if(item is BattlePlantDataBaseRow battlePlant)
                {
                    var pawn = DbService.GetPawn(battlePlant.PawnId);
                    var pawnInstante = ResourceLoader.Load<PackedScene>(pawn.ScenePath).Instantiate<BaseBattlePlant>();
                    foreach(var pawnType in pawnInstante.PlantType.GetFlags())
                    {
                        var typeIcon = ResourceLoader.Load<Texture2D>($"res://raw assets/Images/Monsters/Type icon/{pawnType}.png");
                        hbox.AddIcon(typeIcon, new Vector2(20, 20), pawnType.ToString());
                    }                                   

                    foreach (var pawnClass in pawnInstante.Class.GetFlags())
                    {
                        var classIcon = ResourceLoader.Load<Texture2D>($"res://raw assets/Images/Monsters/Type icon/{pawnClass}.png");
                        hbox.AddIcon(classIcon, new Vector2(20, 20), pawnClass.ToString());
                    }
                }

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
            var icons = new List<ClassIcon>();
            foreach (var node in slot.IconsContainer.GetChildren())
            {
                if (node is ClassIcon icon)
                {
                    icons.Add(icon);
                }
            }

            for (int i = 0; i < container.GetChildCount(); i++)
            {
                var currentSlot = container.GetChild<sell_slot>(i);

                if (currentSlot.isEmpty)
                {
                    currentSlot.Init(slot.ItemDatabaseRow, amount, this, false, icons);
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
            
            newSellSlot.Init(slot.ItemDatabaseRow, amount, this, false, icons);

            slot.Amount -= amount;

            newSellSlot.InSellContainer = !slot.InSellContainer;
            newSellSlot.MoveSlotToSellContainer += Slot_MoveSlotToSellContainer;
        }

        private void InitMonsters(List<PackedScene> monsters)
        {
            foreach(var monster in monsters)
            {
                AIController a = monster.Instantiate<AIController>();
                BaseMonster m = (BaseMonster)a.GetChildren().FirstOrDefault(c => c is BaseMonster);
    
                var slot = Scenes.Widgets.PlantTransfer.MonsterSlot();
                MonstersContainer.AddChild(slot);
                slot.Init(m);
            }

            for (int i = 0; i < monsters.Count / 5; i++)
            {
                MonstersContainer.Columns++;
            }
        }
    }
}
