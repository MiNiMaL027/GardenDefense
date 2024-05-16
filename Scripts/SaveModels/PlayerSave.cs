using BinarySerialization;
using System.Collections.Generic;

namespace SaveModels
{
    public class PlayerSave
    {
        [FieldOrder(0)]
        public int InventorySaveLength;
        [FieldOrder(1)]
        [FieldLength(nameof(InventorySaveLength))]
        public InventorySave InventorySave;
        [FieldOrder(2)]
        public int Gold;
        [FieldOrder(3)]
        public int BestiaryMonstersLength;
        [FieldOrder(4)]
        [FieldLength(nameof(BestiaryMonstersLength))]
        public List<int> BestiaryMonsters;
        [FieldOrder(5)]
        public int AvaliableBattlePlantIdLength;
        [FieldOrder(6)]
        [FieldLength(nameof(AvaliableBattlePlantIdLength))]
        public List<int> AvaliableBattlePlantId;

        [FieldOrder(7)]
        public int BestiaryItemsSaveLength { get; set; }

        [FieldOrder(8)]
        [FieldLength(nameof(BestiaryItemsSaveLength))]
        public AvailableItemsSave BestiaryItemsSave;

        [FieldOrder(9)]
        public int AvaliableShopItemsSaveLength { get; set; }

        [FieldOrder(10)]
        [FieldLength(nameof(AvaliableShopItemsSaveLength))]
        public AvailableItemsSave AvaliableShopItemsSave;

        public PlayerSave() { }

    }
}
