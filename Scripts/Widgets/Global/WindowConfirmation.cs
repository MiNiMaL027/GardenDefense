using Godot;
using System;
namespace Widgets.Global
{
    public partial class WindowConfirmation : ColorRect
    {
        [Signal]
        public delegate void ConfirmEventHandler();
        [Signal]
        public delegate void CancelEventHandler();
        public Label LabelText { get; set; }
        Button ButtonConfirm { get; set; }
        Button ButtonCancel { get; set; }

        public override void _Ready()
        {
            LabelText = GetNode<Label>("PanelContainer/MarginContainer/VBoxContainer/LabelText");

            ButtonConfirm = GetNode<Button>("PanelContainer/MarginContainer/VBoxContainer/HBoxContainer/ButtonConfirm");
            ButtonConfirm.Connect("pressed", new Callable(this, nameof(buttonConfirmPressed)));
            ButtonCancel = GetNode<Button>("PanelContainer/MarginContainer/VBoxContainer/HBoxContainer/ButtonCancel");
            ButtonCancel.Connect("pressed", new Callable(this, nameof(buttonCancelPressed)));
        }
        private void buttonConfirmPressed()
        {
            EmitSignal(SignalName.Confirm);

            QueueFree();
        }
        private void buttonCancelPressed()
        {
            EmitSignal(SignalName.Cancel);

            QueueFree();
        }

        internal void Init(string message)
        {
            LabelText.Text = message;
            LabelText.Visible = false;
            LabelText.Visible = true;

            if (LabelText.Size.X > 500)
            {
                LabelText.CustomMinimumSize = new Vector2(500, LabelText.Size.Y);
                LabelText.AutowrapMode = TextServer.AutowrapMode.Word;
            }
        }
    }

}
