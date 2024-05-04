using BinarySerialization;
using System.Collections.Generic;

namespace SaveModels
{
    public class InventorySave
    {
        [FieldOrder(0)]
        public int InventoryIdArrayLength;

        [FieldOrder(1)]
        [FieldLength(nameof(InventoryIdArrayLength))]
        public List<int> InventoryIdArray;

        [FieldOrder(2)]
        [FieldLength(nameof(InventoryIdArrayLength))]
        public List<int> InventoryAmountArray;

        public InventorySave(InventoryComponent i)
        {
            InventoryIdArray = new List<int>(i.InventoryIdArray);
            InventoryAmountArray = new List<int>(i.InventoryAmountArray);
        }
        public InventorySave() { }
    }
}
