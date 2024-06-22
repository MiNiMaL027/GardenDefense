using Enums;
using Godot;
using System.Collections.Generic;
using static Widgets.BaseSlot;

namespace Widgets.Inventory
{
    public partial class GardenInventoryWidget : Control
    {
        public static List<ItemType> cachedInventoryWidgetTypesToDisplay = new List<ItemType> { ItemType.Misc, ItemType.Seed, ItemType.Fertilizer, ItemType.Fertilizer, ItemType.Harvestable, ItemType.Pot, ItemType.BattlePlant };
        public static IComparer<BaseSlot> cachedSortingOrder = new BaseSlot.Comparers.DefaultAsc();
        public Button ButtonFiltering { get; set; }
        public PopupMenu PopupMenuFiltering { get; set; }
        public OptionButton OptionButtonSortingType { get; set; }
        public OptionButton OptionButtonSortingOrder { get; set; }
        public InventoryWidget InventoryWidget { get; set; }
        public override void _Ready()
        {
            ButtonFiltering = GetNode<Button>("VBoxContainer/HBoxContainer/ButtonFiltering");
            ButtonFiltering.Pressed += ButtonFiltering_Pressed;
            PopupMenuFiltering = GetNode<PopupMenu>("VBoxContainer/HBoxContainer/PopupMenu");
            PopupMenuFiltering.IndexPressed += PopupMenuFiltering_IndexPressed;
            InventoryWidget = GetNode<InventoryWidget>("VBoxContainer/InventoryWidget");
            OptionButtonSortingType = GetNode<OptionButton>("VBoxContainer/HBoxContainer/OptionButtonSortingType");
            OptionButtonSortingType.ItemSelected += OptionButtonSortingType_ItemSelected;
            OptionButtonSortingOrder = GetNode<OptionButton>("VBoxContainer/HBoxContainer/OptionButtonSortingOrder");
            OptionButtonSortingOrder.ItemSelected += OptionButtonSortingOrder_ItemSelected;

        }
        private void OptionButtonSortingOrder_ItemSelected(long index)
        {
            if (OptionButtonSortingType.GetSelectedId() == -1)
                return;
            ChooseCorrectComparer();
        }
        private void OptionButtonSortingType_ItemSelected(long index)
        {
            if (OptionButtonSortingOrder.GetSelectedId() == -1)
                return;
            ChooseCorrectComparer();

        }
        public void ChooseCorrectComparer()
        {
            int sortingOrder = OptionButtonSortingOrder.GetSelectedId();
            int sortingType = OptionButtonSortingType.GetSelectedId();
            if(sortingOrder == 0)
            {
                if (sortingType == 0)
                {
                    InventoryWidget.SortingOrder = new BaseSlot.Comparers.DefaultAsc();
                }
                else if(sortingType == 1)
                {
                    InventoryWidget.SortingOrder = new BaseSlot.Comparers.PriceAsc();
                }
                else if (sortingType == 2)
                {
                    InventoryWidget.SortingOrder = new BaseSlot.Comparers.AmountAsc();
                }
                else if (sortingType == 3)
                {
                    InventoryWidget.SortingOrder = new BaseSlot.Comparers.TypeAsc();
                }
            }
            else
            {
                if (sortingType == 0)
                {
                    InventoryWidget.SortingOrder = new BaseSlot.Comparers.DefaultDesc();
                }
                else if (sortingType == 1)
                {
                    InventoryWidget.SortingOrder = new BaseSlot.Comparers.PriceDesc();
                }
                else if (sortingType == 2)
                {
                    InventoryWidget.SortingOrder = new BaseSlot.Comparers.AmountDesc();
                }
                else if (sortingType == 3)
                {
                    InventoryWidget.SortingOrder = new BaseSlot.Comparers.TypeDesc();
                }
            }
        }
        private void PopupMenuFiltering_IndexPressed(long index)
        {
            bool previousState = PopupMenuFiltering.IsItemChecked((int)index);

            if(previousState == true) //perform removing
            {
                InventoryWidget.RemoveDisplayedTypes((ItemType)index);
            }
            else //perform adding
            {
                InventoryWidget.AddDisplayedTypes((ItemType)index);
            }
            PopupMenuFiltering.SetItemChecked((int)index, !previousState);
        }
        private void ButtonFiltering_Pressed()
        {
            PopupMenuFiltering.Popup();
            var buttonPosition = ButtonFiltering.GlobalPosition;
            PopupMenuFiltering.Position = new Vector2I((int)buttonPosition.X, (int)buttonPosition.Y);
        }
        public virtual void SetInventory(InventoryComponent inventoryComponentToSet)
        {
            InventoryWidget.SetInventory(inventoryComponentToSet, cachedSortingOrder, cachedInventoryWidgetTypesToDisplay.ToArray());
            for(int i =0;i< PopupMenuFiltering.ItemCount; i++)
            {
                PopupMenuFiltering.SetItemChecked(i, false);
            }
            foreach (var type in InventoryWidget.displayedTypes)
            {
                PopupMenuFiltering.SetItemChecked((int)type, true);
            }
            switch (cachedSortingOrder)
            {
                case Comparers.DefaultAsc _:
                    OptionButtonSortingOrder.Select(0);
                    OptionButtonSortingType.Select(0);
                    break;
                case Comparers.DefaultDesc _:
                    OptionButtonSortingOrder.Select(1);
                    OptionButtonSortingType.Select(0);
                    break;
                case Comparers.PriceAsc _:
                    OptionButtonSortingOrder.Select(0);
                    OptionButtonSortingType.Select(1);
                    break;
                case Comparers.PriceDesc _:
                    OptionButtonSortingOrder.Select(1);
                    OptionButtonSortingType.Select(1);
                    break;
                case Comparers.AmountAsc _:
                    OptionButtonSortingOrder.Select(0);
                    OptionButtonSortingType.Select(2);
                    break;
                case Comparers.AmountDesc _:
                    OptionButtonSortingOrder.Select(1);
                    OptionButtonSortingType.Select(2);
                    break;
                case Comparers.TypeAsc _:
                    OptionButtonSortingOrder.Select(0);
                    OptionButtonSortingType.Select(3);
                    break;
                case Comparers.TypeDesc _:
                    OptionButtonSortingOrder.Select(1);
                    OptionButtonSortingType.Select(3);
                    break;
                default:
                    break;
            }
        }
        public override void _ExitTree()
        {
            base._ExitTree();
            cachedInventoryWidgetTypesToDisplay=InventoryWidget.displayedTypes;
            cachedSortingOrder = InventoryWidget.SortingOrder;
        }
    }
}
