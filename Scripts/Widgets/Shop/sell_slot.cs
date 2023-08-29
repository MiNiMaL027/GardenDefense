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
			}
			else if(amount == 1)
			{
				AmountLabel.Text = "";
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

	bool isItemMove;
	public ItemDatabaseRow ItemDatabaseRow { get; set; }

    public override void _PhysicsProcess(double delta)
    {
        if(isItemMove)
		{
			MoveItem();
		}
    }

    public override void _Ready()
    {
        Icon = GetNode<TextureRect>("Panel/TextureRect");
        AmountLabel = GetNode<Label>("Panel/Amount");
        SellPrice = GetNode<Label>("Panel/SellPrice");
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
			TextureRect texture = new TextureRect();
			texture.Texture = ResourceLoader.Load<Texture2D>(ItemDatabaseRow.TextureSpritePath);
			playerController.Hud.AddChild(texture);

			isItemMove = true;
        }
		else if(@event is InputEventMouseButton mouseButtonUp && mouseButtonUp.ButtonIndex == MouseButton.Left && mouseButtonUp.IsPressed() == false)
		{
			isItemMove = false;
			
		}
    }

	private void MoveItem()
	{

	}
}
