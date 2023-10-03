using Godot;
using System;
using System.ComponentModel;

public partial class AcceptWindow : Panel
{
	public Label ContextLabel { get; set; }

	public Button YesButton { get; set; }
	public Button NoButton { get; set; }

    [Signal]
    public delegate void ButtonPressedYesEventHandler();

    public override void _Ready()
	{
		ContextLabel = GetNode<Label>("Panel/VBoxContainer/Context");

		YesButton = GetNode<Button>("Panel/VBoxContainer/HBoxContainer/Yes");
        NoButton = GetNode<Button>("Panel/VBoxContainer/HBoxContainer/No");

        YesButton.Pressed += YesButton_Pressed;
        NoButton.Pressed += NoButton_Pressed;
	}

    private void NoButton_Pressed()
    {
        QueueFree();
    }

    private void YesButton_Pressed()
    {
        EmitSignal(SignalName.ButtonPressedYes);
        QueueFree();
    }

    public void Init(string context, ButtonPressedYesEventHandler action)
    {
        ButtonPressedYes += action;
        ContextLabel.Text = context;
    }

    public override void _GuiInput(InputEvent @event)
    {
        base._GuiInput(@event);

        if (@event is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == MouseButton.Left && mouseButton.Pressed || @event is InputEventMouseButton mouseButtonLeft && mouseButtonLeft.ButtonIndex == MouseButton.Right && mouseButtonLeft.Pressed)
        {
            QueueFree();
        }
    }

}
