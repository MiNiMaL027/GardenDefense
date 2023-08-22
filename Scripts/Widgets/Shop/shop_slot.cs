using Godot;
using Items;
using System;

public partial class shop_slot : Control
{
    public TextureRect Icon { get; set; }
    public Label ItemName { get; set; }
    public Label ItemDesc { get; set; }
    public Label ItemBuyPrice { get; set; }

    public SpinBox AmountLine { get; set; }
    public ItemDatabaseRow ItemDatabaseRow { get; set; }

    private VBoxContainer itemInfoContainer;
    private VBoxContainer buyButtonContainer;
    private Panel descPanel;

    public Button BuyButton { get; set; }
    public event Action BuyButtonClicked;

    public Button DescButton { get; set; }

    public TextureButton CloseDescButton { get; set; }

    public void Init (int id)
    {
        ItemDatabaseRow = DbService.GetItem(id);
        Icon = GetNode<TextureRect>("Button/HBoxContainer/TextureRect");
        ItemName = GetNode<Label>("Button/HBoxContainer/VBoxContainer/Name");
        ItemDesc = GetNode<Label>("Button/Panel/HBoxContainer/Desc");
        ItemBuyPrice = GetNode<Label>("Button/HBoxContainer/VBoxContainer2/Button/buyPrice");
        BuyButton = GetNode<Button>("Button/HBoxContainer/VBoxContainer2/Button");
        DescButton = GetNode<Button>("Button");
        CloseDescButton = GetNode<TextureButton>("Button/Panel/HBoxContainer/Space2/CloseDesc");
        AmountLine = GetNode<SpinBox>("Button/HBoxContainer/VBoxContainer/SpinBox");

        itemInfoContainer = GetNode<VBoxContainer>("Button/HBoxContainer/VBoxContainer");
        buyButtonContainer = GetNode<VBoxContainer>("Button/HBoxContainer/VBoxContainer2");
        descPanel = GetNode<Panel>("Button/Panel");

        BuyButton.Pressed += BuyButton_Pressed;
        DescButton.Pressed += DescButton_Pressed;
        CloseDescButton.Pressed += CloseDescButton_Pressed;

        Refresh();
    }

    private void CloseDescButton_Pressed()
    {
        itemInfoContainer.Visible = true;
        buyButtonContainer.Visible = true;

        descPanel.Visible = false;
        DescButton.Flat = false;
    }

    private void DescButton_Pressed()
    {
        itemInfoContainer.Visible = false;
        buyButtonContainer.Visible = false;

        descPanel.Visible = true;
        DescButton.Flat = false;
    }

    private void BuyButton_Pressed()
    {
        GD.Print("Buy");
        int amount = (int)AmountLine.Value;

        if (amount == null || amount == 0)
            amount = 1;

        var controller = this.GetPlayerController();

        if(controller.Gold >= ItemDatabaseRow.BuyPrice * amount)
        {
            controller.Gold -= ItemDatabaseRow.BuyPrice * amount;
            controller.InventoryComponentSeeds.AddItem(ItemDatabaseRow.Id,amount);
        }

        BuyButtonClicked?.Invoke();
    }

    public void Refresh()
    {
        Icon.Texture = ResourceLoader.Load<Texture2D>(ItemDatabaseRow.TextureSpritePath);
        ItemName.Text = ItemDatabaseRow.ItemName;
        ItemDesc.Text = ItemDatabaseRow.Description;
        ItemBuyPrice.Text = $"{ItemDatabaseRow.BuyPrice}";
    }

    
}
