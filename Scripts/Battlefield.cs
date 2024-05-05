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

    Dictionary<PackedScene, int> AvailableMonstersToSpawn;

    private Random randomizer = new Random();

    int ChanceSpawnMonsterToOtherLines = 0;
    int SpawnedMonsterCount = 0;
    int MaxLineMonsteNumber = 1;
    int MaxEnemyCount;
	int TimeToSpawn;
	int LvlNumber;
	int LineCount;
    int MaxDifficultLevelToBattle;
    
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
            MaxLineMonsteNumber = difficultLevel;
            TimeToSpawn /= difficultLevel;
            difficultLevel = value;
        }
    }
    public override void _Ready()
    {
        base._Ready();
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
        WorldTimer.ScheduleSpawnMonsterEvent(5, new List<int> { TowerDefenseArea.LastNorthernLine, 0, TowerDefenseArea.LastSouthernLine }, new List<int> { Ids.PawnId.Monsters.Wasp, Ids.PawnId.Monsters.Ant, Ids.PawnId.Monsters.AntDog });
        WorldTimer.ScheduleSpawnMonsterEvent(10, new List<int> { TowerDefenseArea.LastNorthernLine, 0, TowerDefenseArea.LastSouthernLine }, new List<PackedScene>(availableMonstersToSpawn));

    }

    private void initMonster()
    {
        var monstersAndLines = new Dictionary<int, List<AIController>>();
        Random random = new Random();

        if (ChanceSpawnMonsterToOtherLines > 0)
        {
            double remainingChance = ChanceSpawnMonsterToOtherLines;

            for (int i = 0; i < LineCount; i++)
            {
                double chanceToSpawnOnCurrentLine = remainingChance / (LineCount - i);

                if (chanceToSpawnOnCurrentLine <= 0)
                    break;

                int monstersToSpawn = random.Next(0, MaxLineMonsteNumber);

                // Створюємо список монстрів для поточної лінії
                List<AIController> monstersOnLine = new List<AIController>();

                for (int j = 0; j < monstersToSpawn; j++)
                {
                    if (random.NextDouble() <= chanceToSpawnOnCurrentLine)
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
        if (monstersAndLines.Count <= 0)
        {
            var line = random.Next(0, LineCount);

            List<AIController> monstersOnLine = new List<AIController>();
            monstersOnLine.Add(GetRandomAvailableMonster());

            monstersAndLines[line] = monstersOnLine;
        }

        SpawnMonsters(monstersAndLines);
    }


    private void SpawnMonsters(Dictionary<int, List<AIController>> monstersAndLines)
    {
        SpawnedMonsterCount += monstersAndLines.Values.Sum(list => list.Count);

        // Проходження кожної лінії у словнику
        foreach (var kvp in monstersAndLines)
        {
            int lineNumber = kvp.Key;
            List<AIController> monstersOnLine = kvp.Value;

            // Заспавнення монстрів на поточній лінії з затримкою між ними
            for (int i = 0; i < monstersOnLine.Count; i++)
            {
                AIController monster = monstersOnLine[i];
                TowerDefenseArea.SpawnMonster(lineNumber, monster, i * 1);
            }
        }

        RefreshDifficult();
    }

    private void RefreshDifficult()
    {
        var stepCount = MaxEnemyCount / Constants.MaxDifficultLevel - 1;
        DifficultyLevel = Math.Min(SpawnedMonsterCount / stepCount, Constants.MaxDifficultLevel);

        if(DifficultyLevel > Constants.MaxDifficultLevel)
        {
            DifficultyLevel = Math.Min(DifficultyLevel, Constants.MaxDifficultLevel);
        }
    }

    private AIController GetRandomAvailableMonster()
    {
        // Filter the dictionary to only include monsters whose difficulty is less than or equal to the DifficultyLevel
        var suitableMonsters = AvailableMonstersToSpawn.Where(monster => monster.Value <= DifficultyLevel).ToList();
        if (suitableMonsters.Count == 0)
        {
            GD.Print("No monsters available with difficulty level " + DifficultyLevel + " or lower.");
            return null;
        }
        int index = randomizer.Next(suitableMonsters.Count);
        return suitableMonsters[index].Key.Instantiate<AIController>();


    }
}
