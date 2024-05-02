using Controllers;
using Enums;
using Godot;
using Widgets.Inventory;

namespace Widgets.GardenWidgets
{
    public partial class BattlefieldWidget :MainWidget
    {
        public WorldTimer WorldTimer { get; set; }
        public InventoryWidget InventoryWidget { get; set; }

        public override void _Ready()
        {
            base._Ready();
            WorldTimer = GetNode<WorldTimer>("WorldTimer");
            WorldTimer.Init(WorldTimerMode.CountDown, 3);
            GD.Print("BattlefieldWidget ready finished");
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

    }
}
