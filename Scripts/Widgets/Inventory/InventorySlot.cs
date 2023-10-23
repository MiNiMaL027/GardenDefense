using Controllers;
using Enums;
using Godot;
using Items;
using System;
using Widgets.ContextMenu;
using Widgets.ToolTip;

namespace Widgets.Inventory
{
    public partial class InventorySlot : BaseSlot
    {
        public int ItemId { get; set; }
       
        public InventoryWidget parentWidget;
        public TextureRect TextureRect { get; set; }

        Item item;

        public ItemTooltip itemTooltip;

        public event EventHandler<(InventorySlot, int, bool)> ItemChanged;


        public void Init(ItemDatabaseRow item, int amountToSet, InventoryWidget parentWidgetToSet)
        {
            parentWidget = parentWidgetToSet;
            ItemDatabaseRow = item;
            TextureRect.Texture = GD.Load<Texture2D>(ItemDatabaseRow.TextureSpritePath);
            ItemId = item.Id;
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
            itemTooltip = Item.GetTooltipSceneByType(ItemDatabaseRow.ItemType);
            Vector2 globalMousePosition = GetViewport().GetMousePosition();

            AddChild(itemTooltip);

            itemTooltip.TopLevel = true;

            itemTooltip.ShowTooltipDbRow(ItemDatabaseRow);
            itemTooltip.AdjustControlInViewport(globalMousePosition);
            itemTooltip.PostInit();
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

                if (item is Pot)
                {
                    if (GameInstance.World.PutArea.isEnable)
                    {
                        ownerParent.AddChild(item);
                        item.GlobalPosition = GameInstance.World.PutArea.SpawnPosition;
                        item.InitializeItem(ItemDatabaseRow);

                        parentWidget.InventoryComponent.RemoveItem(ItemId, 1);
                    }
                    else
                    {
                        this.GetPlayerController().Hud.GardenWidget.InfoWindow.AddInfoPanel("Area is disable, please move all object from it");
                    }
                }                     
                else
                {
                    ownerParent.AddChild(item);
                    ownerParent.MoveChild(item, playerController.GetIndex());

                    item.GlobalPosition = playerController.CameraBase.GlobalPosition + new Vector3(7,0,3) + playerController.CameraBase.GlobalTransform.Basis.Y * 2;
                    item.InitializeItem(ItemDatabaseRow);

                    playerController.CurrentPressedObject = item;
                    playerController.CurrentPressedObject.LeftMouseDownListener(mouseButton, playerController);

                    parentWidget.InventoryComponent.RemoveItem(ItemId, 1);
                }              
            }
            else if (e is InputEventMouseButton mouseButtonUp && mouseButtonUp.ButtonIndex == MouseButton.Left && mouseButtonUp.IsPressed() == false)
            {
                mouseButtonUp.GlobalPosition = GetViewport().GetMousePosition();
                PlayerController playerController = this.GetPlayerController();

                playerController._UnhandledInput(mouseButtonUp);

                item.LinearVelocity = Vector3.Zero;
            }
            else if(e is InputEventMouseButton rightMouseButtonDown && rightMouseButtonDown.ButtonIndex == MouseButton.Right && rightMouseButtonDown.IsPressed() == false)
            {
                ItemContextMenu itemContextMenu = Scenes.Widgets.ContextMenu.ItemContextMenu();
                var playerController = this.GetPlayerController();

                playerController.RemoveOpenedContextMenu();
                playerController.OpenedContextMenu = itemContextMenu;
                playerController.Hud.AddChild(itemContextMenu);

                itemContextMenu.Init(null, this, playerController, true);
                playerController.Hud.AddAtMousePosition(itemContextMenu);
            }
        }

        public void RemoveOrSell(int amount, bool sell)
        {
            ItemChanged?.Invoke(this, (this, amount, sell));
        }
    }
}
