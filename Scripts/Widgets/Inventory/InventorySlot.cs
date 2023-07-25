using Godot;
using Items;
using System;

namespace Widgets.Inventory
{
    public partial class InventorySlot : Panel
    {
        public int ItemId { get; set; }
        public int Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                if (amount > 1) //display count of items
                {
                    LabelAmount.Text = amount.ToString();
                }
                else if (amount == 1) //text shouldn't be displayed
                {
                    LabelAmount.Text = "";
                }
                else //remove from screen if amount 0
                {
                    QueueFree();
                }
            }
        }
        private int amount;
        public TextureRect TextureRect { get; set; }
        public Label LabelAmount { get; set; }

        public void Init(int itemId, int amountToSet)
        {
            ItemDatabaseRow databaseRow = DbService.GetItem(itemId);
            TextureRect.Texture = GD.Load<Texture2D>(databaseRow.TextureSpritePath);
            ItemId = itemId;
            amount = amountToSet;
            if (amount > 1)
            {
                LabelAmount.Text = amount.ToString();
            }
            else
            {
                LabelAmount.Text = "";

            }
        }
        public override void _Ready()
        {
            TextureRect = GetNode<TextureRect>("TextureRect");
            LabelAmount = GetNode<Label>("LabelAmount");
        }
    }
}
