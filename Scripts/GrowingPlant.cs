using Controllers;
using Enums;
using Godot;
using Interfaces;
using Items;
using System;
using System.Collections.Generic;

public partial class GrowingPlant : StaticBody3D, IPressable, IHoverable
{
    public SeedDatabaseRow SeedData;
    public PlantSocket PlantSocket;
    public Sprite3D InfoSprite;
    private List<Node> notVisualNodes;
    private GrowingPlantTooltip tooltip;
    private int numberOfSeedReturns = 0;

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
                if(CurrentStage < SeedData.StagesAmount)
                {
                    Timer.Stop();
                    Timer.WaitTime = timeLeft * 2;
                    InfoSprite.Texture = ResourceLoader.Load<Texture2D>("res://raw assets/Images/Info/NeedWater.png");
                    Timer.Start();
                }
            }
            else if(watered == false && wateredToSet == true)
            {
                watered= true;
                
                if(CurrentStage < SeedData.StagesAmount)
                {
                    double timeLeft = Timer.TimeLeft;
                    Timer.Stop();
                    Timer.WaitTime = timeLeft / 2;
                    InfoSprite.Texture = null;
                    Timer.Start();
                }
            }
        }
    }
    private int currentStage;
    private DateTime dateTimeStageBegin;
    private Timer Timer;
    private Random rnd;
    private bool watered = false;
    public int availableCrop = 0;
    public int cropModifier = 1;
    public bool Harvestable = false;
    public void Init(Seed seed)
    {
        SeedData = DbService.GetItem(seed.Id) as SeedDatabaseRow;

        var parentPot = GetParent().GetParent<Pot>();

        if (parentPot.Fertilizer != null)
        {
            switch (parentPot.Fertilizer.FertilizerType)
            {
                case FertilizerType.enlarge:
                    cropModifier = 2;
                    break;
                case FertilizerType.speed:
                    SeedData.MinSecondsToChangeState /= 2;
                    SeedData.MaxSecondsToChangeState /= 2;
                    break;
                case FertilizerType.returning:
                    numberOfSeedReturns = RandomDropCountSeed();
                    break;
            }
        }

        InfoSprite.Texture = ResourceLoader.Load<Texture2D>("res://raw assets/Images/Info/NeedWater.png");

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
    }

    private void Timer_Timeout()
    {
        CurrentStage++;
        if(CurrentStage == SeedData.StagesAmount)
        {
            availableCrop = new Random().Next(SeedData.MinCropAmount,SeedData.MaxCropAmount + 1)*cropModifier;
            Harvestable= true;
            InfoSprite.Texture = ResourceLoader.Load<Texture2D>("res://raw assets/Images/Info/GrewUp.png");
        }
        tooltip?.RefreshBar(CurrentStage);
    }

    public void LeftMouseDownListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
    {
        TryHarvest(playerController);
    }

    public void LeftMouseUpListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
    {
        
    }
    public void RightMouseDownListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
    {
    }
    public void ShowTooltip()
    {
        tooltip = Scenes.Widgets.ToolTip.GrowingPlantTooltip();
        PlayerController playerController = this.GetPlayerController();
        playerController.Hud.AddChild(tooltip);
        tooltip.ShowTooltip(this);
        playerController.Hud.AddAtMousePosition(tooltip);
    }

    public void HideTooltip()
    {
        if(tooltip!= null)
        {
            tooltip.HideTooltip();
            tooltip = null;
        }
        
    }

    public void MouseEnter()
    {
        ShowTooltip();
    }

    public void MouseLeave()
    {       
        HideTooltip();
    }

    private int RandomDropCountSeed()
    {
        Random rnd = new Random();
        var chance = rnd.Next(0, 100);

        if (chance < 10)
            return 3;
        else if (chance < 40)
            return 2;
        else if (chance < 75)
            return 1;

        return 0;
    }

    public void TryHarvest(PlayerController playerController)
    {
        if (Harvestable)
        {
            Node parent = playerController.GetParent();
            for (int i = 0; i < availableCrop; i++)
            {
                Item item = Scenes.Items.Item();
                parent.AddChild(item);
                GD.Print(SeedData.GrowUpId);
                item.InitializeItem(SeedData.GrowUpId);
                item.GlobalPosition = GlobalPosition;
                item.LinearVelocity = Vector3.Up;
            }

            if (numberOfSeedReturns > 0)
            {
                for (int i = 0; i < numberOfSeedReturns; i++)
                {
                    Seed seed = Scenes.Items.Seed();
                    parent.AddChild(seed);
                    seed.InitializeItem(SeedData.Id);
                    seed.GlobalPosition = GlobalPosition;
                    seed.LinearVelocity = Vector3.Up;
                }
            }

            PlantSocket.IsUsed = false;
            this.QueueFree();
        }
    }
}
