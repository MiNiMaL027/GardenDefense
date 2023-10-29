using Enums;
using Godot;
using System;
namespace Widgets.Buttons
{
    public partial class CategoriesButton : Button
    {
        [Export]
        public ItemType ItemType { get; set; }

        public event EventHandler<ButtonEventData> ButtonClicked;

        public override void _Ready()
        {
            Pressed += OnButtonPressed;
        }

        private void OnButtonPressed()
        {
            ButtonClicked?.Invoke(this, new ButtonEventData(ItemType));
        }
    }

}