using Controllers;
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
	PackedScene[] availableMonstersToSpawn;
    [Export]
    int ChanceSpawnMonsterToOtherLines = 0;
    [Export]
    int MaxLineMonsterNumber = 1;
    [Export]
    int MaxEnemyCount = 0;
    [Export]
    int TimeToSpawn = 10;
    [Export]
    int MaxDifficultLevelToBattle = 2;






    Dictionary<PackedScene, int> AvailableMonstersToSpawn;

    private Random randomizer = new Random();
    public Timer Timer { get; set; }

    int SpawnedMonsterCount = 0;
	int LvlNumber;
	int LineCount;
    int stepCount;


    int difficultLevel = 1;
    int DifficultyLevel {
        get
        {
            return difficultLevel;
        }
        set 
        {
            if(value > MaxDifficultLevelToBattle)
                value = MaxDifficultLevelToBattle;

            ChanceSpawnMonsterToOtherLines *= difficultLevel;
            MaxLineMonsterNumber = difficultLevel;
            TimeToSpawn /= difficultLevel;
            difficultLevel = value;
        }
    }
    public override void _Ready()
    {
        base._Ready();
        DifficultyLevel = DifficultyLevel;
        AvailableMonstersToSpawn = new Dictionary<PackedScene, int>();
        foreach (var s in availableMonstersToSpawn)
        {
            AIController a = s.Instantiate<AIController>();
            BaseMonster m = (BaseMonster)a.GetChildren().FirstOrDefault(c => c is BaseMonster);
            int difficulty = m.DifficultyLevel;
            AvailableMonstersToSpawn.Add(s, difficulty);
        }
        TowerDefenseArea = GetNode<TowerDefenseArea>("TowerDefenseArea");
    }
    public void Init(int lvlNumber, Dictionary<int, int> plants)
	{
        this.GetPlayerController().BattlefieldInventory.Init(plants);
        stepCount = MaxEnemyCount / (Constants.MaxDifficultLevel - 1);

    }
    public void InitTimer()
    {
        if(Timer == null)
        {
            Timer = new Timer();
            AddChild(Timer);
            Timer.WaitTime = TimeToSpawn;
            Timer.OneShot = false;
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
        initMonster();
    }

    private void initMonster()
    {
        var monstersAndLines = new Dictionary<int, List<PackedScene>>();
        Random random = new Random();

        if (ChanceSpawnMonsterToOtherLines > 0)
        {
            double remainingChance = ChanceSpawnMonsterToOtherLines;
            LineCount = TowerDefenseArea.LastSouthernLine - TowerDefenseArea.LastNorthernLine + 1;
            for (int i = TowerDefenseArea.LastNorthernLine; i <= TowerDefenseArea.LastSouthernLine; i++)
            {
                double chanceToSpawnOnCurrentLine = remainingChance / (LineCount - i);

                if (chanceToSpawnOnCurrentLine <= 0)
                    break;

                int monstersToSpawn = random.Next(0, MaxLineMonsterNumber + 1);
                GD.Print("monstersToSpawn = " + monstersToSpawn);
                GD.Print("chanceToSpawnOnCurrentLine = " + chanceToSpawnOnCurrentLine);

                // Створюємо список монстрів для поточної лінії
                List<PackedScene> monstersOnLine = new List<PackedScene>();

                for (int j = 0; j < monstersToSpawn; j++)
                {
                    if (random.Next(0,100) <= chanceToSpawnOnCurrentLine)
                    {
                        monstersOnLine.Add(GetRandomAvailableMonster());
                    }
                }

                // Додаємо список монстрів для поточної лінії до словника
                monstersAndLines[i] = monstersOnLine;

                remainingChance -= chanceToSpawnOnCurrentLine;
            }
        }

        // Якщо жоден монстр не був спавнений, спавнемо хоча б одного випадкового монстра
        if (monstersAndLines.Values.Sum(list => list.Count) <= 0)
        {
            var line = random.Next(TowerDefenseArea.LastNorthernLine, TowerDefenseArea.LastSouthernLine + 1);

            List<PackedScene> monstersOnLine = new List<PackedScene>();
            monstersOnLine.Add(GetRandomAvailableMonster());

            monstersAndLines[line] = monstersOnLine;
        }
        SpawnMonsters(monstersAndLines);
    }


    private void SpawnMonsters(Dictionary<int, List<PackedScene>> monstersAndLines)
    {
        SpawnedMonsterCount += monstersAndLines.Values.Sum(list => list.Count);

        // Проходження кожної лінії у словнику
        foreach (var kvp in monstersAndLines)
        {
            int lineNumber = kvp.Key;
            List<PackedScene> monstersOnLine = kvp.Value;

            // Заспавнення монстрів на поточній лінії з затримкою між ними
            for (int i = 0; i < monstersOnLine.Count; i++)
            {
                AIController monster = monstersOnLine[i].Instantiate<AIController>();
                WorldTimer.ScheduleSpawnMonsterEvent(WorldTimer.CurrentSecond + i, new List<int>() { lineNumber }, monstersOnLine);
            }
        }

        RefreshDifficult();
    }

    private void RefreshDifficult()
    {

        DifficultyLevel = Math.Min(1 + (SpawnedMonsterCount / stepCount), Constants.MaxDifficultLevel);


        if (DifficultyLevel > Constants.MaxDifficultLevel)
        {
            DifficultyLevel = Math.Min(DifficultyLevel, Constants.MaxDifficultLevel);
        }
    }

    private PackedScene GetRandomAvailableMonster()
    {
        // Filter the dictionary to only include monsters whose difficulty is less than or equal to the DifficultyLevel
        var suitableMonsters = AvailableMonstersToSpawn.Where(monster => monster.Value <= DifficultyLevel).ToList();
        if (suitableMonsters.Count == 0)
        {
            GD.Print("No monsters available with difficulty level " + DifficultyLevel + " or lower.");
            return null;
        }
        int index = randomizer.Next(suitableMonsters.Count);
        return suitableMonsters[index].Key;
    }
}
