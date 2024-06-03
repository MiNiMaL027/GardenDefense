using Godot;
using Items;
namespace Widgets.ToolTip
{
    public partial class ItemTooltip : BaseTooltip
    {
        Label LabelItemName;
        TextureRect TextureRect;
        public override void _Ready()
        {
            base._Ready();

            LabelItemName = GetNode<Label>("MarginContainer/VBoxContainer/LabelItemName");
            TextureRect = GetNode<TextureRect>("MarginContainer/VBoxContainer/TextureRect");
        }
        public virtual void ShowTooltipDbRow(ItemDatabaseRow itemDatabaseRow)
        {
            LabelItemName.Text = itemDatabaseRow.ItemName;
            TextureRect.Texture = ResourceLoader.Load<Texture2D>(itemDatabaseRow.TextureSpritePath);
        }
        public virtual void ShowTooltip(Item item)
        {
            LabelItemName.Text = item.ItemName;
            TextureRect.Texture = ResourceLoader.Load<Texture2D>(item.TextureSpritePath);
        }
    }
}
