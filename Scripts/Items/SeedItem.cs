using Enums;
using Godot;

namespace Items
{
    public class SeedItem 
    {
        public int SeedId { get; set; }

        public SeedType SeedType { get; set; }

        public int Count { get; set; }

        public Texture2D Icon { get; set; }
    }
}
