using Godot;
using Widgets.Inventory;
using Items;
using Controllers;
using System;

namespace Widgets.GardenWidgets
{
    public partial class GardenWidget : Control
    {
        InventoryWidget InventoryWidget { get; set; }
        public override void _Ready()
        {
            InventoryWidget = GetNode<InventoryWidget>("InventoryWidget");
        }

        internal void Init(PlayerController playerController)
        {
            InventoryWidget.SetInventory(playerController.InventoryComponentSeeds);
        }
    }
}
