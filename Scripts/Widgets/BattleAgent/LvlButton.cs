using Godot;
using System;

public partial class LvlButton : Button
{
	[Export]
	public int LvlNumber { get; set; }
    [Export]
    public int SlotsCount { get; set; }
	public override void _Ready()
	{
		Text = LvlNumber.ToString();
        Pressed += LvlButton_Pressed;
	}

    private void LvlButton_Pressed()
    {
        var transferWindow = Scenes.Widgets.PlantTransfer.PlantTransferWindow();
        transferWindow.Init(LvlNumber, SlotsCount);
        GetParent().AddChild(transferWindow);
    }
}
