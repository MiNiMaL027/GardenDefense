using Enums;
using Godot;
using Items;
using Widgets.Buttons;

namespace Widgets.Bestiary
{
    public partial class BestiaryWindow : Control
    {

        TextureButton ButtonClose;

        ItemList ItemListContainer;

        VBoxContainer ItemDescInfo;

        HBoxContainer MainInfo;

        VBoxContainer AdditionalInfo;

        Panel DescPanel;

        Label BuyPriceLabel;
        Label SellPriceLabel;

        ItemType currentCategorie;

        VBoxContainer CategorieseContainer;

        public override void _Ready()
        {
            ButtonClose = GetNode<TextureButton>("HBoxContainer/PanelContainer/HBoxContainer/Spacer/ButtonClose");
            ItemListContainer = GetNode<ItemList>("HBoxContainer/PanelContainer/HBoxContainer/ItemList");
            ItemDescInfo = GetNode<VBoxContainer>("HBoxContainer/PanelContainer/HBoxContainer/ItemDescription/PanelContainer/VBoxContainer/MarginContainer/Description");
            CategorieseContainer = GetNode<VBoxContainer>("HBoxContainer/CategoriesContainer");

            MainInfo = GetNode<HBoxContainer>("HBoxContainer/PanelContainer/HBoxContainer/ItemDescription/PanelContainer/VBoxContainer/MainInfo");
            BuyPriceLabel = GetNode<Label>("HBoxContainer/PanelContainer/HBoxContainer/ItemDescription/PanelContainer/VBoxContainer/MarginContainer/Description/CostInfo/BuyPrice");
            SellPriceLabel = GetNode<Label>("HBoxContainer/PanelContainer/HBoxContainer/ItemDescription/PanelContainer/VBoxContainer/MarginContainer/Description/CostInfo/SellPrice");
            AdditionalInfo = GetNode<VBoxContainer>("HBoxContainer/PanelContainer/HBoxContainer/ItemDescription/PanelContainer/VBoxContainer/MarginContainer/Description/AdditionalInfo");
            DescPanel = GetNode<Panel>("HBoxContainer/PanelContainer/HBoxContainer/ItemDescription/PanelContainer/desc");

            ItemListContainer.ItemSelected += ItemCointeiner_ItemActivated;
            
            ButtonClose.Pressed += ButtonClose_Pressed;

            Init();
        }

        private void ItemCointeiner_ItemActivated(long index)
        {
            ClearAdditionalInfo();         

            var bestiariy = this.GetPlayerController().bestiaryItems[currentCategorie];
            var item = DbService.GetItem(bestiariy[(int)index]);

            MainInfo.GetChild<TextureRect>(0).Texture = ResourceLoader.Load<Texture2D>(item.TextureSpritePath);

            MainInfo.GetChild<Label>(1).Text = item.ItemName;
            ItemDescInfo.GetChild<Label>(1).Text = item.Description;
            BuyPriceLabel.Text = $"{item.BuyPrice}";
            SellPriceLabel.Text = $"{item.SellPrice}";

            InitAdditionalContainer(item);
        }

        private void ButtonClose_Pressed()
        {
            Hud hud = this.GetHud();

            hud.CloseBestiary();
        }
 
        public void Init()
        {
            var bestiariy = this.GetPlayerController().bestiaryItems;

            if (CategorieseContainer.GetChildCount() > 0)
                for (int i = 0; i < CategorieseContainer.GetChildCount(); i++)
                {
                    CategorieseContainer.RemoveChild(CategorieseContainer.GetChild<Button>(i));
                }

            foreach(ItemType type in bestiariy.Keys)
            {
                var newButton = new CategoriesButton() { ToggleMode = true, Text = $"{type}", ButtonGroup = ResourceLoader.Load<ButtonGroup>("res://Scenes/Widgets/Bestiary/ButtonGroupBestiaryCategory.tres") };
                newButton.ButtonClicked += NewButton_ButtonClicked;
                newButton.ItemType = type;

                CategorieseContainer.AddChild(newButton);
            }         
        }

        private void NewButton_ButtonClicked(object sender, ButtonEventData e)
        {
            OpenCategorie(e.ItemType);
        }

        private void OpenCategorie(ItemType type)
        {
            ItemListContainer.Clear();

            var bestiariy = this.GetPlayerController().bestiaryItems[type];

            for (int i = 0; i < bestiariy.Count; i++)
            {
                var item = DbService.GetItemDataById(bestiariy[i]);
                GD.Print(item.itemName);
                ItemListContainer.AddItem(item.itemName, item.texture);
            } 
            
            currentCategorie = type;
        }

        private void ClearAdditionalInfo()
        {
            foreach(var child in AdditionalInfo.GetChildren())
            {
                child.QueueFree();
            }
        }

        private void InitAdditionalContainer(ItemDatabaseRow item)
        {
            if (item is SeedDatabaseRow)
            {
                var seed = (SeedDatabaseRow)item;

                var setting = new LabelSettings();
                setting.FontSize = 25;
                setting.OutlineColor = new Color(0, 0, 0);
                setting.OutlineSize = 5;

                var seedTypeLabel = new Label();
                seedTypeLabel.LabelSettings = setting;

                var stageAmountLabel = new Label();
                stageAmountLabel.LabelSettings = setting;

                var stageTimeLabel = new Label();
                stageTimeLabel.LabelSettings = setting;

                var cropValueLabel = new Label();
                cropValueLabel.LabelSettings = setting;

                AdditionalInfo.AddChild(seedTypeLabel);
                AdditionalInfo.AddChild(stageAmountLabel);
                AdditionalInfo.AddChild(stageTimeLabel);
                AdditionalInfo.AddChild(cropValueLabel);


                seedTypeLabel.Text = $"Soket / Seed type - {seed.SeedType}";
                stageAmountLabel.Text = $"Stage to grow - {seed.StagesAmount}";
                stageTimeLabel.Text = $"Time to change stage ~ {seed.MinSecondsToChangeState} - {seed.MaxSecondsToChangeState} sec.";
                cropValueLabel.Text = $"Crop ~ {seed.MinCropAmount} - {seed.MaxCropAmount}";

                return;
            }

            if (item is FertilizerDatabaseRow)
            {
                var fertilizer = (FertilizerDatabaseRow)item;

                var setting = new LabelSettings();
                setting.FontSize = 25;
                setting.OutlineColor = new Color(0, 0, 0);
                setting.OutlineSize = 5;

                var timeLabel = new Label();
                timeLabel.LabelSettings = setting;

                AdditionalInfo.AddChild(timeLabel);

                timeLabel.Text = $"Active time - {fertilizer.SecondsDuration}";

                return;
            }

            if(item is PotDatabaseRow)
            {
                var pot = (PotDatabaseRow)item;

                var setting = new LabelSettings();
                setting.FontSize = 25;
                setting.OutlineColor = new Color(0, 0, 0);
                setting.OutlineSize = 5;

                var soketCountLabel = new Label();
                soketCountLabel.LabelSettings = setting;

                AdditionalInfo.AddChild(soketCountLabel);

                soketCountLabel.Text = $"Small sokets - {pot.SmallPotsAmount} \nBig sokets - {pot.BigPotsAmount}";

                return;
            }
        }
    }
}

