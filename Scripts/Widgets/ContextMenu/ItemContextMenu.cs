using System;
using System.Linq;
using Controllers;
using Godot;
using Items;
using Widgets.Inventory;

namespace Widgets.ContextMenu
{
    public partial class ItemContextMenu : Control
    {
        protected PlayerController playerController;
        protected HBoxContainer Container;
        Item targetItem;
        InventorySlot InventorySlot;
        protected Timer timerConfirm;
        public bool isInventorySlot;

        public override void _Ready()
        {
            Container = GetNode<HBoxContainer>("HBoxContainer");
            timerConfirm = new Timer();
            timerConfirm.WaitTime = 0.8;
            timerConfirm.OneShot= true;

            AddChild(timerConfirm);
        }

        public virtual void Init(Item item, InventorySlot inventorySlot, PlayerController playerControllerToSet, bool isInInventory)
        {
            InventorySlot = inventorySlot;
            targetItem = item;

            if(inventorySlot != null)
            {
                inventorySlot.ItemChanged += InventorySlot_ItemChanged;
            }

            playerController = playerControllerToSet;
           
            isInventorySlot = isInInventory;

            AddButton(new TextureButton(), "Details", "res://raw assets/Images/ToolsButton/Detail.png", Details_Pressed);

            if(isInInventory == false)
            {
                AddButton(new TextureButton(), "Move to inventory", "res://raw assets/Images/ToolsButton/MoveToBag.png", MoveToInventory_Pressed);
            }

            AddButton(Scenes.Widgets.ContextMenu.TextureButtonTimeShader(), "Sell", "res://raw assets/Images/ToolsButton/Sell.png", Sell_ButtonDown, Sell_ButtonUp);

            AddButton(Scenes.Widgets.ContextMenu.TextureButtonTimeShader(), "Delete", "res://raw assets/Images/ToolsButton/Deletel.png", Delete_ButtonDown, Delete_ButtonUp);
        }

        private void InventorySlot_ItemChanged(object sender, (InventorySlot, int, bool) e)
        {
            if (e.Item3)
                playerController.Gold += e.Item1.ItemDatabaseRow.SellPrice * e.Item2; 

             playerController.GardenInventory.RemoveItem(e.Item1.ItemDatabaseRow.Id, e.Item2);
        }

        #region Delete
        public void Delete_ButtonDown()
        {
            Container.GetNode<TextureButtonTimeShader>("Delete").SetShaderMaterial(GD.Load<ShaderMaterial>("res://Shaders/Materials/ConfirmationCircleShader.tres"));
            timerConfirm.Timeout += Delete_Pressed_Confirm_Timeout;
            timerConfirm.Start();
        }

        public virtual void Delete_Pressed_Confirm_Timeout()
        {
            Container.GetNode<TextureButtonTimeShader>("Delete").Material = null;

            if(isInventorySlot)
            {
                var amountWindow = Scenes.Widgets.Inventory.InventoryAmountWindow();
                playerController.Hud.AddChild(amountWindow);
                playerController.Hud.AddAtMousePosition(amountWindow.GetChild<Panel>(0));
                amountWindow.Init(InventorySlot.Amount, InventorySlot.ItemDatabaseRow.SellPrice, false);
                amountWindow.ButtonPressedAccept += AmountWindow_ButtonPressedAccept;
            }
            else
            {
                if (targetItem is Pot pot)
                {
                    if (pot.sockets.FirstOrDefault(s => s.IsUsed) != null)
                    {
                        playerController.Hud.MainWidget.InfoWindow.AddInfoPanel($"can`t delete pot, bacause pot have used socked", targetItem.TextureSpritePath);

                        return;
                    }
                }

                targetItem.QueueFree();
            }

            playerController.RemoveOpenedContextMenu();
            timerConfirm.Timeout -= Delete_Pressed_Confirm_Timeout;

        }

        public void Delete_ButtonUp()
        {
            Container.GetNode<TextureButtonTimeShader>("Delete").Material = null;
            timerConfirm.Stop();
            timerConfirm.Timeout -= Delete_Pressed_Confirm_Timeout;
        }
        #endregion
        #region Sell

