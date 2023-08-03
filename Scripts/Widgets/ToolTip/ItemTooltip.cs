using Godot;
using System;

public partial class ItemTooltip : BaseTooltip
{
    Label LabelItemName;
    public override void _Ready()
    {
        LabelItemName = GetNode<Label>("PanelContainer/LabelItemName");
    }
    public override void HideTooltip()
    {
        this.QueueFree();
    }

    public override void ShowTooltip(Node n)
    {
        Item item = n as Item;
        LabelItemName.Text = item.ItemName;
    }
}
