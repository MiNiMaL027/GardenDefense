using Godot;
using System;

public partial class UpgradeValueEntity : HBoxContainer
{
    Label ValueNameLabel { get; set; }
    Label CurrentValueLabel { get; set; }
    public Label NextValueLabel { get; set; }

    public override void _Ready()
    {
        ValueNameLabel = GetNode<Label>("ValueName");
        CurrentValueLabel = GetNode<Label>("CurrentValue");
        NextValueLabel = GetNode<Label>("NextValue");
    }

    public void Init(string name, int currentValue, int nextValue)
    {
        ValueNameLabel.Text = name;
        CurrentValueLabel.Text = currentValue.ToString();
        NextValueLabel.Text = nextValue.ToString();
    }

    public void Refresh(int currentValue, int nextValue)
    {
        CurrentValueLabel.Text = currentValue.ToString();
        NextValueLabel.Text = nextValue.ToString();
    }

    public void Block()
    {
        NextValueLabel.LabelSettings.FontColor = new Color(0.651f, 0.086f, 0.059f);
    }

    public void UnBlock()
    {
        NextValueLabel.LabelSettings.FontColor = new Color(0.20f, 0.95f, 0.00f);
    }
}
