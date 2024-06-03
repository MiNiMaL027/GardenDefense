using Godot;

public partial class ClassIcon : PanelContainer
{
	TextureRect TextureRect { get; set; }
	Texture2D Texture2D { get; set; }
	string description { get; set; }
	ClassIconTooltip tooltip { get; set; }
	public override void _Ready()
	{
		TextureRect = GetNode<TextureRect>("TextureRect");
	}

	public void Init(Texture2D texture, string text)
	{
        TextureRect.Texture = texture;
		Texture2D = texture;
		description = text;
        MouseEntered += ClassIcon_MouseEntered;
        MouseExited += ClassIcon_MouseExited;
	}

    private void ClassIcon_MouseExited()
    {
        if (tooltip != null)
        {
            tooltip.HideTooltip();

            tooltip = null;
        }
    }

    private void ClassIcon_MouseEntered()
    {
        tooltip = Scenes.Widgets.ToolTip.ClassIconTooltip();
        Vector2 globalMousePosition = GetViewport().GetMousePosition();

        AddChild(tooltip);

        tooltip.ShowTooltip(Texture2D, description);
        tooltip.AdjustControlInViewport(globalMousePosition);
        tooltip.PostInit();         
    }
}
