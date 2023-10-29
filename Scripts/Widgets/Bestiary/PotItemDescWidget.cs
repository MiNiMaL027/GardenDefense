using Godot;
using Items;
using System;
namespace Widgets.Bestiary
{
    public partial class PotItemDescWidget : ItemDescWidget
    {
        Label LabelSmallSockets;
        Label LabelBigSockets;
        Label LabelWateredTime;


        public override void _Ready()
        {
            LabelSmallSockets = GetNode<Label>("VBoxContainer/MarginContainer/VBoxContainer/LabelSmallSockets");
            LabelBigSockets = GetNode<Label>("VBoxContainer/MarginContainer/VBoxContainer/LabelBigSockets");
            LabelWateredTime = GetNode<Label>("VBoxContainer/MarginContainer/VBoxContainer/LabelWateredTime");

            TextureRect = GetNode<TextureRect>("VBoxContainer/HBoxContainer/TextureRect");
            LabelName = GetNode<Label>("VBoxContainer/HBoxContainer/LabelName");
            LabelDescription = GetNode<Label>("VBoxContainer/MarginContainer/VBoxContainer/LabelDescription");

            LabelBuyPrice = GetNode<Label>("VBoxContainer/MarginContainer/VBoxContainer/CostInfo/LabelBuyPrice");
            LabelSellPrice = GetNode<Label>("VBoxContainer/MarginContainer/VBoxContainer/CostInfo/LabelSellPrice");
        }
        public override void Init(object o)
        {
            if (o is PotDatabaseRow itemDatabaseRow)
            {
                LabelSmallSockets.Text = $"Small sockets: {itemDatabaseRow.SmallPotsAmount}";
                LabelBigSockets.Text = $"Big sockets: {itemDatabaseRow.BigPotsAmount}";
                LabelWateredTime.Text = $"Watered time: {itemDatabaseRow.WaterTime} seconds";


                LabelName.Text = itemDatabaseRow.ItemName;
                LabelDescription.Text = itemDatabaseRow.Description;
                LabelBuyPrice.Text = itemDatabaseRow.BuyPrice.ToString();
                LabelSellPrice.Text = itemDatabaseRow.SellPrice.ToString();
                TextureRect.Texture = GD.Load<Texture2D>(itemDatabaseRow.TextureSpritePath);
            }
        }
    }
}
