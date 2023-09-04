using Godot;
using Items;
using System;

public partial class sell_slot : Control
{
	public int Amount 
	{ 
		get { return amount; }
		set
		{ 
			amount = value;
			if(amount > 1)
			{
				AmountLabel.Text = amount.ToString();	
				AllSellPrice.Text = (ItemDatabaseRow.SellPrice * amount).ToString();
			}
			else if(amount == 1)
			{
				AmountLabel.Text = "";
                AllSellPrice.Text = ItemDatabaseRow.SellPrice.ToString();
            }
			else
			{
				QueueFree();
			}
		}
	}
	private int amount;
	public SellWindow ParentWidget { get; set; }
	public TextureRect Icon { get; set; }
	public Label AmountLabel { get; set; }
	public Label SellPrice { get; set; }
	public Label AllSellPrice { get; set; }
	public ItemDatabaseRow ItemDatabaseRow { get; set; }
	private movement_slot MSlot { get; set; }

	public bool InSellContainer;

	public event EventHandler<(sell_slot,int)> MoveSlotToSellContainer;

    public override void _Ready()
    {
        Icon = GetNode<TextureRect>("TextureRect");
        AmountLabel = GetNode<Label>("Amount");
        SellPrice = GetNode<Label>("HBoxContainer/SellPrice");
		AllSellPrice = GetNode<Label>("HBoxContainer/AllItemSellPrice");
    }

    public void Init(ItemDatabaseRow item, int itemAmount, SellWindow parentWidget)
	{
        ParentWidget = parentWidget;
		ItemDatabaseRow = item;
        Icon.Texture = ResourceLoader.Load<Texture2D>(ItemDatabaseRow.TextureSpritePath);
		SellPrice.Text = ItemDatabaseRow.SellPrice.ToString();
		AllSellPrice.Text = (ItemDatabaseRow.SellPrice * itemAmount).ToString();
		amount = itemAmount;
		if(amount > 1)
		{
			AmountLabel.Text = amount.ToString();
		}
		else
		{
			AmountLabel.Text = "";
		}
    }

    public override void _GuiInput(InputEvent @event)
    {
        base._GuiInput(@event);
        if (@event is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == MouseButton.Left && mouseButton.IsPressed() == true)
        {
			var playerController = this.GetPlayerController();
			MSlot = Scenes.Widgets.Shop.MovementSlot();	
			playerController.Hud.AddChild(MSlot);
			MSlot.icon.Texture = ResourceLoader.Load<Texture2D>(ItemDatabaseRow.TextureSpritePath);
			GD.Print(InSellContainer);
		}
		else if(@event is InputEventMouseButton mouseButtonUp && mouseButtonUp.ButtonIndex == MouseButton.Left && mouseButtonUp.IsPressed() == false)
		{
			GetWidgetAtMousePosiiton();
            MSlot.QueueFree();
		}

		if(@event is InputEventMouseButton mouseDoubleClick && mouseDoubleClick.ButtonIndex == MouseButton.Left && mouseDoubleClick.DoubleClick)
		{
			GetWidgetAtMousePosiiton(true);
        }
    }

	public void GetWidgetAtMousePosiiton(bool exactlyMove = false)
	{
        var mousePosition = GetGlobalMousePosition();
		var hud = this.GetPlayerController().Hud;

        foreach (var widget in hud.GetTree().GetNodesInGroup("SellContainer"))
        {
            if (widget is Control controlWidget)
            {
                // Отримуємо позицію та розмір віджету
                var widgetPosition = controlWidget.GlobalPosition;
                var widgetSize = controlWidget.GetRect().Size;

                // Перевіряємо, чи позиція миші знаходиться в межах віджету
                if (mousePosition.X >= widgetPosition.X &&
                    mousePosition.X <= widgetPosition.X + widgetSize.X &&
                    mousePosition.Y >= widgetPosition.Y &&
                    mousePosition.Y <= widgetPosition.Y + widgetSize.Y)
                {
					if(controlWidget.Name == "VBoxContainer" && !InSellContainer || controlWidget.Name == "VBoxContainer2" && InSellContainer || exactlyMove)
					{
						var amountWindow = Scenes.Widgets.Shop.AmountWindow();

						if (Amount == 1)
						{
							MoveToSellContainer(1);
							return;
						}

						hud.AddChild(amountWindow);
						amountWindow.Init(this);						
					}
                }
            }
        }
    }

	public void MoveToSellContainer(int amount)
	{
        MoveSlotToSellContainer?.Invoke(this,(this, amount));
    }
}
