using System;
using Godot;
namespace Widgets.Bestiary
{
    public partial class BestiaryListItem:PanelContainer
    {
        [Signal]
        public delegate void ItemSelectedEventHandler(BestiaryListItem sender);
        public TextureRect TextureRect;
        public Label LabelName;
        public int ItemId;
        public static StyleBoxFlat SelectedItemStyle;
        public static StyleBoxFlat DefaultItemStyle;

        static BestiaryListItem()
        {
            SelectedItemStyle = GD.Load<StyleBoxFlat>("res://Styles/Widgets/Bestiary/BestiaryListItemSelected.tres");
            DefaultItemStyle = GD.Load<StyleBoxFlat>("res://Styles/Widgets/Bestiary/BestiaryListItemDefault.tres");

        }
        public override void _Ready()
        {
            LabelName=GetNode<Label>("HBoxContainer/LabelName");
            TextureRect = GetNode<TextureRect>("HBoxContainer/TextureRect");
        }
        public void Init(string name, Texture2D texture2D, int itemId)
        {
            TextureRect.Texture= texture2D;
            LabelName.Text= name;
            ItemId= itemId;
        }
        public override void _GuiInput(InputEvent e)
        {
            base._GuiInput(e);
            if(e is InputEventMouseButton mouseButtonEvent && mouseButtonEvent.ButtonIndex == MouseButton.Left && mouseButtonEvent.Pressed == true)
            {
                EmitSignal(SignalName.ItemSelected, this);
            }
        }
    }
}
