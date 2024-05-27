using Godot;
using System.Collections.Generic;
using System.Linq;

namespace Widgets.BattleAgent
{
    public partial class BattleAgentWindow : Control
    {
        Button CloseButton { get; set; }
        Control LvlContainer { get; set; }
        List<LvlButton> LvlButtons = new List<LvlButton>();
        public override void _Ready()
        {
            CloseButton = GetNode<Button>("PanelContainer/TextureButton");
            LvlContainer = GetNode<Control>("PanelContainer/MarginContainer/VBoxContainer/ScrollContainer/lvlButtonContainer");

            CloseButton.Pressed += CloseButton_Pressed;

            Init();
        }

        private void CloseButton_Pressed()
        {
            QueueFree();
        }

        private void Init()
        {
            LvlButtons.Clear();
            var currentLvl = this.GetPlayerController().currentLvl;

            foreach(var child in LvlContainer.GetChildren())
            {
                var button = child as LvlButton;
                LvlButtons.Add(button);
                if (button.LvlNumber < currentLvl)
                    button.Init(Enums.LvlButtonState.Completed);
                else if (button.LvlNumber == currentLvl)
                    button.Init(Enums.LvlButtonState.Active);
                else if (button.LvlNumber > currentLvl)
                    button.Init(Enums.LvlButtonState.Disabled);
            }
        }
    }
}

