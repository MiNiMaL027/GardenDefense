using Godot;
using System;
namespace Widgets.Global
{
    public partial class main_menu : Control
    {
        public Button NewGameButton { get; set; }
        public Button ContinueButton { get; set; }
        public Button OptionButton { get; set; }
        public Button ExitButton { get; set; }
        public override void _Ready()
        {
            NewGameButton = GetNode<Button>("MainMenuWidget/MarginContainer/VBoxContainer/NewGameButton");
            ContinueButton = GetNode<Button>("MainMenuWidget/MarginContainer/VBoxContainer/ContinueButton");
            OptionButton = GetNode<Button>("OptionButton");
            ExitButton = GetNode<Button>("MainMenuWidget/MarginContainer/VBoxContainer/ExitButton");

            NewGameButton.Pressed += NewGameButton_Pressed;

            ContinueButton.Pressed += ContinueButton_Pressed;

            OptionButton.Pressed += OptionButton_Pressed;
            ExitButton.Pressed += ExitButton_Pressed;
        }

        private void ExitButton_Pressed()
        {
            GetTree().Quit();
        }

        private void OptionButton_Pressed()
        {
            var options = Scenes.Widgets.OptionPanel();
            AddChild(options);
        }

        private void ContinueButton_Pressed()
        {
            GameInstance.Instance.ResumeGame();
            QueueFree();

        }

        private void NewGameButton_Pressed()
        {
            GameInstance.Instance.StartNewGame();
            QueueFree();
        }
    }
}
