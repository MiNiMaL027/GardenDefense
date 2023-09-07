using Godot;
using System;
using Widgets.Inventory;

public partial class InventoryAmountWindow : Panel
{
	Label ValueLabel;
	Label MaxValueLabel;
	HSlider Slider;
	TextureButton AcceptButton;
	TextureButton CancelButton;
	bool Sell;
	HBoxContainer CoinsAmountContainer;
	Label CoinsAmountLabel;

	InventorySlot Slot;

	int amount;
	public int Amount
	{
		get { return amount; }
		set
		{
			amount = value;
			ValueLabel.Text = value.ToString();		
			CoinsAmountLabel.Text = (Slot.itemDatabaseRow.SellPrice * value).ToString();
		}
	}

	public override void _Ready()
	{
		ValueLabel = GetNode<Label>("VBoxContainer/HBoxContainer/Value");
		MaxValueLabel = GetNode<Label>("VBoxContainer/HBoxContainer/MaxValue");
		Slider = GetNode<HSlider>("VBoxContainer/HSlider");
		AcceptButton = GetNode<TextureButton>("VBoxContainer/HBoxContainer2/Accept");
		CancelButton = GetNode<TextureButton>("VBoxContainer/HBoxContainer2/Cancel");
		CoinsAmountContainer = GetNode<HBoxContainer>("VBoxContainer/Coins");
		CoinsAmountLabel = GetNode<Label>("VBoxContainer/Coins/CoinCount");

        Slider.ValueChanged += Slider_ValueChanged;
        AcceptButton.Pressed += AcceptButton_Pressed;
        CancelButton.Pressed += CancelButton_Pressed;
	}

    private void CancelButton_Pressed()
    {
		QueueFree();
    }

    private void AcceptButton_Pressed()
    {
		Slot.RemoveOrSell(Amount, Sell);
		QueueFree();
    }

    private void Slider_ValueChanged(double value)
    {
		Amount = (int)value;
    }

    public void Init(InventorySlot slot, bool sell)
	{
		Slot = slot;
		Sell = sell;

		if(sell)
			CoinsAmountContainer.Visible = true;
		else
			CoinsAmountContainer.Visible = false;
		
		Amount = slot.Amount;
		MaxValueLabel.Text = " / " + slot.Amount.ToString();
		Slider.MaxValue = slot.Amount;
		Slider.MinValue = 1;
		Slider.Value = slot.Amount;
		CoinsAmountLabel.Text = (slot.itemDatabaseRow.SellPrice * slot.Amount).ToString();
	}
}
