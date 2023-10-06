using Godot;
using System;

public partial class ExpandPanel : Control
{
	Button ReturnButton { get; set; }

	public override void _Ready()
	{
		ReturnButton = GetNode<Button>("ReturnButton");

        ReturnButton.Pressed += ReturnButton_Pressed;
	}

    private void ReturnButton_Pressed()
    {
        this.GetPlayerController().Hud.ShopWindow.Visible = true;

        GameInstance.World.MobilePlanforms.ToHide();

        QueueFree();
    }
}
