using System;
using Controllers;
using Godot;

namespace Widgets.ContextMenu
{
    public partial class ItemContextMenu:Control
    {
        protected HBoxContainer container;
        protected PlayerController playerController;
        Item targetItem;
        Timer timerConfirm;
        public override void _Ready()
        {
            container = GetNode<HBoxContainer>("VBoxContainer");
            timerConfirm = new Timer();
            timerConfirm.WaitTime = 0.8;
            timerConfirm.OneShot= true;
            AddChild(timerConfirm);
        }
        public virtual void Init(Item item,PlayerController playerControllerToSet, bool isInInventory)
        {
            playerController= playerControllerToSet;
            targetItem = item;

            TextureButton Details = new TextureButton();
            Details.Name = "Details";
            Details.TextureNormal = ResourceLoader.Load<Texture2D>("res://raw assets/Images/ToolsButton/Detail.png");
            Details.Pressed += Details_Pressed;
            container.AddChild(Details);

            if(isInInventory == false)
            {
                TextureButton MoveToInventory = new TextureButton();
                MoveToInventory.Name = "Move to inventory";
                MoveToInventory.TextureNormal = ResourceLoader.Load<Texture2D>("res://raw assets/Images/ToolsButton/MoveToBag.png");
                MoveToInventory.Pressed += MoveToInventory_Pressed;
                container.AddChild(MoveToInventory);
            }

            TextureButtonTimeShader Sell = Scenes.Widgets.ContextMenu.TextureButtonTimeShader();
            Sell.Name = "Sell";
            Sell.TextureNormal = ResourceLoader.Load<Texture2D>("res://raw assets/Images/ToolsButton/Sell.png");
            Sell.ButtonDown += Sell_ButtonDown;
            Sell.ButtonUp += Sell_ButtonUp;
            container.AddChild(Sell);

            TextureButtonTimeShader Delete = Scenes.Widgets.ContextMenu.TextureButtonTimeShader();
            Delete.Name = "Delete";
            Delete.TextureNormal = ResourceLoader.Load<Texture2D>("res://raw assets/Images/ToolsButton/Deletel.png");
            Delete.ButtonDown += Delete_ButtonDown;
            Delete.ButtonUp += Delete_ButtonUp;

            container.AddChild(Delete);
        }

        #region Delete
        public void Delete_ButtonDown()
        {
            GetNode<TextureButtonTimeShader>("VBoxContainer/Delete").SetShaderMaterial(GD.Load<ShaderMaterial>("res://Shaders/Materials/ConfirmationCircleShader.tres"));
            timerConfirm.Timeout += Delete_Pressed_Confirm_Timeout;
            timerConfirm.Start();
        }
        public virtual void Delete_Pressed_Confirm_Timeout()
        {
            GetNode<TextureButtonTimeShader>("VBoxContainer/Delete").Material = null;
            targetItem.QueueFree();
            playerController.RemoveOpenedContextMenu();
            timerConfirm.Timeout -= Delete_Pressed_Confirm_Timeout;

        }
        public void Delete_ButtonUp()
        {
            GetNode<TextureButtonTimeShader>("VBoxContainer/Delete").Material = null;
            timerConfirm.Stop();
            timerConfirm.Timeout -= Delete_Pressed_Confirm_Timeout;
        }
        #endregion
        #region Sell
        public void Sell_ButtonDown()
        {
            GetNode<TextureButtonTimeShader>("VBoxContainer/Sell").SetShaderMaterial(GD.Load<ShaderMaterial>("res://Shaders/Materials/ConfirmationCircleShader.tres"));
            timerConfirm.Timeout += Sell_Pressed_Confirm_Timeout;

            timerConfirm.Start();
        }

        public void Sell_Pressed_Confirm_Timeout()
        {
            GetNode<TextureButtonTimeShader>("VBoxContainer/Sell").Material = null;
            targetItem.QueueFree();
            playerController.RemoveOpenedContextMenu();
            playerController.Gold += targetItem.SellPrice;
            timerConfirm.Timeout -= Sell_Pressed_Confirm_Timeout;

        }
        public void Sell_ButtonUp()
        {
            GetNode<TextureButton>("VBoxContainer/Sell").Material = null;
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
            GD.Print("Details");
            playerController.RemoveOpenedContextMenu();
        }
    }
}
