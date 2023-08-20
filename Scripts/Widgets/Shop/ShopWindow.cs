using Godot;
using System;

public partial class ShopWindow : Control
{
	TextureButton CloseButton;
	public override void _Ready()
	{
		CloseButton = GetNode<TextureButton>("PanelContainer/HBoxContainer/Space/TextureButton");
        CloseButton.Pressed += CloseButton_Pressed;
	}

    private void CloseButton_Pressed()
    {
        this.GetPlayerController().Hud.CloseShop();
    }
}
