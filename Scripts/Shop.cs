using Controllers;
using Godot;
using Interfaces;

public partial class Shop : StaticBody3D, IPressable
{
    public void LeftMouseDownListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
    {
        playerController.Hud.OpenShop();
    }

    public void LeftMouseUpListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
    {
        
    }

    public void RightMouseDownListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
    {
        playerController.Hud.OpenShop();
    }
}
