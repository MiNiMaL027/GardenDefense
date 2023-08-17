using Farm.Scripts.Enums;
using Farm.Scripts.Models;
using Godot;
using System.Collections.Generic;
using static Godot.ItemList;

namespace Farm.Scripts
{
    public class BestiaryContainer
    {
        public List<ItemList> ItemListContainer;

        public BestiaryContainer()
        {
            ItemListContainer = new List<ItemList>()
            {
                new ItemList(){ MaxColumns = 2,AnchorLeft = 0,AnchorRight = 1,AnchorTop = 0, AnchorBottom = 1, SameColumnWidth = true , IconMode = IconModeEnum.Top , FixedIconSize = new Vector2I(64,64), Visible = true},
                new ItemList(){ MaxColumns = 2,AnchorLeft = 0,AnchorRight = 1,AnchorTop = 0, AnchorBottom = 1, SameColumnWidth = true , IconMode = IconModeEnum.Top , FixedIconSize = new Vector2I(64,64), Visible = true}
            };
        }

        public void AddItem(BestiatyItemType type, BestiaryItemModel item )
        {
            ItemListContainer[(int)type].AddItem(item.Name, item.Texture);
        }
    }  
}
