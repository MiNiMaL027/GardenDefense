using Controllers;
using Godot;
using Interfaces;
using System;

public partial class Shop : StaticBody3D, IPressable
{
    public void LeftMouseDownListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
    {
        this.GetPlayerController().Hud.OpenShop();
    }

    public void LeftMouseUpListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
    {
        
    }

    public void RightMouseDownListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
    {
        this.GetPlayerController().Hud.OpenShop();
    }
}
