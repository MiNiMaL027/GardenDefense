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
        Item item;
        public event EventHandler<(InventorySlot, int, bool)> ItemChanged;
        
        

        public override void _Ready()
        {
            TextureRect = GetNode<TextureRect>("TextureRect");
            LabelAmount = GetNode<Label>("LabelAmount");
            base._Ready();
        }

        public override void _GuiInput(InputEvent e)
        {
            base._GuiInput(e);

            if (e is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == MouseButton.Left && mouseButton.IsPressed() == true)
            {
                PlayerController playerController = this.GetPlayerController();
                ItemType itemType = DbService.GetItemType(ItemDatabaseRow.Id);
                item = Item.GetItemSceneByType(itemType);

                ///spawn item in world and make it current pressed object
                Node ownerParent = playerController.GetParent();

                if (item is Pot)
                {
                    if (GameInstance.World is Farm f && f.PutArea.isEnable)
                    {
                        ownerParent.AddChild(item);
                        item.GlobalPosition = f.PutArea.SpawnPosition;
                        item.InitializeItem(ItemDatabaseRow);

                        (FindParent("InventoryWidget") as InventoryWidget).InventoryComponent.RemoveItem(ItemDatabaseRow.Id, 1);
                    }
                    else
                    {
                        playerController.Hud.GardenWidget.InfoWindow.AddInfoPanel("Area is disabled, please move all object from it");
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

                    (FindParent("InventoryWidget") as InventoryWidget).InventoryComponent.RemoveItem(ItemDatabaseRow.Id, 1);
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