        public void Sell_ButtonDown()
        {
            Container.GetNode<TextureButtonTimeShader>("Sell").SetShaderMaterial(GD.Load<ShaderMaterial>("res://Shaders/Materials/ConfirmationCircleShader.tres"));
            timerConfirm.Timeout += Sell_Pressed_Confirm_Timeout;

            timerConfirm.Start();
        }

        public virtual void Sell_Pressed_Confirm_Timeout()
        {
            Container.GetNode<TextureButtonTimeShader>("Sell").Material = null;

            if (isInventorySlot)
            {
                var amountWindow = Scenes.Widgets.Inventory.InventoryAmountWindow();

                playerController.Hud.AddChild(amountWindow);
                playerController.Hud.AddAtMousePosition(amountWindow.GetChild<Panel>(0));
                amountWindow.Init(InventorySlot.Amount, InventorySlot.ItemDatabaseRow.SellPrice, true);
                amountWindow.ButtonPressedAccept += AmountWindow_ButtonPressedAccept;
            }
            else
            {
                if(targetItem is Pot pot)
                {
                    if(pot.sockets.FirstOrDefault(s => s.IsUsed) != null)
                    {
                        playerController.Hud.MainWidget.InfoWindow.AddInfoPanel($"can`t sell pot, bacause pot have used socked", targetItem.TextureSpritePath);

                        return;
                    }
                }

                targetItem.QueueFree();

                playerController.Gold += targetItem.SellPrice;

                playerController.Hud.MainWidget.InfoWindow.AddInfoPanel($"{targetItem.ItemName} was sold", targetItem.TextureSpritePath);
            }

            playerController.RemoveOpenedContextMenu();    
            
            timerConfirm.Timeout -= Sell_Pressed_Confirm_Timeout;
        }

        private void AmountWindow_ButtonPressedAccept(int amount, bool sell)
        {
            playerController.Hud.MainWidget.InfoWindow.AddInfoPanel($"item was sold");

            InventorySlot.RemoveOrSell(amount, sell);           
        }

        public void Sell_ButtonUp()
        {
            Container.GetNode<TextureButton>("Sell").Material = null;

            timerConfirm.Stop();
            timerConfirm.Timeout -= Sell_Pressed_Confirm_Timeout;
        }
        #endregion

        public virtual void MoveToInventory_Pressed()
        {
            targetItem.MoveToInventory(playerController);
            playerController.RemoveOpenedContextMenu();
        }

        public virtual void Details_Pressed()
        {
            playerController.Hud.OpenBestiary();

            if (!playerController.Hud.BestiaryWindow.OpenExactItem(targetItem.ItemType, targetItem.EditorItemId))
            {
                playerController.Hud.CloseBestiary();
                playerController.Hud.GardenWidget.InfoWindow.AddInfoPanel($"Right Now you do not know enough about {targetItem.ItemName}. If you want to know more you can use it more");
            }

            playerController.RemoveOpenedContextMenu();
        }

        protected void AddButton(TextureButton button, string name, string texturePath, Action buttonDown, Action buttonUp)
        {
            button.Name = name;
            button.TextureNormal = ResourceLoader.Load<Texture2D>(texturePath);
            button.ButtonDown += buttonDown;

            if (buttonUp != null)
                button.ButtonUp += buttonUp;

            Container.AddChild(button);

            this.Size += new Vector2(64, 0);

            button.StretchMode = TextureButton.StretchModeEnum.KeepAspectCentered;
            button.IgnoreTextureSize = true;
            button.CustomMinimumSize = new Vector2(64, 64);
        }
        protected void AddButton(TextureButton button, string name, string texturePath, Action buttonDown)
        {
            button.Name = name;
            button.TextureNormal = ResourceLoader.Load<Texture2D>(texturePath);

            button.ButtonDown += buttonDown;

            Container.AddChild(button);

            this.Size += new Vector2(64,0);

            button.StretchMode = TextureButton.StretchModeEnum.KeepAspectCentered;
            button.IgnoreTextureSize = true;
            button.CustomMinimumSize = new Vector2(64, 64);
        }
    }
}
