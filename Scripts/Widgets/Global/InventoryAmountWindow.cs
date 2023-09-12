using Godot;
using System;
using Widgets.Inventory;

public partial class InventoryAmountWindow : Panel
{
	Label ValueLabel;
	Label MaxValueLabel;
	HSlider Slider;
	Button AcceptButton;
	Button CancelButton;
	bool Sell;
	HBoxContainer CoinsAmountContainer;
	Label CoinsAmountLabel;

	int sellPrice;

	[Signal]
	public delegate void ButtonPressedAcceptEventHandler(int amount, bool sell);

	int amount;
	public int Amount
	{
		get { return amount; }
		set
		{
			amount = value;
			ValueLabel.Text = value.ToString();		
			CoinsAmountLabel.Text = (sellPrice * value).ToString();
		}
	}

	public override void _Ready()
	{
		ValueLabel = GetNode<Label>("Panel/VBoxContainer/HBoxContainer/Value");
		MaxValueLabel = GetNode<Label>("Panel/VBoxContainer/HBoxContainer/MaxValue");
		Slider = GetNode<HSlider>("Panel/VBoxContainer/HSlider");
		AcceptButton = GetNode<Button>("Panel/VBoxContainer/HBoxContainer2/Accept");
		CancelButton = GetNode<Button>("Panel/VBoxContainer/HBoxContainer2/Cancel");
		CoinsAmountContainer = GetNode<HBoxContainer>("Panel/VBoxContainer/Coins");
		CoinsAmountLabel = GetNode<Label>("Panel/VBoxContainer/Coins/CoinCount");

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
		EmitSignal(SignalName.ButtonPressedAccept, Amount, Sell);
		QueueFree();
    }

    private void Slider_ValueChanged(double value)
    {
		Amount = (int)value;
    }

    public void Init(int amount, int sellPrice , bool sell)
	{
		Sell = sell;

		if(sell)
			CoinsAmountContainer.Visible = true;
		else
			CoinsAmountContainer.Visible = false;

		this.sellPrice = sellPrice;
		Amount = amount;
		MaxValueLabel.Text = " / " + amount.ToString();
		Slider.MaxValue = amount;
		Slider.MinValue = 1;
		Slider.Value = amount;
		CoinsAmountLabel.Text = (sellPrice * amount).ToString();
	}

    public override void _GuiInput(InputEvent @event)
    {
		base._GuiInput(@event);

		if(@event is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == MouseButton.Left && mouseButton.Pressed || @event is InputEventMouseButton mouseButtonLeft && mouseButtonLeft.ButtonIndex == MouseButton.Right && mouseButtonLeft.Pressed)
		{
			QueueFree();
		}
    }
}
