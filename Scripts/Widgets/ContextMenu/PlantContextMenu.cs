using Controllers;
using Godot;
using System;

namespace Widgets.ContextMenu
{
    public partial class PlantContextMenu : ItemContextMenu
    {
        GrowingPlant Plant;

        public override void _Ready()
        {
            base._Ready();

            timerConfirm = new Timer();
            timerConfirm.WaitTime = 0.8;
            timerConfirm.OneShot = true;

            AddChild(timerConfirm);
        }

        public void Init(GrowingPlant plant, PlayerController playerController)
        {
            Plant = plant;

            this.playerController = playerController;

            AddButton(Scenes.Widgets.ContextMenu.TextureButtonTimeShader(), "Delete", "res://raw assets/Images/ToolsButton/Deletel.png", Delete_ButtonDown, Delete_ButtonUp);
            AddButton(new TextureButton(), "Details", "res://raw assets/Images/ToolsButton/Detail.png", Details_Pressed);
        }

        public override void Delete_Pressed_Confirm_Timeout()
        {
            Container.GetNode<TextureButtonTimeShader>("Delete").Material = null;

            Plant.QueueFree();
            Plant.PlantSocket.IsUsed = false;

            playerController.RemoveOpenedContextMenu();
            timerConfirm.Timeout -= Delete_Pressed_Confirm_Timeout;
        }

        public override void Details_Pressed()
        {
            playerController.Hud.OpenBestiary();

            if (!playerController.Hud.BestiaryWindow.OpenExactItem(Enums.ItemType.Harvestable, Plant.SeedData.Id))
            {
                playerController.Hud.CloseBestiary();
                playerController.Hud.GardenWidget.InfoWindow.AddInfoPanel($"Right Now you do not know enough about {Plant.SeedData.ItemName}. If you want to know more you can use it more");
            }

            playerController.RemoveOpenedContextMenu();
        }
    }
}

