using System;
using Godot;
using Widgets.Bestiary;

namespace Widgets.Buttons
{
    public partial class BestiaryCategoryButton:Button
    {
        [Signal]
        public delegate void CategoryClickedEventHandler(BestiaryCategoryButton sender);
        public BestiaryCategoryData bestiaryCategoryData;

        public override void _Ready()
        {
            Pressed += OnButtonPressed;
        }

        private void OnButtonPressed()
        {
            EmitSignal(SignalName.CategoryClicked, this);
        }
    }
}
