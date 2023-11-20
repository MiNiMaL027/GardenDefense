using Enums;
using Godot;
using System;

namespace Widgets.Buttons
{
    public partial class OrderButton : Button
    {
        [Export]
        public OrderType OrderType { get; set; }
        public bool ForLow;

        public event EventHandler<ButtonEventData> ButtonClicked;

        public override void _Ready()
        {
            Pressed += OnButtonPressed;

            ExpandIcon = true;
        }

        private void OnButtonPressed()
        {
            ButtonClicked?.Invoke(this, new ButtonEventData(OrderType, ForLow));

            if (ForLow)
                Icon = ResourceLoader.Load<Texture2D>("res://raw assets/Images/Hud/Inventory/ArrowUp.png");
            else
                Icon = ResourceLoader.Load<Texture2D>("res://raw assets/Images/Hud/Inventory/ArrowDown.png");
            ForLow = !ForLow;
                       
        }
    }
}
