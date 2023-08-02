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
        this.Hide();
    }

    public override void ShowTooltip(Node n)
    {
        Item item = n as Item;
        if (item.Amount > 1)
        {
            LabelItemName.Text = $"{item.ItemName} ({item.Amount})";
        }
        else
        {
            LabelItemName.Text = item.ItemName;
        }
        this.Show();
    }
}
