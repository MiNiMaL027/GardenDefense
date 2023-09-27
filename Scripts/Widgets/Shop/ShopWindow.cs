using Enums;
using Farm.Scripts.Widgets.Shop.Upgrade;
using Godot;

public partial class ShopWindow : Control
{
	private TextureButton CloseButton;
    public VBoxContainer ItemContainer { get; set; }
    public HBoxContainer CategoriesContainer { get; set; }
    private Button SellButton { get; set; }
    private Button UpgradeButton { get; set; }
    private Label CoinsCount { get; set; }

    private UpgradeService UpgradeService;

    public override void _Ready()
	{     
		CloseButton = GetNode<TextureButton>("PanelContainer/HBoxContainer/Space/TextureButton");
        ItemContainer = GetNode<VBoxContainer>("PanelContainer/HBoxContainer/ShopPanel/ScrollContainer/ItemContainer");
        CategoriesContainer = GetNode<HBoxContainer>("PanelContainer/HBoxContainer/ShopPanel/Categories");
        CoinsCount = GetNode<Label>("PanelContainer/HBoxContainer/ShopPanel/HBoxContainer/CoinsCount");
        SellButton = GetNode<Button>("PanelContainer/HBoxContainer/ShopPanel/HBoxContainer/SellButton");
        UpgradeButton = GetNode<Button>("PanelContainer/HBoxContainer/ShopPanel/HBoxContainer/UpgradeButton");

        CloseButton.Pressed += CloseButton_Pressed;
        SellButton.Pressed += SellButton_Pressed;
        UpgradeButton.Pressed += UpgradeButton_Pressed;

        Init();
	}

    private void UpgradeButton_Pressed()
    {
        Clear();

        UpgradeService = new UpgradeService();
        UpgradeService.Refresh += RefreshCoinsCount;
        UpgradeService.Init(ItemContainer);
    }

    private void SellButton_Pressed()
    {
        this.GetPlayerController().Hud.OpenSellWindow();
    }

    private void CloseButton_Pressed()
    {
        this.GetPlayerController().Hud.CloseShop();
    }

    private void OpenCategorie(ItemType type)
    {
        Clear();

        var avaliableShopItems = this.GetPlayerController().avaliableShopItems[type];

        for (int i = 0; i < avaliableShopItems.Count; i++)
        {
            var slot = Scenes.Widgets.Shop.ShopSlot();

            ItemContainer.AddChild(slot);
            slot.Init(avaliableShopItems[i]);

            slot.BuyButtonClicked += RefreshCoinsCount;
        }     
    }

    private void Clear()
    {
        if(ItemContainer.GetChildCount() > 0)
        {
            for (int i = 0; i < ItemContainer.GetChildCount(); i++)
            {
                ItemContainer.RemoveChild(ItemContainer.GetChild(i));
            }
        }
    }

    public void Init()
    {
        var shopitems = this.GetPlayerController().avaliableShopItems;

        if (CategoriesContainer.GetChildCount() > 0)
            for (int i = 0; i < CategoriesContainer.GetChildCount(); i++)
            {
                CategoriesContainer.RemoveChild(CategoriesContainer.GetChild<Button>(i));
            }

        foreach (ItemType type in shopitems.Keys)
        {
            var newButton = new CategoriesButton() { ToggleMode = true, Text = $"{type}", ButtonGroup = ResourceLoader.Load<ButtonGroup>("res://Scenes/Widgets/Shop/ShopButtons.tres") };

            newButton.ButtonClicked += NewButton_ButtonClicked;
            newButton.ItemType = type;

            CategoriesContainer.AddChild(newButton);
        }

        RefreshCoinsCount();
    }

    private void NewButton_ButtonClicked(object sender, ButtonEventData e)
    {
        OpenCategorie(e.ItemType);
    }

    public void RefreshCoinsCount()
    {
        CoinsCount.Text = $"{this.GetPlayerController().Gold}";
    }
}
