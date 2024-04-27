using Widgets.Shop.Upgrade;
using Godot;
using Items;
using System;

namespace Widgets.Shop
{
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

        public void Init(int id)
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

            AmountLine.ValueChanged += AmountLine_ValueChanged;

            itemInfoContainer = GetNode<VBoxContainer>("Button/HBoxContainer/VBoxContainer");
            buyButtonContainer = GetNode<VBoxContainer>("Button/HBoxContainer/VBoxContainer2");
            descPanel = GetNode<Panel>("Button/Panel");

            BuyButton.Pressed += BuyButton_Pressed;
            DescButton.Pressed += DescButton_Pressed;
            CloseDescButton.Pressed += CloseDescButton_Pressed;

            Refresh();
            RefreshBuyPrice();
        }

        private void AmountLine_ValueChanged(double value)
        {
            ItemBuyPrice.Text = (ItemDatabaseRow.BuyPrice * value).ToString();
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
            int amount = (int)AmountLine.Value;

            if (amount <= 0)
                amount = 1;

            var controller = this.GetPlayerController();

            if (controller.Gold >= ItemDatabaseRow.BuyPrice * amount)
            {
                controller.Gold -= ItemDatabaseRow.BuyPrice * amount;
                controller.MainInventory.AddItem(ItemDatabaseRow.Id, amount);

                RefreshBuyPrice();
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

        public void RefreshBuyPrice()
        {
            if (ItemDatabaseRow.BuyPrice < this.GetPlayerController().Gold)
            {
                ItemBuyPrice.Text = (ItemDatabaseRow.BuyPrice * AmountLine.Value).ToString();
                ItemBuyPrice.LabelSettings.FontColor = new Color(0.086f, 0.424f, 0.086f);
            }
            else
            {
                ItemBuyPrice.Text = ItemDatabaseRow.BuyPrice.ToString();
                ItemBuyPrice.LabelSettings.FontColor = new Color(0.651f, 0.086f, 0.059f);
                AmountLine.Value = 1;
            }

            if (this.GetPlayerController().Gold <= 0)
                AmountLine.MaxValue = 1;
            else
                AmountLine.MaxValue = this.GetPlayerController().Gold / ItemDatabaseRow.BuyPrice;
        }
    }

}
