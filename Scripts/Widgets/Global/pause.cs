using Godot;
using SaveModels;
using System;
namespace Widgets.Global
{
    public partial class pause : Panel
    {
        private Button optionButton;
        private Button buttonExitGame;


        private TextureButton closeButton;
        public override void _Ready()
        {
            optionButton = GetNode<Button>("MarginContainer/VBoxContainer/OptionButton");
            closeButton = GetNode<TextureButton>("CloseButton");
            buttonExitGame = GetNode<Button>("MarginContainer/VBoxContainer/ButtonExitGame");
            buttonExitGame.Pressed += ButtonExitGame_Pressed;

            optionButton.Pressed += OptionButton_Pressed;
            closeButton.Pressed += CloseButton_Pressed;
        }

        private void ButtonExitGame_Pressed()
        {
            GameInstance.Instance.SaveGame();
            GetTree().Quit();
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

}
