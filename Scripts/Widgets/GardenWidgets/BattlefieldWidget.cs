using Controllers;
using Enums;
using Godot;
using Pawns.BattlePlants;
using System;
using Widgets.Inventory;
using static Scenes;

namespace Widgets.GardenWidgets
{
    public partial class BattlefieldWidget : MainWidget
    {
        public Button ButtonBackToFarm { get; set; }
        public WorldTimer WorldTimer { get; set; }
        public InventoryWidget BattlePlantsItemsInventoryWidget { get; set; }
        public InventoryWidget AnotherItemsInventoryWidget { get; set; }
        public EnergyContainer EnergyContainer { get; set; }
        public WaveCounterWidget WaveCounterWidget { get; set; }
        public ChooseSkillWidget ChooseSkillWidget { get; set; }
        public Label MutagenLabel { get; set; }

        public override void _Ready()
        {
            base._Ready();
            ButtonBackToFarm = GetNode<Button>("ButtonBackToFarm");
            EnergyContainer = GetNode<EnergyContainer>("EnergyContainer");
            ButtonBackToFarm.Pressed += ButtonBackToFarm_Pressed;
            WorldTimer = GetNode<WorldTimer>("WorldTimer");          
            this.GetPlayerController().EnergyUpdated += UpdateEnergy;
            BattlePlantsItemsInventoryWidget = GetNode<InventoryWidget>("BattlePlantsInventoryWidget");
            MutagenLabel = GetNode<Label>("MarginContainer/PanelContainer/MarginContainer/HBoxContainer/mutagenLabel");
            GameInstance.PlayerController.MutagenUpdate += PlayerController_MutagenUpdate;
        }

        private void PlayerController_MutagenUpdate(int mutagenCount)
        {
            MutagenLabel.Text = mutagenCount.ToString();
        }

        public override void _ExitTree()
        {
            base._ExitTree();

            this.GetPlayerController().EnergyUpdated -= UpdateEnergy;
        }

        private void ButtonBackToFarm_Pressed()
        {
            GameInstance.Instance.ChangeWorld(Scenes.Worlds.Farm());
        }

        public override void OpenInventory()
        {
            AnotherItemsInventoryWidget = Scenes.Widgets.Inventory.InventoryWidget();

            AddChild(AnotherItemsInventoryWidget);
            AnotherItemsInventoryWidget.SetInventory(this.GetPlayerController().BattlefieldInventory, new BaseSlot.Comparers.DefaultAsc(), ItemType.Misc, ItemType.Fertilizer, ItemType.Harvestable, ItemType.Pot, ItemType.Seed);
        }

        public override void CloseInventory()
        {
            PlayerController playerController = this.GetPlayerController();
            if (playerController.OpenedContextMenu != null && playerController.OpenedContextMenu.isInventorySlot)
                playerController.RemoveOpenedContextMenu();

            AnotherItemsInventoryWidget.QueueFree();

            AnotherItemsInventoryWidget = null;
        }
        public override void ToggleInventory()
        {
            if (AnotherItemsInventoryWidget != null)
            {
                CloseInventory();
            }
            else
            {
                OpenInventory();
            }
        }
        public void UpdateEnergy(int energy)
        {
            EnergyContainer.Refresh(energy);
        }

        public void AddWaveCounterWidget(Stage[] stages)
        {
            if (WaveCounterWidget != null)
                return;

            WaveCounterWidget = Scenes.Widgets.BattleWidget.WaveCounterWidget();
            AddChild(WaveCounterWidget);

            WaveCounterWidget.Init(stages);
        }
        public void OpenChooseSkillWidget(BaseBattlePlant battlePlant)
        {
            if(ChooseSkillWidget != null) return;

            ChooseSkillWidget = Scenes.Widgets.Skills.ChooseSkillWidget();
            AddChild(ChooseSkillWidget);
            ChooseSkillWidget.Init(battlePlant);
        }

        public void CloseChooseSkillWidget()
        {
            if (ChooseSkillWidget == null) return;

            ChooseSkillWidget.QueueFree();
            ChooseSkillWidget = null;
        }
    }
}
