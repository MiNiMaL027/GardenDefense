using Enums;
using Godot;
using Items;
using Pawns;
using System.Collections.Generic;
using System.Linq;
using Widgets.Buttons;

namespace Widgets.Bestiary
{
    public enum BestiaryCategory
    {
        Item=0,
        Monster=1
    }
    public class BestiaryCategoryData
    {
        public BestiaryCategory category;
        public ItemType itemType;
    }
    public partial class BestiaryWindow : Control
    {
        TextureButton ButtonClose;
        GridContainer ItemListContainer;
        VBoxContainer CategoriesContainer;
        DescWidget openedDescWidget;
        BestiaryListItem currentlySelectedListItem;
        Control ItemDescription;

        public static BestiaryCategoryData LastOpenedCategory;
        public static int LastOpenedId;

        public override void _Ready()
        {
            ButtonClose = GetNode<TextureButton>("HBoxContainer/PanelContainer/HBoxContainer/Spacer/ButtonClose");
            ItemListContainer = GetNode<GridContainer>("HBoxContainer/PanelContainer/HBoxContainer/ItemList");
            CategoriesContainer = GetNode<VBoxContainer>("HBoxContainer/CategoriesContainer");
            ItemDescription = GetNode<Control>("HBoxContainer/PanelContainer/HBoxContainer/ItemDescription");
            
            ButtonClose.Pressed += ButtonClose_Pressed;

            Init();
        }
        public void Init()
        {
            ClearCategories();
            AddCategories();

        }
        private void ClearCategories()
        {
            Godot.Collections.Array<Node> categoriesButtons = CategoriesContainer.GetChildren();
            foreach (Node n in categoriesButtons)
            {
                n.QueueFree();
            }

        }
        private void ClearItemList()
        {
            Godot.Collections.Array<Node> items = ItemListContainer.GetChildren();
            foreach (Node n in items)
            {
                n.QueueFree();
            }

        }
        private void CloseOpenedDescWidget()
        {
            if (openedDescWidget != null)
            {
                openedDescWidget.QueueFree();
                openedDescWidget = null;
            }
        }
        private void AddCategories()
        {
            ///add item categories first
            var bestiary = this.GetPlayerController().bestiaryItems;
            foreach (ItemType type in bestiary.Keys)
            {
                BestiaryCategoryData itemCategoryData = new BestiaryCategoryData()
                {
                    category = BestiaryCategory.Item,
                    itemType = type
                };
                var itemCategoryButton = new BestiaryCategoryButton()
                {
                    ToggleMode = true,
                    Text = $"{type}",
                    ButtonGroup = ResourceLoader.Load<ButtonGroup>("res://Scenes/Widgets/Bestiary/ButtonGroupBestiaryCategory.tres"),
                    bestiaryCategoryData = itemCategoryData
                };
                itemCategoryButton.CategoryClicked += CategoryClicked;
                CategoriesContainer.AddChild(itemCategoryButton);
            }

            ///add another category that represents monsters
            var monsterButton = new BestiaryCategoryButton()
            {
                ToggleMode = true,
                Text = "Monster",
                ButtonGroup = ResourceLoader.Load<ButtonGroup>("res://Scenes/Widgets/Bestiary/ButtonGroupBestiaryCategory.tres")
            };
            monsterButton.CategoryClicked += CategoryClicked;
            monsterButton.bestiaryCategoryData = new BestiaryCategoryData()
            {
                category = BestiaryCategory.Monster
            };
            CategoriesContainer.AddChild(monsterButton);
        }

        private void ButtonClose_Pressed()
        {
            Hud hud = this.GetHud();

            hud.CloseBestiary();
        }

        private void CategoryClicked(BestiaryCategoryButton targetButton)
        {
            ClearItemList();
            CloseOpenedDescWidget();
            LastOpenedCategory = targetButton.bestiaryCategoryData;
            List<int> itemIds = null;
            if(LastOpenedCategory.category == BestiaryCategory.Item)
            {
                itemIds = this.GetPlayerController().bestiaryItems[LastOpenedCategory.itemType];
                foreach(int itemId in itemIds)
                {
                    var item = DbService.GetItemDataById(itemId);

                    BestiaryListItem bestiaryListItem = Scenes.Widgets.Bestiary.BestiaryListItem();
                    ItemListContainer.AddChild(bestiaryListItem);
                    bestiaryListItem.Init(item.name, item.texture, itemId);
                    bestiaryListItem.ItemSelected += BestiaryListItem_ItemSelected;
                }
            }
            else
            {
                itemIds = this.GetPlayerController().bestiaryMonsters;
                foreach (int itemId in itemIds)
                {
                    var item = DbService.GetPawnDataById(itemId);

                    BestiaryListItem bestiaryListItem = Scenes.Widgets.Bestiary.BestiaryListItem();
                    ItemListContainer.AddChild(bestiaryListItem);
                    bestiaryListItem.Init(item.name, item.texture, itemId);
                    bestiaryListItem.ItemSelected += BestiaryListItem_ItemSelected;

                }
            }

        }

        private void BestiaryListItem_ItemSelected(BestiaryListItem sender)
        {
            CloseOpenedDescWidget();
            LastOpenedId = sender.ItemId;
            if(currentlySelectedListItem != null && Godot.GodotObject.IsInstanceValid(currentlySelectedListItem) == true)
            {
                currentlySelectedListItem.Set("theme_override_styles/panel", BestiaryListItem.DefaultItemStyle);
            }
            currentlySelectedListItem = sender;
            currentlySelectedListItem.Set("theme_override_styles/panel", BestiaryListItem.SelectedItemStyle);
            if(LastOpenedCategory.category == BestiaryCategory.Item)
            {
                openedDescWidget = Item.GetBestiaryDescriptionSceneByType(LastOpenedCategory.itemType);
                ItemDatabaseRow itemDatabaseRow = DbService.GetItem(currentlySelectedListItem.ItemId);
                ItemDescription.AddChild(openedDescWidget);
                openedDescWidget.Init(itemDatabaseRow);
            }
            else
            {
                PawnDatabaseRow pawnDatabaseRow = DbService.GetPawn(currentlySelectedListItem.ItemId);
                openedDescWidget = Scenes.Widgets.Bestiary.MonsterDescWidget();
                ItemDescription.AddChild(openedDescWidget);
                openedDescWidget.Init(pawnDatabaseRow);
            }
        }

        public bool OpenExactItem(ItemType categoryType, int itemId)
        {
            BestiaryCategoryButton targetButton = (BestiaryCategoryButton)CategoriesContainer.GetChildren().FirstOrDefault(b => (b as Button).Text == categoryType.ToString());
            if (targetButton == null)
            {
                return false;
            }
            targetButton.ButtonPressed = true;
            targetButton.EmitSignal("pressed");
            BestiaryListItem listItem = (BestiaryListItem)ItemListContainer.GetChildren().FirstOrDefault(n => ((BestiaryListItem)n).ItemId == itemId);
            if(listItem == null)
            {

                return false;
            }
            BestiaryListItem_ItemSelected(listItem);
            return true;
        }
        public bool OpenExactMonster(int monsterId)
        {
            return false;
        }
        public void OpenLastItem()
        {

        }
    }
}

