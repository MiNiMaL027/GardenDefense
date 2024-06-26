using Enums;
using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Widgets.Buttons;

namespace Widgets.Inventory
{
    /// <summary>
    /// Base widget container for items. Supports filtering and sorting order.
    /// </summary>
    public partial class InventoryWidget : Control
    {
        [Export]
        public InventorySlotType InventorySlotType { get; set; } = InventorySlotType.InventorySlot;
        public BaseSlot InstantiateSlot()
        {
            switch(InventorySlotType)
            {
                case InventorySlotType.InventorySlot:
                    return Scenes.Widgets.Inventory.InventorySlot();
                case InventorySlotType.BattlefieldSlot:
                    return Scenes.Widgets.Inventory.BattlefieldBattlePlantSlot();
                default: return null; //TODO add battlefield slot
            }
        }
        public IComparer<BaseSlot>? SortingOrder
        {
            get
            {
                return sortingOrder;
            }
            set
            {
                sortingOrder = value;
                if(sortingOrder is BaseSlot.Comparers.DefaultAsc || sortingOrder is BaseSlot.Comparers.DefaultDesc)
                {
                    FillSlots();
                }
                else
                {
                    ReorderSlots();
                }

            }
        }
        private IComparer<BaseSlot>? sortingOrder;
        public List<BaseSlot> InventorySlots { get; set; }

        private void ReorderSlots()
        {
            InventorySlots.Sort(sortingOrder);
            for (int i = 0; i < InventorySlots.Count; i++)
            {
                gridContainer.MoveChild(InventorySlots[i], i);
            }
        }
        public InventoryComponent InventoryComponent { get; set; }
        public GridContainer gridContainer { get; set; }

        public List<ItemType> displayedTypes;

        public void AddDisplayedTypes(params ItemType[] types)
        {
            foreach (ItemType type in types)
            {
                if(displayedTypes.Contains(type) == false)
                {
                    displayedTypes.Add(type);
                }
            }
            FillSlots();
        }
        public void RemoveDisplayedTypes(params ItemType[] types)
        {
            foreach (ItemType type in types)
            {
                while(displayedTypes.Contains(type) == true)
                {
                    displayedTypes.Remove(type);

                }
            }
            FillSlots();
        }

        public override void _Ready()
        {
            displayedTypes = new List<ItemType>();
            gridContainer = GetNode<GridContainer>("Panel/MarginContainer/ScrollContainer/GridContainer");
        }

        public virtual void SetInventory(InventoryComponent inventoryComponentToSet, IComparer<BaseSlot> comparerToSet = null, params ItemType[] displayedTypesToSet)
        {
            InventoryComponent = inventoryComponentToSet;
            InventoryComponent.ItemAdded += ItemAddedListener;
            InventoryComponent.ItemRemoved += ItemRemovedListener;
            if(comparerToSet != null)
            {
                InventorySlots = new List<BaseSlot>();
                sortingOrder = comparerToSet;

            }
            else
            {
                InventorySlots = new List<BaseSlot>();
                sortingOrder = new BaseSlot.Comparers.DefaultAsc();
            }
            displayedTypes = displayedTypesToSet.ToList();
            FillSlots();
        }
        private void ItemAddedListener(int id, int amount, int indexInArray)
        {
            ItemType itemType = DbService.GetItemType(id);
            if (displayedTypes.Contains(itemType) == false)
                return;


            BaseSlot existingSlot = InventorySlots.FirstOrDefault(s => s.ItemDatabaseRow != null && s.ItemDatabaseRow.Id == id);
            if (existingSlot != null)
            {
                existingSlot.Amount += amount;
            }
            else
            {
                var item = DbService.GetItem(id);
                BaseSlot slot = InstantiateSlot();
                InventorySlots.Add(slot);
                gridContainer.AddChild(slot);
                slot.Init(item, amount);
            }
        }
        private void ItemRemovedListener(int id, int amount, int indexInArray)
        {
            BaseSlot removedSlot = InventorySlots.FirstOrDefault(x => x.ItemDatabaseRow != null && x.ItemDatabaseRow.Id == id);
            if(removedSlot != null)
            {
                removedSlot.Amount -= amount;

                if (removedSlot.Amount <= 0 && removedSlot.CanBeEmpty == false)
                {
                    InventorySlots.Remove(removedSlot);
                }

            }

        }

        private void RemoveSlots()
        {
            InventorySlots.Clear();
            Godot.Collections.Array<Node> children = gridContainer.GetChildren();
            foreach(Node child in children)
            {
                child.QueueFree();
            }
        }

        public override void _ExitTree()
        {
            base._ExitTree();
            if (InventoryComponent != null)
            {
                InventoryComponent.ItemAdded -= ItemAddedListener;
                InventoryComponent.ItemRemoved -= ItemRemovedListener;
            }
        }
        /// <summary>
        /// Remove existing slots in widget and load InventoryComponent in widget
        /// </summary>
        public void FillSlots()
        {
            RemoveSlots();
            if(sortingOrder is BaseSlot.Comparers.DefaultDesc)
            {
                for (int i = InventoryComponent.InventoryIdArray.Count-1; i > -1; i--)
                {
                    var item = DbService.GetItem(InventoryComponent.InventoryIdArray[i]);
                    if (displayedTypes.Contains(item.ItemType))
                    {
                        BaseSlot slot = InstantiateSlot();
                        InventorySlots.Add(slot);
                        gridContainer.AddChild(slot);
                        slot.Init(item, InventoryComponent.InventoryAmountArray[i]);
                    }
                }
            }
            else
            {
                for (int i = 0; i < InventoryComponent.InventoryIdArray.Count; i++)
                {
                    var item = DbService.GetItem(InventoryComponent.InventoryIdArray[i]);
                    if (displayedTypes.Contains(item.ItemType))
                    {
                        BaseSlot slot = InstantiateSlot();
                        InventorySlots.Add(slot);
                        gridContainer.AddChild(slot);
                        slot.Init(item, InventoryComponent.InventoryAmountArray[i]);
                    }
                }
                ReorderSlots();
            }
            
        }
    }
}
