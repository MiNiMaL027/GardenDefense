using Controllers;
using Godot;
using System;

public partial class GameInstance : Node
{
    public static Hud Hud { get; set; }
    public static World World { get; set; }
    public static GameInstance Instance { get; set; }
    public override void _Ready()
	{
        Instance = this;
    }
    public void RemoveChildren()
    {
        Godot.Collections.Array<Node> children = this.GetChildren();

        foreach (var child in children)
        {
            child.QueueFree();
        }
    }
    public void StartNewGame()
    {
        ///remove all content first
        RemoveChildren();

        ///load controller and world
        PlayerController playerController = Scenes.Controllers.PlayerController();
        World home = Scenes.Worlds.Garden();

        ///insert controller into world and add the world to tree
        Node3D playerStart = home.GetNode<Node3D>("PlayerStart");

        AddChild(home);
        World = home;

        home.AddChild(playerController);

        playerController.GlobalTransform = playerStart.GlobalTransform;
        playerController.MaxMapExtent = new Vector3(World.MaxMapExtent.X, float.MaxValue, World.MaxMapExtent.Y);
        playerController.MinMapExtent = new Vector3(World.MinMapExtent.X, float.MinValue, World.MinMapExtent.Y);
    }
}
