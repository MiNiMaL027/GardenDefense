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
    int ChanceSpawnMonsterToOtherLines = 100;
    [Export]
    int MaxLineMonsterNumber = 1;
    [Export]
    int MinLineMonsterNumber = 1;
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

            difficultLevel = value;
        }
    }
    public override void _Ready()
    {
        base._Ready();

        TowerDefenseArea = GetNode<TowerDefenseArea>("TowerDefenseArea");
        DifficultyLevel = DifficultyLevel;

        AvailableMonstersToSpawn = new Dictionary<PackedScene, int>();
        foreach (var s in availableMonstersToSpawn)
        {
            AIController a = s.Instantiate<AIController>();
            BaseMonster m = (BaseMonster)a.GetChildren().FirstOrDefault(c => c is BaseMonster);
            int difficulty = m.DifficultyLevel;
            AvailableMonstersToSpawn.Add(s, difficulty);
        }      
    }

    public void Init(int lvlNumber, Dictionary<int, int> plants)
	{
        this.GetPlayerController().BattlefieldInventory.Init(plants);
        stepCount = MaxEnemyCount / MaxDifficultLevelToBattle;
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
        InitMonsters();
    }

    private void InitMonsters()
    {
        var monstersAndLines = new Dictionary<int, List<PackedScene>>();
        Random random = new Random();

        var lines = SplitRange(TowerDefenseArea.LastNorthernLine, TowerDefenseArea.LastSouthernLine);

        lines.Shuffle();

        var chanceToSpawn = ChanceSpawnMonsterToOtherLines;

        for (int i = 0; i < lines.Count; i++)
        {
            if(i == 0 || chanceToSpawn >= random.Next(0, 100))
            {
                int monstersToSpawn = random.Next(MinLineMonsterNumber, MaxLineMonsterNumber + 1);

                List<PackedScene> monstersOnLine = new List<PackedScene>();

                for (int ii = 0; ii < monstersToSpawn; ii++)
                {
                    monstersOnLine.Add(GetRandomAvailableMonster());
                }

                monstersAndLines[lines[i]] = monstersOnLine;

                if(i > 1)
                    chanceToSpawn /= 2;
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
        List<int> lines = monstersAndLines.Keys.ToList();

        List<List<PackedScene>> scenesForLines = monstersAndLines.Values.ToList();
        for(int i = 0; i < scenesForLines.Count;)
        {
            if (scenesForLines[i].Count == 0)
            {
                lines.RemoveAt(i);
                scenesForLines.RemoveAt(i);
            }
            else
            {
                i++;
            }
        }

        int timeOffset = 0;
        List<int> linesToSchedule = new List<int>();
        List<PackedScene> scenesToSchedule= new List<PackedScene>();
        do
        {
            for (int i = 0; i < lines.Count; i++)
            {
                linesToSchedule.Add(lines[i]);
                PackedScene p = scenesForLines[i].FirstOrDefault();
                scenesToSchedule.Add(p);
                scenesForLines[i].Remove(p);
                if (scenesForLines[i].Count == 0)
                {
                    lines.RemoveAt(i);
                    scenesForLines.RemoveAt(i);
                    i--;

                }
            }
            WorldTimer.ScheduleSpawnMonsterEvent(WorldTimer.CurrentSecond + timeOffset, linesToSchedule, scenesToSchedule);
            timeOffset++;
            linesToSchedule.Clear();
            scenesToSchedule.Clear();
        } while (lines.Count > 0);

        RefreshDifficult();
    }



    private void RefreshDifficult()
    {
        DifficultyLevel = Math.Min(1 + (SpawnedMonsterCount / stepCount), Constants.MaxDifficultLevel);
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

    public List<int> SplitRange(int min, int max)
    {
        List<int> result = new List<int>();

        for (int i = min; i <= max; i++)
        {
            result.Add(i);
        }

        return result;
    }
}
