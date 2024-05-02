using Godot;
using System;
namespace Widgets.BattleAgent
{
    public partial class BattleAgentWindow : Control
    {
        Button CloseButton { get; set; }
        public override void _Ready()
        {
            CloseButton = GetNode<Button>("PanelContainer/TextureButton");
            CloseButton.Pressed += CloseButton_Pressed;
        }

        private void CloseButton_Pressed()
        {
            QueueFree();
        }
    }
}

