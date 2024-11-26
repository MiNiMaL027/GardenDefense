using Controllers;
using Enums;
using Godot;
using Items;
using Pawns;
using Pawns.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using Widgets;

public partial class Battlefield : World
{
    public TowerDefenseArea TowerDefenseArea { get; set; }
    /// <summary>
    /// Becomes available when PlayerController enters this world
    /// </summary>
    public WorldTimer WorldTimer { get; set; }
    [Export]
    public PackedScene[] availableMonstersToSpawn;
    [Export]
    public Stage[] BattleStages;

    public Stage CurrentStage { get; set; }
    public int CurrentStageIndex { get; set; } = 0;

    Dictionary<PackedScene, int> AvailableMonstersToSpawn;

    private Random randomizer = new Random();
    public PlayerController PlayerController { get; set; }

	int LvlNumber;
    
  
    public override void _Ready()
    {
        base._Ready();

        TowerDefenseArea = GetNode<TowerDefenseArea>("TowerDefenseArea");      

        AvailableMonstersToSpawn = new Dictionary<PackedScene, int>();
        foreach (var s in availableMonstersToSpawn)
        {
            AIController a = s.Instantiate<AIController>();
            BaseMonster m = (BaseMonster)a.GetChildren().FirstOrDefault(c => c is BaseMonster);
            AvailableMonstersToSpawn.Add(s, m.DifficultyLevel);
        }     
    }

    public void ScheduleNextStage()
    {
        CurrentStage.StageFinish -= ScheduleNextStage;
        int nextStageIndex = CurrentStageIndex + 1;
        if(nextStageIndex >= BattleStages.Length)
        {
            Finish();
            return;
        }
        CurrentStageIndex = nextStageIndex;
        GameInstance.Hud.BattlefieldWidget.WaveCounterWidget.FinishCurrentBlock();
        CurrentStage = BattleStages[nextStageIndex];
        CurrentStage.StageFinish += ScheduleNextStage;
        InitMonsters(CurrentStage);
    }
    public void ScheduleStage()
    {       
        CurrentStage = BattleStages[0];
        CurrentStageIndex = 0;
        GameInstance.Hud.BattlefieldWidget.WaveCounterWidget.FinishCurrentBlock();
        CurrentStage.StageFinish += ScheduleNextStage;
        InitMonsters(CurrentStage);
    }
    public void ScheduleNextStageTimeout()
    {
        if (CurrentStageIndex >= BattleStages.Length-1) //check if last stage
        {
            return;
        }
        CurrentStage.StageFinish -= ScheduleNextStage;
        int nextStageIndex = CurrentStageIndex + 1;       
        CurrentStageIndex = nextStageIndex;
        GameInstance.Hud.BattlefieldWidget.WaveCounterWidget.FinishCurrentBlock();
        CurrentStage = BattleStages[nextStageIndex];
        CurrentStage.StageFinish += ScheduleNextStage;
        InitMonsters(CurrentStage);
    }
    /// <summary>
    /// Called after World is changed and BattleFieldWidget is ready
    /// </summary>
    /// <param name="lvlNumber"></param>
    /// <param name="plants"></param>
    public void Init(int lvlNumber, Dictionary<int, int> plants)
	{
        PlayerController.BattlefieldInventory.Init(plants);
        PlayerController.Hud.BattlefieldWidget.BattlePlantsItemsInventoryWidget.SetInventory(this.GetPlayerController().BattlefieldInventory, new BaseSlot.Comparers.DefaultDesc(), ItemType.BattlePlant);
        LvlNumber = lvlNumber;
    }

    private void InitMonsters(Stage currentStage, bool delayed = false)
    {
        var monstersAndLines = new List<(int, AIController)>();
        Random random = new Random();

        var lines = SplitRange(TowerDefenseArea.LastNorthernLine, TowerDefenseArea.LastSouthernLine);

        lines.Shuffle();

        int monstersToSpawn = random.Next(currentStage.MinMonsterCount, currentStage.MaxMonsterCount + 1);

        for (int i = 0; i < monstersToSpawn; i++)
        {
            int randomLineIndex = random.Next(lines.Count);
            int randomLine = lines[randomLineIndex];

            var monster = GetRandomAvailableMonster(currentStage);

            monstersAndLines.Add((randomLine, monster));
        }

        SpawnMonsters(monstersAndLines, currentStage, delayed);
    }
    public void ShowStageNameWidget()
    {
        var waveWidget = Scenes.Widgets.BattleWidget.WaveWidget();
        GameInstance.Hud.AddChild(waveWidget);
        waveWidget.Init((CurrentStageIndex + 1).ToString());
    }
    private void SpawnMonsters(List<(int Line, AIController Scene)> scenes, Stage currentStage, bool delayed)
    {
        ShowStageNameWidget();
        int SpawnedMonsterCount = scenes.Count;
        currentStage.ActiveMonsters.AddRange(scenes);
        int currentSpawnedMonsterIndex = 0;
        int worldTimerSecond = WorldTimer.worldTimerMode == WorldTimerMode.Default ? WorldTimer.CurrentSecond : 0;
        while (currentSpawnedMonsterIndex < scenes.Count)
        {
            var currentMonster = currentStage.ActiveMonsters[currentSpawnedMonsterIndex];
            WorldTimer.ScheduleSpawnMonsterEvent(worldTimerSecond, currentMonster.Line, currentMonster.Cntroller);
            worldTimerSecond += (int)currentStage.SpawnRate;
            currentSpawnedMonsterIndex++;
        }
        //if 10 stages then last is index 8: 8<=9
        if(CurrentStageIndex <= BattleStages.Length - 1)
        {
            WorldTimer.ScheduleNextStageEvent(worldTimerSecond);
        }
    }

    private AIController GetRandomAvailableMonster(Stage currentStage)
    {
        // Filter the dictionary to only include monsters whose difficulty is less than or equal to the DifficultyLevel
        var suitableMonsters = AvailableMonstersToSpawn.Where(monster => monster.Value <= currentStage.Difficulty).ToList();
        if (suitableMonsters.Count == 0)
        {
            return null;
        }
        int index = randomizer.Next(suitableMonsters.Count);
        return suitableMonsters[index].Key.Instantiate<AIController>();
    }

    public List<int> SplitRange(int min, int max)
    {
        List<int> result = new List<int>();

        for (int i = min; i <= max; i++)
        {
            result.Add(i);
        }

        return result;
    }

    public void Finish()
    {
        var currentlvl = PlayerController.currentLvl;
        if (currentlvl == LvlNumber)
            PlayerController.currentLvl++;

        PlayerController.Mutagen = 0;
        GD.Print("Finish level");
    }
    public override void WorldEnteredListener(PlayerController p)
    {
        PlayerController = p;
        PlayerController.BattlefieldEnergy = PlayerController.MaxEnergy; // start with full energy
        GameInstance.Hud.DisplayBattlefieldWidget(p);
        p.CurrentInventory = p.BattlefieldInventory;
        p.TimerEnergyRestore.Start();

        GameInstance.Hud.BattlefieldWidget.AddWaveCounterWidget(BattleStages);
        GameInstance.Hud.BattlefieldWidget.WaveCounterWidget.StartTimer();

        var time = new Timer()
        {
            WaitTime = BattleStages[0].StageDelay,
            Autostart = true,
            OneShot = true,
        };

        time.Timeout += ScheduleStage;

        AddChild(time);

        WorldTimer.Init(WorldTimerMode.Default);         
    }
    public override void WorldExitedListener(PlayerController p)
    {
        PlayerController.TimerEnergyRestore.Stop();
    }
}
