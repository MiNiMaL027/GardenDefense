using Godot;
using Widgets.Inventory;
using Controllers;
using Widgets.Global;
using Enums;
using Widgets.BattleAgent;

namespace Widgets.GardenWidgets
{
    public partial class GardenWidget : MainWidget
    {
        public InventoryWidget InventoryWidget { get; set; }
        public BattleAgentWindow BattleAgentWindow { get; set; }
        public Button FightButton { get; set; }

        public override void _Ready()
        {
            base._Ready();
            
            FightButton = GetNode<Button>("FightContainer/FightButton");
            FightButton.Pressed += FightButton_Pressed;
        }

        private void FightButton_Pressed()
        {
             OpenBattleAgentWindow();
        }
        public override void OpenInventory()
        {
            InventoryWidget = Scenes.Widgets.Inventory.InventoryWidget();

            AddChild(InventoryWidget);
            InventoryWidget.SetInventory(this.GetPlayerController().MainInventory);
        }

        public override void CloseInventory()
        {
            PlayerController playerController = this.GetPlayerController();
            if (playerController.OpenedContextMenu != null && playerController.OpenedContextMenu.isInventorySlot)
                playerController.RemoveOpenedContextMenu();

            InventoryWidget.QueueFree();
            
            InventoryWidget = null;
        }

        public void OpenBattleAgentWindow()
        {
            BattleAgentWindow = Scenes.Widgets.PlantTransfer.BattleAgentWindow();

            AddChild(BattleAgentWindow);
        }

        public void CloseBattleAgentWindow()
        {
            BattleAgentWindow.QueueFree();

            BattleAgentWindow = null;
        }

        public override void ToggleInventory()
        {
            if(InventoryWidget != null)
            {
                CloseInventory();
            }
            else
            {
                OpenInventory();
            }
        }
    }
}
