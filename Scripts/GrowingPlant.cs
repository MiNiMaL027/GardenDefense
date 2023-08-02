using Controllers;
using Godot;
using Interfaces;
using Items;
using System;
using System.Collections.Generic;

public partial class GrowingPlant : StaticBody3D, IPressable
{
    public SeedDatabaseRow SeedData;
    public PlantSocket PlantSocket;
    public PlantsToolTip PlantToolTip;
    public Sprite3D InfoSprite;
    private List<Node> notVisualNodes;
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
            this.InitVisual(ResourceLoader.Load<PackedScene>(directoryPath + $"/Stage{currentStage}.tscn"), notVisualNodes);
            if(CurrentStage == SeedData.StagesAmount) { return; }
            SetWatered(watered, true);
            Timer.Start();
        }
    }
    public void SetWatered(bool wateredToSet, bool stageChanged = false)
    {
        if (stageChanged)
        {
            if(wateredToSet == true)
            {
                Timer.WaitTime = rnd.Next(SeedData.MinSecondsToChangeState, SeedData.MaxSecondsToChangeState + 1);
                if(watered == false) //means pot became watered so disable texture
                {
                    InfoSprite.Texture = null;
                }
            }
            else
            {
                Timer.WaitTime = rnd.Next(2 * SeedData.MinSecondsToChangeState, 2 * SeedData.MaxSecondsToChangeState + 1);
                if (watered == true) //means pot now wants water so enable texture
                {
                    InfoSprite.Texture = ResourceLoader.Load<Texture2D>("res://raw assets/Images/Info/NeedWater.png");
                }
            }
            watered = wateredToSet;
        }
        else
        {

            if(watered == true && wateredToSet == false) //enable water need texture and increase time
            {
                watered = false;
                double timeLeft = Timer.TimeLeft;
                Timer.Stop();
                Timer.WaitTime = timeLeft * 2;
                InfoSprite.Texture = ResourceLoader.Load<Texture2D>("res://raw assets/Images/Info/NeedWater.png");
                Timer.Start();
            }
            else if(watered == false && wateredToSet == true)
            {
                watered= true;
                double timeLeft = Timer.TimeLeft;
                Timer.Stop();
                Timer.WaitTime = timeLeft / 2;
                InfoSprite.Texture = null;
                Timer.Start();
            }
        }
    }
    private int currentStage;
    private DateTime dateTimeStageBegin;
    private Timer Timer;
    private Random rnd;
    private bool watered = false;
    private int availableCrop = 0;
    public bool Harvestable = false;
    public void Init(Seed seed)
    {
        SeedData = DbService.GetItem(seed.Id) as SeedDatabaseRow;


        InfoSprite.Texture = ResourceLoader.Load<Texture2D>("res://raw assets/Images/Info/NeedWater.png");
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

        notVisualNodes = new List<Node>()
        {
            InfoSprite, Timer
        };

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
            InfoSprite.Texture = ResourceLoader.Load<Texture2D>("res://raw assets/Images/Info/GrewUp.png");
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
}
