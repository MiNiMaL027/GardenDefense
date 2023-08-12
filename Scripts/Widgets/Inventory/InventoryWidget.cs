using Godot;
using Items;
using System.Collections.Generic;

namespace Widgets.Inventory
{
    public partial class InventoryWidget : Control
    {
        public List<InventorySlot> InventorySlots { get; set; } = new List<InventorySlot>();
        public InventoryComponent InventoryComponent { get; set; }
        public GridContainer gridContainer { get; set; }

        private AnimationPlayer Animation { get; set; }

        public override void _Ready()
        {
            Animation = GetNode<AnimationPlayer>("Animation");
            gridContainer = GetNode<GridContainer>("GridContainer");
        }
        public virtual void SetInventory(InventoryComponent inventoryComponentToSet)
        {
            RemoveSlots();

            InventoryComponent = inventoryComponentToSet;
            InventoryComponent.ItemAdded += ItemAddedListener;
            InventoryComponent.ItemRemoved += ItemRemovedListener;

            for (int i = 0; i < InventoryComponent.InventoryIdArray.Count; i++)
            {
                InventorySlot slot = Scenes.Widgets.Inventory.InventorySlot();
                InventorySlots.Add(slot);
                gridContainer.AddChild(slot);
                slot.Init(InventoryComponent.InventoryIdArray[i], InventoryComponent.InventoryAmountArray[i], this);
            }
        }
        private void ItemAddedListener(int id, int amount, int indexInArray)
        {
            if (indexInArray < InventorySlots.Count) //if slot exists just update it
            {
                InventorySlot slot = InventorySlots[indexInArray];
                slot.Amount += amount;
            }
            else
            {
                InventorySlot slot = Scenes.Widgets.Inventory.InventorySlot();
                InventorySlots.Add(slot);
                gridContainer.AddChild(slot);
                slot.Init(id, amount, this);
            }
        }
        private void ItemRemovedListener(int id, int amount, int indexInArray)
        {
            InventorySlot removedSlot = InventorySlots[indexInArray];
            removedSlot.Amount -= amount;

            if (removedSlot.Amount <= 0)
            {
                InventorySlots.Remove(removedSlot);
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

        public void Open()
        {
            foreach(InventorySlot slot in InventorySlots)
            {
                if(slot.itemTooltip != null)
                {
                    slot.InventorySlot_MouseExited();
                }
            }

            Animation.PlayBackwards("Close");
        }

        public void Close()
        {
            Animation.Play("Close");
        }
    }
}
