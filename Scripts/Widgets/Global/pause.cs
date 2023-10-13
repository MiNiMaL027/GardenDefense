using Godot;
using System;

public partial class pause : Panel
{
	private Button optionButton;

    private TextureButton closeButton;
	public override void _Ready()
	{
		optionButton = GetNode<Button>("MarginContainer/VBoxContainer/OptionButton");
        closeButton = GetNode<TextureButton>("CloseButton");

        optionButton.Pressed += OptionButton_Pressed;
        closeButton.Pressed += CloseButton_Pressed;
	}

    private void CloseButton_Pressed()
    {
        GetTree().Paused = false;
        GameInstance.World.AddEffect(false);
        QueueFree();
    }

    private void OptionButton_Pressed()
    {
        var optionPanel = Scenes.Widgets.OptionPanel();
        this.GetPlayerController().Hud.AddChild(optionPanel);
    }
}
