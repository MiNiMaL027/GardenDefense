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
        private CheckButton safeSellingCheck;
        private CheckButton sunMoving;

        private TextureButton CloseButton;
        public override void _Ready()
        {
            soundCurrentValueLabel = GetNode<Label>("MarginContainer/VBoxContainer/HBoxContainer/SoundPanel/SoundContainer/HBoxContainer/CurrentVolume");
            musicCurrentValueLabel = GetNode<Label>("MarginContainer/VBoxContainer/HBoxContainer/MusicPanel/MusicContainer/HBoxContainer/CurrentVolume");

            soundCurrentValueLabel.Text = ((GameInstance.SettingsSave.SoundVolume - -25) * (100 - 0) / (0 - -25) + 0).ToString() + "%";
            musicCurrentValueLabel.Text = ((GameInstance.SettingsSave.MusicVolume - -25) * (100 - 0) / (0 - -25) + 0).ToString() + "%";

            soundSlider = GetNode<HSlider>("MarginContainer/VBoxContainer/HBoxContainer/SoundPanel/SoundContainer/HSlider");
            musicSlider = GetNode<HSlider>("MarginContainer/VBoxContainer/HBoxContainer/MusicPanel/MusicContainer/HSlider");

            soundSlider.ValueChanged += SoundSlider_ValueChanged;
            musicSlider.ValueChanged += MusicSlider_ValueChanged;
            soundSlider.Value = GameInstance.SettingsSave.SoundVolume;
            musicSlider.Value = GameInstance.SettingsSave.MusicVolume;

            infoPanelCheck = GetNode<CheckButton>("MarginContainer/VBoxContainer/HBoxContainer2/VBoxContainer/InfoPanelPAnel/CheckButton");
            infoPanelCheck.Pressed += InfoPanelCheck_Pressed;
            infoPanelCheck.ButtonPressed = GameInstance.SettingsSave.InfoPanel;

            safeSellingCheck = GetNode<CheckButton>("MarginContainer/VBoxContainer/HBoxContainer2/VBoxContainer2/SafeSellingPanel/CheckButton");
            safeSellingCheck.Pressed += SafeSellingCheck_Pressed;
            safeSellingCheck.ButtonPressed = GameInstance.SettingsSave.SafeSelling;

            sunMoving = GetNode<CheckButton>("MarginContainer/VBoxContainer/HBoxContainer2/VBoxContainer/DayOrNight/CheckButton");
            sunMoving.Pressed += SunMoving_Pressed;
            sunMoving.ButtonPressed = GameInstance.SettingsSave.NightOrDayCore;

            CloseButton = GetNode<TextureButton>("CloseButton");
            CloseButton.Pressed += CloseButton_Pressed;
        }

        private void SunMoving_Pressed()
        {
            GameInstance.SettingsSave.NightOrDayCore = !GameInstance.SettingsSave.NightOrDayCore;
        }

        private void SafeSellingCheck_Pressed()
        {
            GameInstance.SettingsSave.SafeSelling = !GameInstance.SettingsSave.SafeSelling;
        }

        private void InfoPanelCheck_Pressed()
        {
            GameInstance.SettingsSave.InfoPanel = !GameInstance.SettingsSave.InfoPanel;            
        }

        private void CloseButton_Pressed()
        {
            GameInstance.SettingsSave.SaveToFile();
            QueueFree();
        }

        private void MusicSlider_ValueChanged(double value)
        {
            GameInstance.SettingsSave.MusicVolume = (int)value;

            musicCurrentValueLabel.Text = ((value - -25) * (100 - 0) / (0 - -25) + 0).ToString() + "%";
        }

        private void SoundSlider_ValueChanged(double value)
        {
            GameInstance.SettingsSave.SoundVolume = (int)value;

            soundCurrentValueLabel.Text = ((value - -25) * (100 - 0) / (0 - -25) + 0).ToString() + "%";
        }

    }
}
