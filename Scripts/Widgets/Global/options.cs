using Farm.Scripts;
using Godot;
using System;
namespace Widgets.Global
{
    public partial class options : Panel
    {
        private HSlider soundSlider;
        private HSlider musicSlider;
        private Label soundCurrentValueLabel;
        private Label musicCurrentValueLabel;

        private CheckButton infoPanelCheck;

        private TextureButton CloseButton;
        public override void _Ready()
        {
            soundCurrentValueLabel = GetNode<Label>("MarginContainer/VBoxContainer/HBoxContainer/SoundPanel/SoundContainer/HBoxContainer/CurrentVolume");
            musicCurrentValueLabel = GetNode<Label>("MarginContainer/VBoxContainer/HBoxContainer/MusicPanel/MusicContainer/HBoxContainer/CurrentVolume");

            soundCurrentValueLabel.Text = ((Options.soundVolume - -25) * (100 - 0) / (0 - -25) + 0).ToString() + "%";
            musicCurrentValueLabel.Text = ((Options.musicVolume - -25) * (100 - 0) / (0 - -25) + 0).ToString() + "%";

            soundSlider = GetNode<HSlider>("MarginContainer/VBoxContainer/HBoxContainer/SoundPanel/SoundContainer/HSlider");
            musicSlider = GetNode<HSlider>("MarginContainer/VBoxContainer/HBoxContainer/MusicPanel/MusicContainer/HSlider");

            soundSlider.ValueChanged += SoundSlider_ValueChanged;
            musicSlider.ValueChanged += MusicSlider_ValueChanged;
            soundSlider.Value = Options.soundVolume;
            musicSlider.Value = Options.musicVolume;

            infoPanelCheck = GetNode<CheckButton>("MarginContainer/VBoxContainer/HBoxContainer2/VBoxContainer/InfoPanelPAnel/CheckButton");
            infoPanelCheck.Pressed += InfoPanelCheck_Pressed;
            infoPanelCheck.ButtonPressed = Options.infoPanel;

            CloseButton = GetNode<TextureButton>("CloseButton");
            CloseButton.Pressed += CloseButton_Pressed;
        }

        private void InfoPanelCheck_Pressed()
        {
            if (Options.infoPanel)
                Options.infoPanel = false;
            else
                Options.infoPanel = true;
        }

        private void CloseButton_Pressed()
        {
            QueueFree();
        }

        private void MusicSlider_ValueChanged(double value)
        {
            Options.musicVolume = (int)value;

            musicCurrentValueLabel.Text = ((value - -25) * (100 - 0) / (0 - -25) + 0).ToString() + "%";
        }

        private void SoundSlider_ValueChanged(double value)
        {
            Options.soundVolume = (int)value;

            soundCurrentValueLabel.Text = ((value - -25) * (100 - 0) / (0 - -25) + 0).ToString() + "%";
        }

    }
}
