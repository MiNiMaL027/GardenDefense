using BaseClasses;
using Controllers;
using Godot;
using Interfaces;
using System;

public partial class Ambar : BaseStaticBody3D, IPressable, IHoverable
{
    public void LeftMouseDownListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
    {
        if(playerController.Hud.GardenWidget.GardenInventoryWidget == null)
            playerController.Hud.GardenWidget.OpenInventory();
        else
            playerController.Hud.GardenWidget.CloseInventory();
    }

    public void LeftMouseUpListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
    {
        
    }

    public void RightMouseDownListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
    {
        
    }
}
