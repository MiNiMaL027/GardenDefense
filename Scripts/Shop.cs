using Controllers;
using Farm.Scripts;
using Godot;
using Interfaces;

public partial class Shop : BaseStaticBody3D, IPressable, IHoverable
{
    public void LeftMouseDownListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
    {
        if(playerController.Hud.ShopWindow == null)
            playerController.Hud.OpenShop();
    }

    public void LeftMouseUpListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
    {
        
    }

    public void RightMouseDownListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
    {
        if (playerController.Hud.ShopWindow == null)
            playerController.Hud.OpenShop();
    }
}
