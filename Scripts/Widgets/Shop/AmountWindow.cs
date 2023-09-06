using Godot;
using System;

public partial class AmountWindow : Panel
{
	private int _amount;
	public int Amount {
		get { return _amount; }
		set 
		{
			_amount = value;
			AmountLabel.Text = value.ToString();
			CoinValueLabel.Text = (value * slot.ItemDatabaseRow.SellPrice).ToString();
		}
	}
	private Label MaxAmountLabel { get; set; }
	private Label AmountLabel { get; set; }
	private Label CoinValueLabel { get; set; }
	private HSlider Slider { get; set; }
	private Button AcceptButton { get; set; }
	private Button CancelButton { get; set; }


	private sell_slot slot;

	public override void _Ready()
	{
		MaxAmountLabel = GetNode<Label>("VBoxContainer/HBoxContainer2/MaxAmount");
		AmountLabel = GetNode<Label>("VBoxContainer/HBoxContainer2/ValueAmount");
		CoinValueLabel = GetNode<Label>("VBoxContainer/HBoxContainer3/CoinValue");
		Slider = GetNode<HSlider>("VBoxContainer/HSlider");
		AcceptButton = GetNode<Button>("VBoxContainer/HBoxContainer/Accept");
		CancelButton = GetNode<Button>("VBoxContainer/HBoxContainer/Cancel");
	}

	public void Init(sell_slot slot)
	{
		this.slot = slot;

		MaxAmountLabel.Text = $"/ {slot.Amount}";
		Amount = slot.Amount;
		Slider.MaxValue = slot.Amount;
		Slider.MinValue = 1;
		Slider.Value = slot.Amount;

		CoinValueLabel.Text = (Amount * slot.ItemDatabaseRow.SellPrice).ToString();

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
		slot.MoveToOtherContainer(Amount);
        QueueFree();
    }

    private void Slider_ValueChanged(double value)
    {
		Amount = (int)value;
    }
}
