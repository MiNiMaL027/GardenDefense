using Godot;
using Widgets.ToolTip;

public partial class ClassIconTooltip : BaseTooltip
{
    Label LabelItemName;
    TextureRect TextureRect;
    public override void _Ready()
    {       
        LabelItemName = GetNode<Label>("MarginContainer/VBoxContainer/LabelItemName");
        TextureRect = GetNode<TextureRect>("MarginContainer/VBoxContainer/TextureRect");
        timeToView = 0.1;

        base._Ready();
    }
    public virtual void ShowTooltip(Texture2D texture, string description)
    {
        LabelItemName.Text = description;
        TextureRect.Texture = texture;
    }
}
