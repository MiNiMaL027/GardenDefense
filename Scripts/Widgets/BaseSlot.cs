using Godot;
using Items;
using Widgets.Inventory;

namespace Farm.Scripts.Widgets
{
    public abstract partial class BaseSlot : Control
    {
        public ItemDatabaseRow ItemDatabaseRow { get; set; }

        public Label LabelAmount { get; set; }

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

        protected int amount;
    }
}
