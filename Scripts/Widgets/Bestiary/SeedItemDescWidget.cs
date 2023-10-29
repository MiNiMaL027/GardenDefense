using System;
using Godot;
using Items;

namespace Widgets.Bestiary
{
    public partial class SeedItemDescWidget:ItemDescWidget
    {
        public Label LabelSeedType;
        public Label LabelStagesAmount;
        public Label LabelTimeToChangeState;
        public Label LabelCropAmount;
        public override void _Ready()
        {
            TextureRect = GetNode<TextureRect>("VBoxContainer/HBoxContainer/TextureRect");
            LabelName = GetNode<Label>("VBoxContainer/HBoxContainer/LabelName");
            LabelDescription = GetNode<Label>("VBoxContainer/MarginContainer/VBoxContainer/LabelDescription");

            LabelBuyPrice = GetNode<Label>("VBoxContainer/MarginContainer/VBoxContainer/CostInfo/LabelBuyPrice");
            LabelSellPrice = GetNode<Label>("VBoxContainer/MarginContainer/VBoxContainer/CostInfo/LabelSellPrice");

            LabelSeedType = GetNode<Label>("VBoxContainer/MarginContainer/VBoxContainer/LabelSeedType");
            LabelStagesAmount = GetNode<Label>("VBoxContainer/MarginContainer/VBoxContainer/LabelStagesAmount");
            LabelTimeToChangeState = GetNode<Label>("VBoxContainer/MarginContainer/VBoxContainer/LabelTimeToChangeStage");
            LabelCropAmount = GetNode<Label>("VBoxContainer/MarginContainer/VBoxContainer/LabelCropAmount");
        }
        public override void Init(object o)
        {
            if(o is SeedDatabaseRow itemDatabaseRow)
            {
                LabelSeedType.Text = $"Seed size: {itemDatabaseRow.SeedType}";
                LabelStagesAmount.Text = $"Stages to grow: {itemDatabaseRow.StagesAmount}";
                LabelTimeToChangeState.Text = $"Stage time: from {itemDatabaseRow.MinSecondsToChangeState} s. to {itemDatabaseRow.MaxSecondsToChangeState} s.";
                if(itemDatabaseRow.MaxCropAmount == itemDatabaseRow.MinCropAmount)
                {
                    LabelCropAmount.Text = $"Crop: {itemDatabaseRow.MaxCropAmount}";

                }
                else
                {
                    LabelCropAmount.Text = $"Crop: {itemDatabaseRow.MinCropAmount} - {itemDatabaseRow.MaxCropAmount}";
                }


                LabelName.Text = itemDatabaseRow.ItemName;
                LabelDescription.Text = itemDatabaseRow.Description;
                LabelBuyPrice.Text = itemDatabaseRow.BuyPrice.ToString();
                LabelSellPrice.Text = itemDatabaseRow.SellPrice.ToString();
                TextureRect.Texture = GD.Load<Texture2D>(itemDatabaseRow.TextureSpritePath);
            }
        }
    }
}
