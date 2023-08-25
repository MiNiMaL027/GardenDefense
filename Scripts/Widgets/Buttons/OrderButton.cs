using Enums;
using Farm.Scripts.Enums;
using Godot;
using System;

namespace Farm.Scripts.Widgets.Buttons
{
    public partial class OrderButton : Button
    {
        [Export]
        public OrderType OrderType { get; set; }

        public event EventHandler<ButtonEventData> ButtonClicked;

        public override void _Ready()
        {
            Pressed += OnButtonPressed;
        }

        private void OnButtonPressed()
        {
            ButtonClicked?.Invoke(this, new ButtonEventData(OrderType));
        }
    }
}
