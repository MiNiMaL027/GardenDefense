using Enums;
using Godot;
using System.Collections.Generic;

namespace Widgets.Bestiary
{
    public partial class BestiaryWindow : Control
    {

        TextureButton ButtonClose;

        ItemList ItemListContainer;

        TextureRect ItemDescTexture;
        VBoxContainer ItemText;

        ItemType currentCategorie;

        VBoxContainer CategorieseContainer;
        public override void _Ready()
        {
            ButtonClose = GetNode<TextureButton>("HBoxContainer/PanelContainer/HBoxContainer/Spacer/ButtonClose");
            ItemListContainer = GetNode<ItemList>("HBoxContainer/PanelContainer/HBoxContainer/ItemList");
            ItemDescTexture = GetNode<TextureRect>("HBoxContainer/PanelContainer/HBoxContainer/ItemDescription/PanelContainer/TextureRect");
            ItemText = GetNode<VBoxContainer>("HBoxContainer/PanelContainer/HBoxContainer/ItemDescription/PanelContainer/desc/VBoxContainer");
            CategorieseContainer = GetNode<VBoxContainer>("HBoxContainer/CategoriesContainer");

            ItemListContainer.ItemSelected += ItemCointeiner_ItemActivated;
            
            ButtonClose.Pressed += ButtonClose_Pressed;

            Init();
        }

        private void BestiarySeedWindow_Pressed()
        {
            OpenCategorie(ItemType.Seed);       
        }

        private void ItemCointeiner_ItemActivated(long index)
        {
            var bestiariy = this.GetPlayerController().bestiaryItems[currentCategorie];
            var item = DbService.GetItem(bestiariy[(int)index]);

            ItemDescTexture.Texture = ResourceLoader.Load<Texture2D>(item.TextureSpritePath);

            ItemText.GetChild<Label>(0).Text = item.ItemName;
            ItemText.GetChild<Label>(1).Text = item.Description;
            ItemText.GetChild<Label>(2).Text = $"{item.BuyPrice}";
            ItemText.GetChild<Label>(3).Text = $"{item.SellPrice}";
        }

        private void ButtonClose_Pressed()
        {
            Hud hud = this.GetHud();
            hud.CloseBestiary();
        }

        private void Clear()
        {
            if(ItemListContainer.ItemCount > 0)
                for (int i = 0; i < ItemListContainer.ItemCount; i++)
                {
                    ItemListContainer.RemoveItem(i);
                }          
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
            Clear();

            var bestiariy = this.GetPlayerController().bestiaryItems[type];

            for (int i = 0; i < bestiariy.Count; i++)
            {
                var item = DbService.GetItemDataById(bestiariy[i]);
                ItemListContainer.AddItem(item.itemName, item.texture);
            } 
            
            currentCategorie = type;
        }
    }
}

