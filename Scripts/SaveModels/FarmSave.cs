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
        public string SaveDateLength;
        [FieldOrder(3)]
        [FieldLength(nameof(SaveDateLength))]
        public string SaveDate;

        [FieldOrder(4)]
        public int SavedPotsLength;
        [FieldOrder(5)]
        [FieldLength(nameof(SavedPotsLength))]
        public List<PotSave> SavedPots;
        public FarmSave() { }
    }
}