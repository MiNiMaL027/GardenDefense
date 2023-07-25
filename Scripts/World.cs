using Godot;
using System;

public partial class World : Node3D
{
    [Export]
    public Vector2 MaxMapExtent = new Vector2();
    [Export]
    public Vector2 MinMapExtent = new Vector2();
}
