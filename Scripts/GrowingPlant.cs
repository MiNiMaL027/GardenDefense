using Controllers;
using Godot;
using Interfaces;
using Items;
using System;
using static ItemsId.ItemId;

public partial class GrowingPlant : StaticBody3D, IPressable
{
    public SeedDatabaseRow SeedData;
    public PlantSocket PlantSocket;
    public PlantsToolTip PlantToolTip;
    public Sprite3D InfoSprite;
    public int CurrentStage
    {
        get
        {
            return currentStage;
        }
        set
        {
            currentStage = value;
            dateTimeStageBegin = DateTime.Now;
            string directoryPath = SeedData.MeshPath.Substring(0, SeedData.MeshPath.LastIndexOf('/'));
            this.InitVisual(ResourceLoader.Load<PackedScene>(directoryPath + $"/Stage{currentStage}.tscn"), Timer, InfoSprite);
            if(CurrentStage == SeedData.StagesAmount) { return; }
            if (GetParent().GetParent<Pot>().Watered)
            {              
                watered = true;
                Timer.WaitTime = rnd.Next(SeedData.MinSecondsToChangeState, SeedData.MaxSecondsToChangeState + 1);
            }
            else
            {
                ChangeInfoSpriteToWater();
                watered = false;
                Timer.WaitTime = rnd.Next(2 * SeedData.MinSecondsToChangeState, 2 * SeedData.MaxSecondsToChangeState + 1);
            }
            Timer.Start();

        }
    }
    private int currentStage;
    private DateTime dateTimeStageBegin;
    private Timer Timer;
    private Random rnd;
    private bool watered = false;
    private int availableCrop = 0;
    public bool Harvestable = false;
    public bool Watered
    {
        get { return watered; }
        set
        {
            if(value == true && watered == false)
            {
                InfoSprite.Texture = null;
                watered= true;
                double timeLeft = Timer.TimeLeft;
                Timer.Stop();
                Timer.WaitTime = timeLeft / 2;
                Timer.Start();
            }
            else if (watered == true && value == false)
            {
                ChangeInfoSpriteToWater();
                double timeLeft = Timer.TimeLeft;
                Timer.Stop();
                Timer.WaitTime = timeLeft * 2;
                Timer.Start();
            }
        }
    }
    public void Init(Seed seed)
    {
        SeedData = DbService.GetItem(seed.Id) as SeedDatabaseRow;

        PlantToolTip = Scenes.Widgets.ToolTip.PlantsToolTip();
        AddChild(PlantToolTip);
        PlantToolTip.Init(ResourceLoader.Load<Texture2D>(seed.TextureSpritePath), seed.ItemName, seed.StagesAmount);
        RemoveChild(PlantToolTip);

        CurrentStage = 1;
    }
    public override void _Ready()
    {
        Timer = GetNode<Timer>("Timer");
        InfoSprite = GetNode<Sprite3D>("InfoSprite");

        Timer.Timeout += Timer_Timeout;

        rnd = new Random();

        MouseEntered += GrowingPlant_MouseEntered;
        MouseExited += GrowingPlant_MouseExited;
    }

    private void GrowingPlant_MouseExited()
    {
        RemoveChild(PlantToolTip);
    }

    private void GrowingPlant_MouseEntered()
    {
        AddChild(PlantToolTip);
        PlantToolTip.GlobalPosition = GetViewport().GetMousePosition();
    }

    private void Timer_Timeout()
    {
        CurrentStage++;
        if(CurrentStage == SeedData.StagesAmount)
        {
            availableCrop = 1;
            Harvestable= true;
        }

        PlantToolTip.RefreshBar(CurrentStage);
    }

    public void LeftMouseDownListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
    {
        if(Harvestable)
        {
            Node parent = playerController.GetParent();
            Item item = Scenes.Items.Item();

            parent.AddChild(item);
            item.InitializeItem(SeedData.GrowUpId);

            item.GlobalPosition = GlobalPosition;
            item.LinearVelocity = Vector3.Up;
            PlantSocket.isUsed = false;

            this.QueueFree();
        }
    }

    public void LeftMouseUpListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
    {
        
    }

    private void ChangeInfoSpriteToWater()
    {
        InfoSprite.GlobalRotation = Vector3.Zero;
        InfoSprite.Texture = ResourceLoader.Load<Texture2D>("res://raw assets/Images/Info/NeedWater.png");
    }
}
