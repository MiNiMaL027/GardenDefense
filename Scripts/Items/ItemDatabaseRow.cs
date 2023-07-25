using Enums;
namespace Items
{
    public class ItemDatabaseRow
    {
        public int Id;
        public string ItemName;
        public string Description;
        public int BuyPrice;
        public int SellPrice;
        public ItemType ItemType;
        public string TextureSpritePath;
        public string MeshPath;
    }
}
