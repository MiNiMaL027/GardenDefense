using Godot;
using Widgets.Inventory;
using Items;
using Controllers;
using System;

namespace Widgets.GardenWidgets
{
    public partial class GardenWidget : Control
    {
        public InventoryWidget InventoryWidget { get; set; }
        Label LabelGold { get; set; }

        public override void _Ready()
        {
            LabelGold = GetNode<Label>("HBoxContainer/LabelGold");
        }

        internal void Init(PlayerController playerController)
        {
            UpdateGold(playerController.Gold);
        }

        public void UpdateGold(int newGold)
        {
            LabelGold.Text = newGold.ToString();
        }

        public void OpenInventory()
        {
            InventoryWidget = Scenes.Widgets.Inventory.InventoryWidget();

            AddChild(InventoryWidget);
            InventoryWidget.SetInventory(this.GetPlayerController().InventoryComponentSeeds);
        }

        public void CloseInventory()
        {
            if (this.GetPlayerController().OpenedContextMenu != null && this.GetPlayerController().OpenedContextMenu.isInventorySlot)
                this.GetPlayerController().RemoveOpenedContextMenu();

            InventoryWidget.QueueFree();
            
            InventoryWidget = null;
        }
    }
}
