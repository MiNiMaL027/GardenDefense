using Controllers;
using Godot;
using Items;
using Pawns;
using System;

public partial class LaboratorySlot : Panel
{
	TextureRect BackgroundTexture { get; set; }
	TextureRect MainBattlePlantIcon { get; set; }
	TextureRect HarvestIcon { get; set; }
	Label HarvestPriceLabel { get; set; }
	Label GoldPriceLabel { get; set; }

	Label HpLabel { get; set; }
	Label DamageLabel { get; set; }
	Label AttackSpeedLabel { get; set; }
	Label RangeLabel { get; set; }

	Label ItemNameLabel { get; set; }

	private bool _IsActive;
	public bool IsActive {
		get
		{ 
			return _IsActive;
		} 
		set 
		{
			if (value)
			{
				BackgroundTexture.Texture = null;
				GetChild<MarginContainer>(1).Visible = true;
                MouseDefaultCursorShape = CursorShape.Arrow;
            }
			else
			{
				BackgroundTexture.Texture = ResourceLoader.Load<Texture2D>("res://raw assets/Images/Lock.png");
                GetChild<MarginContainer>(1).Visible = false;
                MouseDefaultCursorShape = CursorShape.Cross;
            }

			_IsActive = value;
		}
	}

	public Button BuyButton { get; set; }

	[Export]
	public int BattlePlantId;
	private int HarvestCount;
	private int GoldPrice;
	private int HarvestId;

    public override void _Ready()
	{
		MainBattlePlantIcon = GetNode<TextureRect>("MarginContainer/VBoxContainer/HBoxContainer/TextureRect");
		HarvestIcon = GetNode<TextureRect>("MarginContainer/VBoxContainer/Button/HBoxContainer/HarvestTexture");

		HarvestPriceLabel = GetNode<Label>("MarginContainer/VBoxContainer/Button/HBoxContainer/HarvestPrice");
		GoldPriceLabel = GetNode<Label>("MarginContainer/VBoxContainer/Button/HBoxContainer/GoldPrice");

		HpLabel = GetNode<Label>("MarginContainer/VBoxContainer/HBoxContainer/VBoxContainer/HpContainer/HpLabel");
		DamageLabel = GetNode<Label>("MarginContainer/VBoxContainer/HBoxContainer/VBoxContainer/DamgeContainer/DamageLabel");
		AttackSpeedLabel = GetNode<Label>("MarginContainer/VBoxContainer/HBoxContainer/VBoxContainer/AttackSpeedContainer/AttackSpeedLabel");
		RangeLabel = GetNode<Label>("MarginContainer/VBoxContainer/HBoxContainer/VBoxContainer/RangeContainer/RangeLabel");
		ItemNameLabel = GetNode<Label>("MarginContainer/VBoxContainer/Label");

		BuyButton = GetNode<Button>("MarginContainer/VBoxContainer/Button");

		BackgroundTexture = GetNode<TextureRect>("BackTextureSlot");

        BuyButton.Pressed += BuyButton_Pressed;

		Init();
    }

    private void BuyButton_Pressed()
    {
        var playerController = this.GetPlayerController();

        if (playerController.Gold < GoldPrice)
            return;

        if (playerController.MainInventory.CountOfItem(HarvestId) < HarvestCount)
            return;
  
        playerController.Gold -= GoldPrice;
		playerController.MainInventory.RemoveItem(HarvestId, HarvestCount);
		playerController.MainInventory.AddItem(BattlePlantId);
    }

    public void Init()
	{
        if (!this.GetPlayerController().avaliableBattlePlantId.Contains(BattlePlantId))
		{
			IsActive = false;
			return;
		}

		IsActive = true;

		var DbBatllePlant = DbService.GetItem(BattlePlantId) as BattlePlantDataBaseRow;


		MainBattlePlantIcon.Texture = ResourceLoader.Load<Texture2D>(DbBatllePlant.TextureSpritePath);
		HarvestIcon.Texture = DbService.GetItemDataById(DbBatllePlant.BuyCropId).texture;

		HarvestPriceLabel.Text = DbBatllePlant.BuyCropCount.ToString();
		GoldPriceLabel.Text = DbBatllePlant.BuyPrice.ToString();

		PawnDatabaseRow pawnDatabaseRow = DbService.GetPawn(DbBatllePlant.PawnId);
        Pawn pawn = GD.Load<PackedScene>(pawnDatabaseRow.ScenePath).Instantiate<Pawn>();
		HpLabel.Text = pawn.PawnStats.MaxHealth.ToString();
		DamageLabel.Text = pawn.PawnStats.Strength.ToString();
        AttackSpeedLabel.Text = pawn.PawnStats.AttackSpeed.ToString();
        RangeLabel.Text = pawn.PawnStats.AttackRange.ToString();
		pawn.QueueFree();

		HarvestCount = DbBatllePlant.BuyCropCount;
		GoldPrice = DbBatllePlant.BuyPrice;
		HarvestId = DbBatllePlant.BuyCropId;

		ItemNameLabel.Text = DbBatllePlant.ItemName;

		ButtonInit();
	}

	public void ButtonInit()
	{
        var playerController = this.GetPlayerController();

        if (playerController.Gold < GoldPrice)
		{
			GoldPriceLabel.LabelSettings.FontColor = new Color(0.651f, 0.086f, 0.059f);
        }
		else
		{
			GoldPriceLabel.LabelSettings.FontColor = new Color(0.086f, 0.424f, 0.086f);
        }

		if(playerController.MainInventory.CountOfItem(HarvestId) < HarvestCount)
		{
			HarvestPriceLabel.LabelSettings.FontColor = new Color(0.651f, 0.086f, 0.059f);
        }
		else
		{
			HarvestPriceLabel.LabelSettings.FontColor = new Color(0.086f, 0.424f, 0.086f);

        }          
    }
}
