using Enums;
using Farm.Scripts;
using Farm.Scripts.Enums;
using Farm.Scripts.Models;
using Godot;
using System.Collections.Generic;

namespace Widgets.Bestiary
{
    public partial class BestiaryWindow : Control
    {
        BestiaryContainer _container;

        TextureButton ButtonClose;

        Control ItemListContainer;

        int previouslyItemListIndex = 0;
        Control ItemDescription;
        VBoxContainer ItemText;

        VBoxContainer CategorieseContainer;
        public override void _Ready()
        {
            _container = new BestiaryContainer();
            ButtonClose = GetNode<TextureButton>("HBoxContainer/PanelContainer/HBoxContainer/Spacer/ButtonClose");
            ItemListContainer = GetNode<Control>("HBoxContainer/PanelContainer/HBoxContainer/ItemListContainer");
            ItemDescription = GetNode<Control>("HBoxContainer/PanelContainer/HBoxContainer/ItemDescription");
            ItemText = GetNode<VBoxContainer>("HBoxContainer/PanelContainer/HBoxContainer/ItemDescription/PanelContainer/desc/VBoxContainer");
            CategorieseContainer = GetNode<VBoxContainer>("HBoxContainer/CategoriesContainer");

            _container.AddItem(BestiatyItemType.Other, new BestiaryItemModel("Carrot", ResourceLoader.Load<Texture2D>("res://raw assets/Images/ItemSprites/icon_carrot.png")));
            _container.AddItem(BestiatyItemType.Seed, new BestiaryItemModel("CarrotSeed", ResourceLoader.Load<Texture2D>("res://raw assets/Images/ItemSprites/CarrotSeedPack_icon.png")));

            CategorieseContainer.GetChild<Button>(0).Pressed += BestiaryWindow_Pressed;
            CategorieseContainer.GetChild<Button>(1).Pressed += BestiaryWindow_Pressed1; ;

            for (int i = 0; i < _container.ItemListContainer.Count; i++)
            {
                _container.ItemListContainer[i].ItemActivated += ItemCointeiner_ItemActivated;
            }
            
            ButtonClose.Pressed += ButtonClose_Pressed;
        }

        private void BestiaryWindow_Pressed1()
        {
            OpenCategory(BestiatyItemType.Other);
        }

        private void BestiaryWindow_Pressed()
        {
            OpenCategory(BestiatyItemType.Seed);
        }

        private void ItemCointeiner_ItemActivated(long index)
        {
            var itemList = ItemListContainer.GetChild<ItemList>(0);
            ItemDescription.GetChild(0).GetChild<TextureRect>(0).Texture = itemList.GetItemIcon((int)index);
            ItemText.GetChild<Label>(0).Text = itemList.GetItemText((int)index);
        }

        private void ButtonClose_Pressed()
        {
            Hud hud = this.GetHud();
            hud.CloseBestiary();
        }

        private void Clear()
        {
            ItemDescription.GetChild(0).GetChild<TextureRect>(0).Texture = null;
            ItemText.GetChild<Label>(0).Text = null;
        }

        private void OpenCategory(BestiatyItemType type)
        {
            if(ItemListContainer.GetChildCount() > 0)
                ItemListContainer.RemoveChild(ItemListContainer.GetChild<ItemList>(0));

            GD.Print(_container.ItemListContainer[(int)type].GetItemText(0));
            ItemListContainer.AddChild(_container.ItemListContainer[(int)type]);
        }
    }
}

