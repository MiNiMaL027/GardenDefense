using Godot;
using Items;
using System;
using static ItemsId.ItemId;

public partial class GrowingPlant : StaticBody3D
{
    public SeedDatabaseRow SeedData;
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
            this.InitVisual(ResourceLoader.Load<PackedScene>(directoryPath + $"/Stage{currentStage}.tscn"));
            if(CurrentStage == SeedData.StagesAmount) { return; }
            if (GetParent<Pot>().Watered)
            {
                watered = true;
                Timer.WaitTime = rnd.Next(SeedData.MinSecondsToChangeState, SeedData.MaxSecondsToChangeState + 1);
            }
            else
            {
                watered = false;
                Timer.WaitTime = rnd.Next(2 * SeedData.MinSecondsToChangeState, 2 * SeedData.MaxSecondsToChangeState + 1);
            }
            GD.Print("Timer.Start");
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
                watered= true;
                double timeLeft = Timer.TimeLeft;
                Timer.Stop();
                Timer.WaitTime = timeLeft / 2;
                Timer.Start();
            }
            else if(watered == true && value == false)
            {
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
        CurrentStage = 1;
    }
    public override void _Ready()
    {
        Timer = GetNode<Timer>("Timer");
        Timer.Timeout += Timer_Timeout;
        rnd = new Random();
    }

    private void Timer_Timeout()
    {
        GD.Print("Timer_Timeout");
        CurrentStage++;
        if(CurrentStage == SeedData.StagesAmount)
        {
            availableCrop = 1;
            Harvestable= true;
        }
    }
}
