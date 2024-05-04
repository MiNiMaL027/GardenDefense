using Godot;
using SaveModels;
using System;
using System.Collections.Generic;

public partial class InventoryComponent : Node
{
    [Signal]
    public delegate void ItemAddedEventHandler(int id, int amount, int indexInArray);
    [Signal]
    public delegate void ItemRemovedEventHandler(int id, int amount, int indexInArray);

    public InventoryComponent()
    {
        InventoryIdArray = new List<int>();
        InventoryAmountArray = new List<int>();
    }
    /// <summary>
    /// Does not trigger events. Call before connection to widget
    /// </summary>
    /// <param name="inv">key is itemId, value is correcponding amount</param>
    public void Init(Dictionary<int, int> inv)
    {
        InventoryIdArray.Clear();
        InventoryAmountArray.Clear();
        int i = 0;
        foreach (KeyValuePair<int, int> kvp in inv)
        {
            InventoryIdArray.Add(kvp.Key);
            InventoryAmountArray.Add(kvp.Value);
        }
    }
    public List<int> InventoryIdArray { get; set; }
    public List<int> InventoryAmountArray { get; set; }

    public void AddItem(int itemId, int amount = 1)
    {
        for (int i = 0; i < InventoryIdArray.Count; ++i)
        {
            if (InventoryIdArray[i] == itemId)
            {
                InventoryAmountArray[i] += amount;
                EmitSignal(SignalName.ItemAdded, itemId, amount, i);

                return;
            }
        }

        InventoryIdArray.Add(itemId);
        InventoryAmountArray.Add(amount);

        EmitSignal(SignalName.ItemAdded, itemId, amount, InventoryIdArray.Count - 1);
    }

    public void RemoveItem(int itemId, int amount)
    {
        for (int i = 0; i < InventoryIdArray.Count; ++i)
        {
            if (InventoryIdArray[i] == itemId)
            {
                if (InventoryAmountArray[i] <= amount)
                {
                    InventoryIdArray.RemoveAt(i);
                    InventoryAmountArray.RemoveAt(i);
                }
                else
                {
                    InventoryAmountArray[i] -= amount;
                }

                EmitSignal(SignalName.ItemRemoved, itemId, amount, i);

                return;
            }
        }
    }

    public int CountOfItem(int itemId)
    {
        for (int i = 0; i < InventoryIdArray.Count; ++i)
        {
            if (InventoryIdArray[i] == itemId)
            {
                return InventoryAmountArray[i];
            }
        }

        return 0;
    }
    internal void LoadFromSave(InventorySave inventorySave)
    {
        InventoryIdArray = new List<int>(inventorySave.InventoryIdArray);
        InventoryAmountArray = new List<int>(inventorySave.InventoryAmountArray);
    }
}
