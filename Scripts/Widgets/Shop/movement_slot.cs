using Godot;
using System;

public partial class movement_slot : Panel
{
	public TextureRect icon { get; set; }

    public override void _Ready()
    {
        icon = GetNode<TextureRect>("Icon");
    }

    public override void _PhysicsProcess(double delta)
    {
        MoveTo();
    }

    private void MoveTo()
    {
        this.GlobalPosition = GetViewport().GetMousePosition() - new Vector2(50,50);
    }
}
