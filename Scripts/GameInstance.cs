using Controllers;
using Godot;
using SaveModels;
using System;
using System.Collections.Generic;

public partial class GameInstance : Node
{
    public static Hud Hud { get; set; }
    public static World World { get; set; }
    public static GameInstance Instance { get; set; }
    public static PlayerController PlayerController { get; set; }
    public static GameSave GameSave { get; set; }
    private static SettingsSave settingsSave;
    public static SettingsSave SettingsSave
    {
        get
        {
            if(settingsSave == null)
            {
                settingsSave = SettingsSave.LoadFromFile();
                if (settingsSave == null)
                {
                    settingsSave = new SettingsSave();
                }
                else
                {
                    settingsSave.ApplySettings();
                }
            }
            return settingsSave;
        }
    }



    public override void _Ready()
	{
        GameSave = GameSave.LoadFromFile();
        Instance = this;
    }
    public void StartNewGame()
    {
        GameSave.DeleteSave();
        GameSave = null;
        ///remove all content first
        this.RemoveChildren();

        ///load controller and world
        PlayerController playerController = Scenes.Controllers.PlayerController();
        World farm = Scenes.Worlds.Farm();

        ///insert controller into world and add the world to tree
        Node3D playerStart = farm.GetNode<Node3D>("PlayerStart");

        AddChild(farm);
        World = farm;

        farm.AddChild(playerController);

        playerController.GlobalTransform = playerStart.GlobalTransform;
        playerController.MaxMapExtent = new Vector3(World.MaxMapExtent.X, float.MaxValue, World.MaxMapExtent.Y);
        playerController.MinMapExtent = new Vector3(World.MinMapExtent.X, float.MinValue, World.MinMapExtent.Y);
        PlayerController = playerController;
        PlayerController.NewGameInit();
        World.WorldEnteredListener(playerController);


    }
    public void ChangeWorld(World world)
    {
        World.WorldExitedListener(PlayerController);
        if(World is Farm currentFarm)
        {
            GameInstance.Instance.SaveGame();
        }
        World.RemoveChild(PlayerController);
        this.RemoveChildren();

        AddChild(world);
        World = world;
        if(world is Farm newFarm)
        {
            FarmSave farmSave = GameSave?.FarmSave;
            if(farmSave != null)
            {
                newFarm.LoadFromSave(farmSave);
            }
        }
        Node3D playerStart = world.GetNode<Node3D>("PlayerStart");

        world.AddChild(PlayerController);
        PlayerController.GlobalTransform = playerStart.GlobalTransform;
        PlayerController.MaxMapExtent = new Vector3(World.MaxMapExtent.X, float.MaxValue, World.MaxMapExtent.Y);
        PlayerController.MinMapExtent = new Vector3(World.MinMapExtent.X, float.MinValue, World.MinMapExtent.Y);
        world.WorldEnteredListener(PlayerController);


        //world.AddChild(ResourceLoader.Load<PackedScene>("res://Scenes/Worlds/Options/day_or_nigh_core.tscn").Instantiate<WorldEnvironment>());
    }
    public void SaveGame()
    {
        if (GameSave == null)
        {
            GameSave = new GameSave();
        }
        FarmSave farmSave = null;
        if(World is Farm f)
        {
            farmSave = f.GetFarmSave();
        }
        PlayerSave playerSave = PlayerController.GetPlayerSave();
        GameSave.PlayerSave = playerSave;
        if(farmSave != null)
        {
            GameSave.FarmSave = farmSave;
        }
        GameSave.SaveToFile();
    }
    public void ResumeGame()
    {
        ///remove all content first
        this.RemoveChildren();
        PlayerController playerController = Scenes.Controllers.PlayerController();
        Farm farm = Scenes.Worlds.Farm();

        ///insert controller into world and add the world to tree
        Node3D playerStart = farm.GetNode<Node3D>("PlayerStart");

        AddChild(farm);
        World = farm;
        FarmSave farmSave = GameSave?.FarmSave;
        if (farmSave != null)
        {
            farm.LoadFromSave(farmSave);
        }
        farm.AddChild(playerController);

        playerController.GlobalTransform = playerStart.GlobalTransform;
        playerController.MaxMapExtent = new Vector3(World.MaxMapExtent.X, float.MaxValue, World.MaxMapExtent.Y);
        playerController.MinMapExtent = new Vector3(World.MinMapExtent.X, float.MinValue, World.MinMapExtent.Y);
        PlayerController = playerController;
        World.WorldEnteredListener(playerController);
        if (GameSave?.PlayerSave != null)
        {
            playerController.LoadFromSave(GameSave.PlayerSave);
        }
        else
        {
            playerController.NewGameInit();
        }

    }
}
