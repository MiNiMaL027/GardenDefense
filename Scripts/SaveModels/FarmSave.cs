using BinarySerialization;
using System;
using System.Collections.Generic;

namespace SaveModels
{
    public class FarmSave
    {
        [FieldOrder(0)]
        public int SavedItemsLength;
        [FieldOrder(1)]
        [FieldLength(nameof(SavedItemsLength))]
        public List<ItemSave> SavedItems;
        [FieldOrder(2)]
        public string SaveDate;

        [FieldOrder(3)]
        public int SavedPotsLength;
        [FieldOrder(4)]
        [FieldLength(nameof(SavedPotsLength))]
        public List<PotSave> SavedPots;
    }
}