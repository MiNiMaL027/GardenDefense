using Godot;
using System;

public partial class main_menu : Panel
{
	public Button NewGameButton { get; set; }
	public Button ContinueButton { get; set; }
	public Button OptionButton { get; set; }
	public Button ExitButton { get; set; }
	public override void _Ready()
	{
		NewGameButton = GetNode<Button>("VBoxContainer/NewGameButton");
		ContinueButton = GetNode<Button>("VBoxContainer/ContinueButton");
		OptionButton = GetNode<Button>("OptionButton");
		ExitButton = GetNode<Button>("VBoxContainer/ExitButton");

        NewGameButton.Pressed += NewGameButton_Pressed;

        ContinueButton.Pressed += ContinueButton_Pressed;

        OptionButton.Pressed += OptionButton_Pressed;
	}

    private void OptionButton_Pressed()
    {
        
    }

    private void ContinueButton_Pressed()
    {
        throw new NotImplementedException();
    }

    private void NewGameButton_Pressed()
    {
		this.GetGameInstance().StartNewGame();
    }

}
