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
        private int amount;
        public TextureRect TextureRect { get; set; }
        public Label LabelAmount { get; set; }

        public void Init(int itemId, int amountToSet)
        {
            ItemDatabaseRow databaseRow = DbService.GetItem(itemId);
            TextureRect.Texture = GD.Load<Texture2D>(databaseRow.TextureSpritePath);
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
        public override void _Ready()
        {
            TextureRect = GetNode<TextureRect>("TextureRect");
            LabelAmount = GetNode<Label>("LabelAmount");
        }

        public override void _GuiInput(InputEvent e)
        {
            base._GuiInput(e);
            if (e is InputEventMouseButton mouseButton && mouseButton.ButtonIndex==MouseButton.Left && mouseButton.IsPressed() == true)
            {
                PlayerController playerController = this.GetPlayerController();
                ItemType itemType = DbService.GetItemType(ItemId);
                Item item= Item.GetSceneByType(itemType);
                item.InitializeItem(ItemId);
                ///spawn item in world and make it current pressed object
                Node ownerParent = playerController.GetParent();
                ownerParent.AddChild(item);
                ownerParent.MoveChild(item, playerController.GetIndex());
                item.GlobalPosition = playerController.CameraBase.GlobalPosition + playerController.CameraBase.GlobalTransform.Basis.Y * 2;
                Amount--;
                playerController.CurrentPressedObject = item;
                playerController.CurrentPressedObject.LeftMouseDownListener(mouseButton, playerController);
            }
            else if (e is InputEventMouseButton mouseButtonUp && mouseButtonUp.ButtonIndex == MouseButton.Left && mouseButtonUp.IsPressed() == false)
            {
                mouseButtonUp.GlobalPosition = GetViewport().GetMousePosition();
                PlayerController playerController = this.GetPlayerController();
                playerController._UnhandledInput(mouseButtonUp);
            }
        }
    }
}
