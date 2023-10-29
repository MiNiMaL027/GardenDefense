using Godot;
using Items;
using System;
namespace Widgets.Bestiary
{
    public partial class ItemDescWidget : DescWidget
    {
        public Label LabelBuyPrice;
        public Label LabelSellPrice;

        public override void _Ready()
        {
            TextureRect = GetNode<TextureRect>("VBoxContainer/HBoxContainer/TextureRect");
            LabelName = GetNode<Label>("VBoxContainer/HBoxContainer/LabelName");
            LabelDescription = GetNode<Label>("VBoxContainer/MarginContainer/VBoxContainer/LabelDescription");

            LabelBuyPrice = GetNode<Label>("VBoxContainer/MarginContainer/VBoxContainer/CostInfo/LabelBuyPrice");
            LabelSellPrice = GetNode<Label>("VBoxContainer/MarginContainer/VBoxContainer/CostInfo/LabelSellPrice");
        }

        public override void Init(object o)
        {
            if(o is ItemDatabaseRow itemDatabaseRow)
            {
                LabelName.Text = itemDatabaseRow.ItemName;
                LabelDescription.Text = itemDatabaseRow.Description;
                LabelBuyPrice.Text = itemDatabaseRow.BuyPrice.ToString();
                LabelSellPrice.Text = itemDatabaseRow.SellPrice.ToString();
                TextureRect.Texture = GD.Load<Texture2D>(itemDatabaseRow.TextureSpritePath);
            }
            else if (o is Item item)
            {
                LabelName.Text = item.ItemName;
                LabelDescription.Text = item.Description;
                LabelBuyPrice.Text = item.BuyPrice.ToString();
                LabelSellPrice.Text = item.SellPrice.ToString();
                TextureRect.Texture = GD.Load<Texture2D>(item.TextureSpritePath);
            }
        }
    }

}
