using Godot;
using Items;
using System;

public partial class ItemTooltip : Control
{
    Label LabelItemName;
    public override void _Ready()
    {
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
