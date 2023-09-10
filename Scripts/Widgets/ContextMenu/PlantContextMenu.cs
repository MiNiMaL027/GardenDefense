using Controllers;
using Godot;
using System;
using Widgets.ContextMenu;

public partial class PlantContextMenu : ItemContextMenu
{
	GrowingPlant Plant;

	public override void _Ready()
	{
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
        AddButton(new TextureButton(), "Details", "res://raw assets/Images/ToolsButton/Detail.png", Details_Pressed, null);
	}

    public override void Delete_Pressed_Confirm_Timeout()
    {
        GetNode<TextureButtonTimeShader>("Delete").Material = null;

		Plant.QueueFree();
		Plant.PlantSocket.IsUsed = false;

		playerController.RemoveOpenedContextMenu();
		timerConfirm.Timeout -= Delete_Pressed_Confirm_Timeout;
    }
}
