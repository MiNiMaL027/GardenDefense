using System;
using Controllers;
using Godot;
using Items;
using Widgets.Inventory;

namespace Widgets.ContextMenu
{
    public partial class ItemContextMenu : Control
    {
        protected PlayerController playerController;
        Item targetItem;
        InventorySlot InventorySlot;
        Timer timerConfirm;
        bool isInventorySlot;
        public override void _Ready()
        {
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

            TextureButton Details = new TextureButton();
            Details.Name = "Details";
            Details.TextureNormal = ResourceLoader.Load<Texture2D>("res://raw assets/Images/ToolsButton/Detail.png");
            Details.Pressed += Details_Pressed;
            AddChild(Details);

            if(isInInventory == false)
            {
                TextureButton MoveToInventory = new TextureButton();
                MoveToInventory.Name = "Move to inventory";
                MoveToInventory.TextureNormal = ResourceLoader.Load<Texture2D>("res://raw assets/Images/ToolsButton/MoveToBag.png");
                MoveToInventory.Pressed += MoveToInventory_Pressed;
                AddChild(MoveToInventory);
            }

            TextureButtonTimeShader Sell = Scenes.Widgets.ContextMenu.TextureButtonTimeShader();
            Sell.Name = "Sell";
            Sell.TextureNormal = ResourceLoader.Load<Texture2D>("res://raw assets/Images/ToolsButton/Sell.png");
            Sell.ButtonDown += Sell_ButtonDown;
            Sell.ButtonUp += Sell_ButtonUp;
            AddChild(Sell);

            TextureButtonTimeShader Delete = Scenes.Widgets.ContextMenu.TextureButtonTimeShader();
            Delete.Name = "Delete";
            Delete.TextureNormal = ResourceLoader.Load<Texture2D>("res://raw assets/Images/ToolsButton/Deletel.png");
            Delete.ButtonDown += Delete_ButtonDown;
            Delete.ButtonUp += Delete_ButtonUp;

            AddChild(Delete);
        }

        private void InventorySlot_ItemChanged(object sender, (InventorySlot, int, bool) e)
        {
            if (e.Item3)
                playerController.Gold += e.Item1.itemDatabaseRow.SellPrice * e.Item2; 

             playerController.InventoryComponentSeeds.RemoveItem(e.Item1.itemDatabaseRow.Id, e.Item2);
        }

        #region Delete
        public void Delete_ButtonDown()
        {
            GetNode<TextureButtonTimeShader>("Delete").SetShaderMaterial(GD.Load<ShaderMaterial>("res://Shaders/Materials/ConfirmationCircleShader.tres"));
            timerConfirm.Timeout += Delete_Pressed_Confirm_Timeout;
            timerConfirm.Start();
        }
        public virtual void Delete_Pressed_Confirm_Timeout()
        {
            GetNode<TextureButtonTimeShader>("Delete").Material = null;

            if(isInventorySlot)
            {
                var amountWindow = Scenes.Widgets.Inventory.InventoryAmountWindow();
                playerController.Hud.AddChild(amountWindow);
                playerController.Hud.AddAtMousePosition(amountWindow);
                amountWindow.Init(InventorySlot, false);
            }
            else
            {
                targetItem.QueueFree();
            }

            playerController.RemoveOpenedContextMenu();
            timerConfirm.Timeout -= Delete_Pressed_Confirm_Timeout;

        }
        public void Delete_ButtonUp()
        {
            GetNode<TextureButtonTimeShader>("Delete").Material = null;
            timerConfirm.Stop();
            timerConfirm.Timeout -= Delete_Pressed_Confirm_Timeout;
        }
        #endregion
        #region Sell
        public void Sell_ButtonDown()
        {
            GetNode<TextureButtonTimeShader>("Sell").SetShaderMaterial(GD.Load<ShaderMaterial>("res://Shaders/Materials/ConfirmationCircleShader.tres"));
            timerConfirm.Timeout += Sell_Pressed_Confirm_Timeout;

            timerConfirm.Start();
        }

        public void Sell_Pressed_Confirm_Timeout()
        {
            GetNode<TextureButtonTimeShader>("Sell").Material = null;

            if (isInventorySlot)
            {
                var amountWindow = Scenes.Widgets.Inventory.InventoryAmountWindow();
                playerController.Hud.AddChild(amountWindow);
                playerController.Hud.AddAtMousePosition(amountWindow);
                amountWindow.Init(InventorySlot, true);
            }
            else
            {
                targetItem.QueueFree();
                playerController.Gold += targetItem.SellPrice;
            }

            playerController.RemoveOpenedContextMenu();         
            timerConfirm.Timeout -= Sell_Pressed_Confirm_Timeout;

        }
        public void Sell_ButtonUp()
        {
            GetNode<TextureButton>("Sell").Material = null;
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
            playerController.RemoveOpenedContextMenu();
        }
    }
}
