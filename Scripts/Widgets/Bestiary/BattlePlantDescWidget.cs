using System;
using Controllers;
using Godot;
using Items;
using Pawns;

namespace Widgets.Bestiary
{
    public partial class BattlePlantDescWidget:ItemDescWidget
    {
        public Label LabelHealth;
        public Label LabelDamage;
        public Label LabelAttackSpeed;
        public Label LabelRange;
        public TextureRect TextureRectCropIcon;
        public Label LabelCropCount;
        public override void _Ready()
        {
            TextureRect = GetNode<TextureRect>("VBoxContainer/HBoxContainer/TextureRect");
            LabelName = GetNode<Label>("VBoxContainer/HBoxContainer/LabelName");
            LabelDescription = GetNode<Label>("VBoxContainer/MarginContainer/VBoxContainer/LabelDescription");

            LabelBuyPrice = GetNode<Label>("VBoxContainer/MarginContainer/VBoxContainer/CostInfo/LabelBuyPrice");
            LabelSellPrice = GetNode<Label>("VBoxContainer/MarginContainer/VBoxContainer/CostInfo/LabelSellPrice");

            LabelHealth = GetNode<Label>("VBoxContainer/MarginContainer/VBoxContainer/HFlowContainer/HBoxContainer/LabelHealth");
            LabelDamage = GetNode<Label>("VBoxContainer/MarginContainer/VBoxContainer/HFlowContainer/HBoxContainer2/LabelDamage");
            LabelAttackSpeed = GetNode<Label>("VBoxContainer/MarginContainer/VBoxContainer/HFlowContainer/HBoxContainer3/LabelAttackSpeed");
            LabelRange = GetNode<Label>("VBoxContainer/MarginContainer/VBoxContainer/HFlowContainer/HBoxContainer4/LabelRange");

            TextureRectCropIcon = GetNode<TextureRect>("VBoxContainer/MarginContainer/VBoxContainer/CostInfo/TextureRectCropIcon");
            LabelCropCount = GetNode<Label>("VBoxContainer/MarginContainer/VBoxContainer/CostInfo/LabelCropCount");
        }
        public override void Init(object o)
        {
            if(o is BattlePlantDataBaseRow itemDatabaseRow)
            {
                LabelName.Text = itemDatabaseRow.ItemName;
                LabelDescription.Text = itemDatabaseRow.Description;
                LabelBuyPrice.Text = itemDatabaseRow.BuyPrice.ToString();
                LabelSellPrice.Text = itemDatabaseRow.SellPrice.ToString();
                TextureRect.Texture = GD.Load<Texture2D>(itemDatabaseRow.TextureSpritePath);

                PawnDatabaseRow pawnDatabaseRow = DbService.GetPawn(itemDatabaseRow.PawnId);
                Pawn pawn = GD.Load<PackedScene>(pawnDatabaseRow.ScenePath).Instantiate<Pawn>();
                LabelRange.Text = pawn.PawnStats.AttackRange.ToString();
                LabelHealth.Text = pawn.PawnStats.MaxHealth.ToString();
                LabelDamage.Text = pawn.PawnStats.Strength.ToString();
                LabelAttackSpeed.Text = pawn.PawnStats.AttackSpeed.ToString();
                pawn.QueueFree();
                ItemDatabaseRow cropToBuy = DbService.GetItem(itemDatabaseRow.BuyCropId);
                TextureRectCropIcon.TooltipText = cropToBuy.ItemName;
                TextureRectCropIcon.Texture = GD.Load<Texture2D>(cropToBuy.TextureSpritePath);
                LabelCropCount.Text = itemDatabaseRow.BuyCropCount.ToString();


            }
        }
    }
}
