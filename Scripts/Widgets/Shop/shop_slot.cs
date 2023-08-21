using Godot;
using Items;
using System;

public partial class shop_slot : Control
{
    public TextureRect Icon { get; set; }
    public Label ItemName { get; set; }
    public Label ItemDesc { get; set; }
    public Label ItemBuyPrice { get; set; }
    public ItemDatabaseRow ItemDatabaseRow { get; set; }

    public Button BuyButton { get; set; }
    public event Action BuyButtonClicked;

    public void Init (int id)
    {
        ItemDatabaseRow = DbService.GetItem(id);
        Icon = GetNode<TextureRect>("Button/HBoxContainer/TextureRect");
        ItemName = GetNode<Label>("Button/HBoxContainer/VBoxContainer/Name");
        ItemDesc = GetNode<Label>("Button/HBoxContainer/VBoxContainer/Desc");
        ItemBuyPrice = GetNode<Label>("Button/HBoxContainer/VBoxContainer2/Button/buyPrice");
        BuyButton = GetNode<Button>("Button/HBoxContainer/VBoxContainer2/Button");

        BuyButton.Pressed += BuyButton_Pressed;

        Refresh();
    }

    private void BuyButton_Pressed()
    {
        GD.Print("Buy");
        var controller = this.GetPlayerController();

        if(controller.Gold >= ItemDatabaseRow.BuyPrice)
        {
            controller.Gold -= ItemDatabaseRow.BuyPrice;
            controller.InventoryComponentSeeds.AddItem(ItemDatabaseRow.Id);
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
