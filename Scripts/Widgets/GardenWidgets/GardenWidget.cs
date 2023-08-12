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
            InventoryWidget = GetNode<InventoryWidget>("InventoryWidget");
            LabelGold = GetNode<Label>("HBoxContainer/LabelGold");
        }

        internal void Init(PlayerController playerController)
        {
            InventoryWidget.SetInventory(playerController.InventoryComponentSeeds);
            UpdateGold(playerController.Gold);
        }
        public void UpdateGold(int newGold)
        {
            LabelGold.Text = newGold.ToString();
        }
    }
}
