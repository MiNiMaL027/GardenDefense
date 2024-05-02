using BaseClasses;
using Controllers;
using Godot;
using Interfaces;
using System;

public partial class Laboratory : BaseStaticBody3D, IPressable, IHoverable
{
    public void LeftMouseDownListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
    {
        if(playerController.Hud.LaboratoryWindow == null)
            playerController.Hud.OpenLaboratory();
    }

    public void LeftMouseUpListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
    {
        
    }

    public void RightMouseDownListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
    {
        if (playerController.Hud.LaboratoryWindow == null)
            playerController.Hud.OpenLaboratory();
    }
}
