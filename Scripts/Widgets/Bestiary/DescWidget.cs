using System;
using Godot;

namespace Widgets.Bestiary
{
    public class DefaultDescWidgetInitializer
    {
        public string Name;
        public string Description;
        public Texture2D texture2D;
    }
    public partial class DescWidget: PanelContainer
    {
        public TextureRect TextureRect;
        public Label LabelName;
        public Label LabelDescription;

        public BestiaryCategoryData bestiaryCategoryData;
        public int DescribedId;
        public override void _Ready()
        {
            TextureRect = GetNode<TextureRect>("VBoxContainer/HBoxContainer/TextureRect");
            LabelName = GetNode<Label>("VBoxContainer/HBoxContainer/LabelName");
            LabelDescription = GetNode<Label>("VBoxContainer/MarginContainer/VBoxContainer/LabelDescription");
        }
        public virtual void Init(object o)
        {
            if(o is DefaultDescWidgetInitializer defaultDescWidgetInitializer)
            {
                LabelName.Text= defaultDescWidgetInitializer.Name;
                LabelDescription.Text= defaultDescWidgetInitializer.Description;
                TextureRect.Texture = defaultDescWidgetInitializer.texture2D;
            }
        }
    }
}
