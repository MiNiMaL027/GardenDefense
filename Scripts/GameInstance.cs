using Controllers;
using Godot;
using System;

public partial class GameInstance : Node
{
    public static Hud Hud { get; set; }
    public static World World { get; set; }
    public static GameInstance Instance { get; set; }
    public static PlayerController PlayerController { get; set; }
    public override void _Ready()
	{
        Instance = this;
    }
    public void StartNewGame()
    {
        ///remove all content first
        this.RemoveChildren();

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
        PlayerController = playerController;
        UpdateHud();

    }
    public void ChangeWorld(World world)
    {
        World.RemoveChild(PlayerController);
        this.RemoveChildren();

        AddChild(world);
        World = world;
        Node3D playerStart = world.GetNode<Node3D>("PlayerStart");

        world.AddChild(PlayerController);
        PlayerController.GlobalTransform = playerStart.GlobalTransform;
        PlayerController.MaxMapExtent = new Vector3(World.MaxMapExtent.X, float.MaxValue, World.MaxMapExtent.Y);
        PlayerController.MinMapExtent = new Vector3(World.MinMapExtent.X, float.MinValue, World.MinMapExtent.Y);
        UpdateHud();
    }
    private void UpdateHud()
    {
        Hud = PlayerController.GetNode<Hud>("Hud");
        if (GameInstance.World is Farm)
        {
            Hud.DisplayGardenWidget(PlayerController);
        }
        else if (GameInstance.World is Battlefield bf)
        {
            Hud.DisplayBattlefieldWidget(PlayerController);
            bf.WorldTimer = Hud.BattlefieldWidget.WorldTimer;
        }
    }
}
