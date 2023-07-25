using Controllers;
using Godot;
using Widgets.GardenWidgets;

public partial class Hud : CanvasLayer
{
    public GardenWidget GardenWidget { get; set; }
	public override void _Ready()
	{
        GameInstance.Hud = this;
    }

    public GardenWidget DisplayGardenWidget(PlayerController playerController)
    {
        GardenWidget gardenWidget= Scenes.Widgets.GardenWidgets.GardenWidget();
        AddChild(gardenWidget);
        gardenWidget.Init(playerController);
        return gardenWidget;
    }
}
