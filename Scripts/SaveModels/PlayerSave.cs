using BinarySerialization;

namespace SaveModels
{
    public class PlayerSave
    {
        [FieldOrder(0)]
        public int InventorySaveLength { get; set; }
        [FieldOrder(1)]
        [FieldLength(nameof(InventorySaveLength))]
        public InventorySave InventorySave;
        [FieldOrder(2)]
        public int Gold;


    }
}
