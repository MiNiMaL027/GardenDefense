using Controllers;
using Enums;
using Godot;
using Interfaces;
using Items;
using System;

namespace Widgets.Inventory
{
    public partial class InventorySlot : Panel
    {
        public int ItemId { get; set; }
        public int Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                if (amount > 1) //display count of items
                {
                    LabelAmount.Text = amount.ToString();
                }
                else if (amount == 1) //text shouldn't be displayed
                {
                    LabelAmount.Text = "";
                }
                else //remove from screen if amount 0
                {
                    QueueFree();
                }
            }
        }
        public InventoryWidget parentWidget;
        private int amount;
        public TextureRect TextureRect { get; set; }
        public Label LabelAmount { get; set; }

        Item item;
        public ItemTooltip itemTooltip;
        public ItemDatabaseRow itemDatabaseRow;


        public void Init(int itemId, int amountToSet, InventoryWidget parentWidgetToSet)
        {
            parentWidget= parentWidgetToSet;
            itemDatabaseRow = DbService.GetItem(itemId);
            TextureRect.Texture = GD.Load<Texture2D>(itemDatabaseRow.TextureSpritePath);
            ItemId = itemId;
            amount = amountToSet;
            if (amount > 1)
            {
                LabelAmount.Text = amount.ToString();
            }
            else
            {
                LabelAmount.Text = "";

            }
        }

        public void InventorySlot_MouseExited()
        {
            if (itemTooltip != null)
            {
                itemTooltip.HideTooltip();
                itemTooltip = null;
            }
        }

        private void InventorySlot_MouseEntered()
        {
            itemTooltip = Item.GetTooltipSceneByType(itemDatabaseRow.ItemType);
            Vector2 globalMousePosition = GetViewport().GetMousePosition();
            AddChild(itemTooltip);
            itemTooltip.TopLevel = true;
            itemTooltip.ShowTooltipDbRow(itemDatabaseRow);
            itemTooltip.AdjustControlInViewport(globalMousePosition);
            itemTooltip.init();
        }

        public override void _Ready()
        {
            TextureRect = GetNode<TextureRect>("TextureRect");
            LabelAmount = GetNode<Label>("LabelAmount");
            MouseEntered += InventorySlot_MouseEntered;
            MouseExited += InventorySlot_MouseExited;
        }

        public override void _GuiInput(InputEvent e)
        {
            base._GuiInput(e);
            if (e is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == MouseButton.Left && mouseButton.IsPressed() == true)
            {
                PlayerController playerController = this.GetPlayerController();
                ItemType itemType = DbService.GetItemType(ItemId);
                item = Item.GetItemSceneByType(itemType);

                ///spawn item in world and make it current pressed object
                Node ownerParent = playerController.GetParent();
                ownerParent.AddChild(item);
                ownerParent.MoveChild(item, playerController.GetIndex());

                item.GlobalPosition = playerController.CameraBase.GlobalPosition + playerController.CameraBase.GlobalTransform.Basis.Y * 2;
                item.InitializeItem(itemDatabaseRow);
                playerController.CurrentPressedObject = item;
                playerController.CurrentPressedObject.LeftMouseDownListener(mouseButton, playerController);

                parentWidget.InventoryComponent.RemoveItem(ItemId, 1);

            }
            else if (e is InputEventMouseButton mouseButtonUp && mouseButtonUp.ButtonIndex == MouseButton.Left && mouseButtonUp.IsPressed() == false)
            {
                mouseButtonUp.GlobalPosition = GetViewport().GetMousePosition();
                PlayerController playerController = this.GetPlayerController();
                playerController._UnhandledInput(mouseButtonUp);
                item.LinearVelocity = Vector3.Zero;
            }
        }
    }
}
