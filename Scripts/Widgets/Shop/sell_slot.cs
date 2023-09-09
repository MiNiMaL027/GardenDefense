using Farm.Scripts.Widgets;
using Godot;
using Items;
using System;

public partial class sell_slot : BaseSlot
{
	public SellWindow ParentWidget { get; set; }
	public TextureRect Icon { get; set; }
	public Label SellPrice { get; set; }

	private movement_slot MSlot { get; set; }

	public bool InSellContainer;

	public event EventHandler<(sell_slot, int)> MoveSlotToSellContainer;

    public override void _Ready()
    {
        Icon = GetNode<TextureRect>("TextureRect");
        LabelAmount = GetNode<Label>("Amount");
        SellPrice = GetNode<Label>("HBoxContainer/SellPrice");
    }

    public void Init(ItemDatabaseRow item, int itemAmount, SellWindow parentWidget)
	{
        ParentWidget = parentWidget;
		ItemDatabaseRow = item;
        Icon.Texture = ResourceLoader.Load<Texture2D>(ItemDatabaseRow.TextureSpritePath);
		SellPrice.Text = ItemDatabaseRow.SellPrice.ToString();
		amount = itemAmount;
		if(amount > 1)
		{
			LabelAmount.Text = amount.ToString();
		}
		else
		{
			LabelAmount.Text = "";
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
			MoveToWidgetAtMousePosiiton();
            MSlot.QueueFree();
		}

		if(@event is InputEventMouseButton mouseDoubleClick && mouseDoubleClick.ButtonIndex == MouseButton.Left && mouseDoubleClick.DoubleClick)
		{
			MoveToWidgetAtMousePosiiton(true);
        }
    }

	public void MoveToWidgetAtMousePosiiton(bool exactlyMove = false)
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
						var amountWindow = Scenes.Widgets.Inventory.InventoryAmountWindow();

						if (Amount == 1)
						{
							AmountWindow_ButtonPressedAccept(amount, true);
							return;
						}

						hud.AddChild(amountWindow);
						amountWindow.Init(amount, ItemDatabaseRow.SellPrice, true);
                        amountWindow.ButtonPressedAccept += AmountWindow_ButtonPressedAccept;
					}
                }
            }
        }
    }

    private void AmountWindow_ButtonPressedAccept(int amount, bool sell)
    {
        MoveSlotToSellContainer?.Invoke(this, (this, amount));
    }
}
