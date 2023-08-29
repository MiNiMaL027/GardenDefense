using Farm.Scripts.Widgets.ToolTip;
using Godot;
using Items;

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
}
