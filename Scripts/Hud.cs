using Controllers;
using Godot;
using Widgets.Bestiary;
using Widgets.GardenWidgets;

public partial class Hud : CanvasLayer
{
    public GardenWidget GardenWidget { get; set; }
    public BestiaryWindow BestiaryWindow { get; set; }
	public override void _Ready()
	{
        GameInstance.Hud = this;
    }

    public void DisplayGardenWidget(PlayerController playerController)
    {
        GardenWidget gardenWidget= Scenes.Widgets.GardenWidgets.GardenWidget();
        AddChild(gardenWidget);
        gardenWidget.Init(playerController);
        GardenWidget = gardenWidget;
    }
    public void AddAtMousePosition(Control widget)
    {
        Vector2 mousePos = GetViewport().GetMousePosition();
        widget.AdjustControlInViewport(mousePos);
    }
    public WindowConfirmation DisplayWindowConfirmation(string initText)
    {
        WindowConfirmation windowConfirmation = Scenes.Widgets.WindowConfirmation();
        AddChild(windowConfirmation);
        windowConfirmation.Init(initText);
        return windowConfirmation;
    }
    public void OpenBestiary()
    {
        BestiaryWindow = Scenes.Widgets.Bestiary.BestiaryWindow();
        AddChild(BestiaryWindow);
    }
    public void CloseBestiary()
    {
        BestiaryWindow.QueueFree();
        BestiaryWindow = null;
    }
}
