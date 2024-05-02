using Controllers;
using Godot;
using Widgets.Bestiary;
using Widgets.GardenWidgets;
using Widgets.Global;
using Widgets.Shop;

public partial class Hud : CanvasLayer
{
    public GardenWidget GardenWidget { get; set; }
    public BattlefieldWidget BattlefieldWidget { get; set; }
    public MainWidget MainWidget { get; set; }

    public BestiaryWindow BestiaryWindow { get; set; }
    public ShopWindow ShopWindow { get; set; }
    public SellWindow SellWindow { get; set; }
    public LaboratoryWindow LaboratoryWindow { get; set; }

    

    public override void _Ready()
	{
        GameInstance.Hud = this;
    }

    public void DisplayGardenWidget(PlayerController playerController)
    {
        if (GodotObject.IsInstanceValid(BattlefieldWidget))
        {
            BattlefieldWidget.QueueFree();
            BattlefieldWidget = null;
        }
        if(GardenWidget != null)
        {
            AddChild(GardenWidget);
            MainWidget = GardenWidget;
            return;
        }
        GardenWidget gardenWidget = Scenes.Widgets.GardenWidgets.GardenWidget();

        AddChild(gardenWidget);

        GardenWidget = gardenWidget;
        MainWidget = GardenWidget;

    }
    public void DisplayBattlefieldWidget(PlayerController playerController)
    {
        if (GodotObject.IsInstanceValid(GardenWidget))
        {
            GardenWidget.RemoveFromParent();
        }
        BattlefieldWidget bw = Scenes.Widgets.GardenWidgets.BattlefieldWidget();

        AddChild(bw);

        BattlefieldWidget = bw;
        MainWidget = BattlefieldWidget;

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

        BestiaryWindow.OpenLastItem();

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
        SellWindow = Scenes.Widgets.Shop.SellWindow();

        AddChild(SellWindow);
    }

    public void OpenExpandPanel()
    {
        AddChild(Scenes.Widgets.Shop.ExpandPanel());
    }

    public void CloseSellWindow()
    {
        SellWindow.QueueFree();     
        
        SellWindow = null;
    }

    public void OpenLaboratory()
    {
        LaboratoryWindow = Scenes.Widgets.Laboratory.LaboratoryWindow();

        ToLowMusic(true);

        AddChild(LaboratoryWindow);
    }

    public void CloseLaboratory()
    {
        LaboratoryWindow.QueueFree();

        ToLowMusic(false);

        LaboratoryWindow = null;
    }

    private void ToLowMusic(bool isLowMusic = true)
    {
        if (isLowMusic)
        {
            GameInstance.World.AddEffect(true);
        }
        else if (!isLowMusic && SellWindow == null && BestiaryWindow == null && ShopWindow == null)
        {
            GameInstance.World.AddEffect(false);
        }
    }

    public void Pause()
    {
        var pauseMenu = Scenes.Widgets.PausePanel();
        AddChild(pauseMenu);
        ToLowMusic(true);
        
        GetTree().Paused = true;
    }   
}
