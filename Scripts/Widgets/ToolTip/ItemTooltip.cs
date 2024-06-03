using Godot;
using Items;
namespace Widgets.ToolTip
{
    public partial class ItemTooltip : BaseTooltip
    {
        Label LabelItemName;
        public override void _Ready()
        {
            base._Ready();

            LabelItemName = GetNode<Label>("LabelItemName");
        }
        public virtual void ShowTooltipDbRow(ItemDatabaseRow itemDatabaseRow)
        {
            LabelItemName.Text = itemDatabaseRow.ItemName;
        }
        public virtual void ShowTooltip(Item item)
        {
            LabelItemName.Text = item.ItemName;
        }
        public virtual void ShowTooltip(string itemName)
        {
            LabelItemName.Text = itemName;
        }
    }

}
