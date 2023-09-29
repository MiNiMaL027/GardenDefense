using Godot;

public partial class InfoPanel : Panel
{
	Label InfoLabel { get; set; }

    TextureRect Icon { get; set; }
	TextureButton CloseButton { get; set; }

    public int TimeToDelete = 5;

    public override void _Ready()
	{
		InfoLabel = GetNode<Label>("HBoxContainer/Label");
        Icon = GetNode<TextureRect>("HBoxContainer/TextureRect");
		CloseButton = GetNode<TextureButton>("HBoxContainer/Button");

        CloseButton.Pressed += CloseButton_Pressed;

        #region Timer

        Timer timer = new Timer();
        timer.Autostart = true;
        timer.WaitTime = TimeToDelete;
        timer.OneShot = true;
        AddChild(timer);

        timer.Timeout += CloseButton_Pressed;

        #endregion
    }

    private void CloseButton_Pressed()
    {
		QueueFree();
    }

    public void AddText(string text)
	{
		InfoLabel.Text = text;
	}

    public void AddTexture( string texturePath)
    {
        Icon.Texture = GD.Load<Texture2D>(texturePath);
    }
}
