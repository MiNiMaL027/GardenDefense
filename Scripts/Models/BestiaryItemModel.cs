using Godot;

namespace Farm.Scripts.Models
{
    public class BestiaryItemModel
    {
        public Texture2D Texture { get; set; }
        public string Name { get; set; }

        public BestiaryItemModel(string name, Texture2D texture)
        {
            Name = name;
            Texture = texture;
        }
    }
}
