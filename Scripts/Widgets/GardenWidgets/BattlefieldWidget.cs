using Controllers;
using Enums;
using Godot;
using System;
using Widgets.Inventory;

namespace Widgets.GardenWidgets
{
    public partial class BattlefieldWidget : MainWidget
    {
        public Button ButtonBackToFarm { get; set; }
        public WorldTimer WorldTimer { get; set; }
        public InventoryWidget InventoryWidget { get; set; }
        public Label EnergyLabel { get; set; }

        public override void _Ready()
        {
            base._Ready();
            ButtonBackToFarm = GetNode<Button>("ButtonBackToFarm");
            EnergyLabel = GetNode<Label>("EnergyContainer/EnergyLabel");
            ButtonBackToFarm.Pressed += ButtonBackToFarm_Pressed;
            WorldTimer = GetNode<WorldTimer>("WorldTimer");
            WorldTimer.Init(WorldTimerMode.CountDown, 3);
            GD.Print("BattlefieldWidget.Ready() finished");
        }

        private void ButtonBackToFarm_Pressed()
        {
            GameInstance.Instance.ChangeWorld(Scenes.Worlds.Farm());
        }

        public override void OpenInventory()
        {
            InventoryWidget = Scenes.Widgets.Inventory.InventoryWidget();

            AddChild(InventoryWidget);
            InventoryWidget.SetInventory(this.GetPlayerController().BattlefieldInventory);
        }

        public override void CloseInventory()
        {
            PlayerController playerController = this.GetPlayerController();
            if (playerController.OpenedContextMenu != null && playerController.OpenedContextMenu.isInventorySlot)
                playerController.RemoveOpenedContextMenu();

            InventoryWidget.QueueFree();

            InventoryWidget = null;
        }
        public override void ToggleInventory()
        {
            if (InventoryWidget != null)
            {
                CloseInventory();
            }
            else
            {
                OpenInventory();
            }
        }
        public void UpdateEnergy(float energy)
        {
            EnergyLabel.Text = energy.ToString();
        }
    }
}
