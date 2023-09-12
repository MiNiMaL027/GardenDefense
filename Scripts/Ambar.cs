using Controllers;
using Godot;
using Interfaces;
using System;

public partial class Ambar : StaticBody3D, IPressable
{
    public void LeftMouseDownListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
    {
        if(playerController.Hud.GardenWidget.InventoryWidget == null)
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
