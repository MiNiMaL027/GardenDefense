using Godot;

public partial class InfoWindow : Control
{
	public VBoxContainer InfoContainer;

	public override void _Ready()
	{		
		InfoContainer = GetNode<VBoxContainer>("VBoxContainer");
	}

	public void ClearAll()
	{
		foreach(var child in InfoContainer.GetChildren())
		{
			child.QueueFree();
		}
	}

	public void AddInfoPanel(string text)
	{
		var panel = Scenes.Widgets.infoPanel();
		InfoContainer.AddChild(panel);

		panel.AddText(text);
	}

	public void AddInfoPanel(string text, string texturePath)
	{
        var panel = Scenes.Widgets.infoPanel();
        InfoContainer.AddChild(panel);

        panel.AddText(text);
		panel.AddTexture(texturePath);
    }
}
