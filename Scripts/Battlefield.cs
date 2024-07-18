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
    public int RestTimeBetweenStages = 10;
    [Export]
    public PackedScene[] availableMonstersToSpawn;
    [Export]
    public Stage[] BattleStages;

    public Stage currentStage;

    public List<Stage> DefaultBattleStages;

    public int currentStageIndex = 0;

    private int currentSpawnedMonsterIndex = 0;

    Dictionary<PackedScene, int> AvailableMonstersToSpawn;

    private Random randomizer = new Random();
    public PlayerController PlayerController { get; set; }
    public Timer Timer { get; set; }
    public Timer SpawnTimer { get; set; }
    public Timer RestTimer { get; set; }

    int SpawnedMonsterCount = 0;
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
            int difficulty = m.DifficultyLevel;
            AvailableMonstersToSpawn.Add(s, difficulty);
        }

        DefaultBattleStages = BattleStages.Where(s => s.StageType == StageType.Default).ToList();
        currentStage = BattleStages.FirstOrDefault(s => s.StageType == StageType.Preparatory);

        RefreshSpawnTimer();

        RestTimer = new Timer();
        AddChild(RestTimer);
        RestTimer.OneShot = true;
        RestTimer.WaitTime = RestTimeBetweenStages;
        RestTimer.Timeout += NextStage;
    }

    private void RefreshSpawnTimer(bool delete = false)
    {
        if (delete)
            SpawnTimer.QueueFree();

        SpawnTimer = new Timer();
        AddChild(SpawnTimer);
        SpawnTimer.OneShot = false;
    }

    private void NextStage()
    {
        if(currentStage.StageType != StageType.Preparatory)
            currentStage.StageFinish -= FinishStage;

        currentSpawnedMonsterIndex = 0;
        GD.Print($"Stage count = {DefaultBattleStages.Count} || Stage index = {currentStageIndex}");
        if(DefaultBattleStages.Count <= currentStageIndex)
        {
            var bossStage = BattleStages.FirstOrDefault(s => s.StageType == StageType.Boss);

            if (bossStage != null)
            {
                currentStage = bossStage;
            }
            else 
            {
                var lastStage = BattleStages.FirstOrDefault(s => s.StageType == StageType.LastStage);
                currentStage = lastStage;
            }

            StartFinishStage(currentStage);
            return;
        }   
        
        
        currentStage = DefaultBattleStages[currentStageIndex];
        currentStageIndex++;

        GD.Print($"{currentStage.StageType} - next stage");

        currentStage.StageFinish += FinishStage;

        UpdateLines(currentStage.LinesCount);
        UpdateTimer(currentStage.StageDuration, currentStage.SpawnRate);

        InitMonsters();
    }
    private void FinishStage()
    {
        SpawnTimer.Stop();
        Timer.Stop();

        RestTimer.Start(0);
    }
    private void UpdateLines(int linesCount)
    {
        var range = ExtensionMethods.GetRange(linesCount);

        TowerDefenseArea.LastNorthernLine = range.min;
        TowerDefenseArea.LastSouthernLine = range.max;
    }

    private void UpdateTimer(int? time, float spawnRate)
    {
        if(time == null)
        {
            Timer.Stop();
        }
        else
        {
            Timer.WaitTime = time.Value;
            Timer.Start(0);
        }
        

        SpawnTimer.WaitTime = spawnRate;
        SpawnTimer.Start(0);
    }

    private void StartFinishStage(Stage stage)
    {
        stage.StageFinish += Finish;
        GD.Print("Finish stage");

        UpdateLines(stage.LinesCount);
        UpdateTimer(null, stage.SpawnRate);

        InitMonsters();
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

    public void InitTimer()
    {
        if(Timer == null)
        {
            Timer = new Timer();
            AddChild(Timer);
            Timer.WaitTime = BattleStages.FirstOrDefault(s => s.StageType == StageType.Preparatory).StageDuration;
            Timer.OneShot = true;
            Timer.Timeout += Timer_Timeout;
            Timer.Start();
        }
        else
        {
            Timer.Stop();
        }
    }
    private void Timer_Timeout()
    {
        NextStage();        
    }

    private void InitMonsters()
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

            var monster = GetRandomAvailableMonster();

            monstersAndLines.Add((randomLine, monster));
        }

        SpawnMonsters(monstersAndLines);
    }

    private void SpawnMonsters(List<(int Line, AIController Scene)> scenes)
    {
        GD.Print("Spawn monsters \n");
        SpawnedMonsterCount = scenes.Count;
        currentStage.ActiveMonsters.AddRange(scenes);

        RefreshSpawnTimer(true);

        SpawnTimer.Timeout += () => SpawnTimer_Timeout(scenes.Count);
        SpawnTimer.Start();
    }

    private void SpawnTimer_Timeout(int maxMonsters)
    {
        
        if (currentSpawnedMonsterIndex >= maxMonsters - 1)
        {
            SpawnTimer.Stop();
            return;
        }

        var currentMonster = currentStage.ActiveMonsters[currentSpawnedMonsterIndex];

        WorldTimer.ScheduleSpawnMonsterEvent(WorldTimer.CurrentSecond, currentMonster.Line, currentMonster.Cntroller);
        GD.Print(currentMonster.Cntroller);
        GD.Print($"Stage index:{currentStageIndex} - monster spawned");

        currentSpawnedMonsterIndex++;
    }

    private AIController GetRandomAvailableMonster()
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

        GD.Print("Finish level");
    }
    public override void WorldEnteredListener(PlayerController p)
    {
        PlayerController = p;
        PlayerController.BattlefieldEnergy = PlayerController.MaxEnergy; // start with full energy
        GameInstance.Hud.DisplayBattlefieldWidget(p);
        p.CurrentInventory = p.BattlefieldInventory;
        p.TimerEnergyRestore.Start();

    }
    public override void WorldExitedListener(PlayerController p)
    {
        PlayerController.TimerEnergyRestore.Stop();

    }
}
