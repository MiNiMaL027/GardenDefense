using Godot;
using System;

namespace Widgets.Bestiary
{
    public partial class BestiaryWindow : Control
    {
        TextureButton ButtonClose;
        public override void _Ready()
        {
            ButtonClose = GetNode<TextureButton>("HBoxContainer/PanelContainer/HBoxContainer/Spacer/ButtonClose");
            ButtonClose.Pressed += ButtonClose_Pressed;
        }

        private void ButtonClose_Pressed()
        {
            Hud hud = this.GetHud();
            hud.CloseBestiary();
        }
    }
}

