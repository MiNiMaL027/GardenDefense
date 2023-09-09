using Controllers;
using Godot;
using Widgets.Bestiary;
using Widgets.GardenWidgets;

public partial class Hud : CanvasLayer
{
    public GardenWidget GardenWidget { get; set; }
    public BestiaryWindow BestiaryWindow { get; set; }
    public ShopWindow ShopWindow { get; set; }
    public SellWindow SellWindow { get; set; }
	public override void _Ready()
	{
        GameInstance.Hud = this;
    }

    public void DisplayGardenWidget(PlayerController playerController)
    {
        GardenWidget gardenWidget= Scenes.Widgets.GardenWidgets.GardenWidget();
        AddChild(gardenWidget);
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

        ToLowMusic();
    }
    public void CloseBestiary()
    {
        BestiaryWindow.QueueFree();

        BestiaryWindow = null;

        ToLowMusic(false);
    }

    public void OpenShop()
    {
        ShopWindow = Scenes.Widgets.Shop.ShopWindow();

        AddChild(ShopWindow);

        ToLowMusic();
    }

    public void CloseShop()
    {
        ShopWindow.QueueFree();

        ShopWindow = null;

        ToLowMusic(false);
    }
    
    public void OpenSellWindow()
    {
        CloseShop();
        SellWindow = Scenes.Widgets.Shop.SellWindow();

        AddChild(SellWindow);

        ToLowMusic();
    }

    public void CloseSellWindow()
    {
        OpenShop();
        SellWindow.QueueFree();      
        SellWindow = null;
        ToLowMusic(false);
    }

    public void CloseAllWidgets()
    {
        if (BestiaryWindow != null)
            CloseBestiary();
        else if(SellWindow != null)
            CloseSellWindow();
        else if (ShopWindow != null)
            CloseShop();      
    }

    private void ToLowMusic(bool isLowMusic = true)
    {
        if(isLowMusic)
        {
            GameInstance.World.ChangeBus(1);
        }
        else if(!isLowMusic && SellWindow == null && BestiaryWindow == null && ShopWindow == null)
        {
            GameInstance.World.ChangeBus(0);
        }
    }
}
