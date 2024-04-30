using Controllers;
using Farm.Scripts;
using Godot;
using Items;
using Pawns;
using Pawns.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Battlefield : Node3D
{
    [Export]
    string[] MonstersPaths;
	List<AIController> AvailablePawnsToSpawn = new List<AIController>();
    int ChanceSpawnMonsterToOtherLines = 0;
    int SpawnedMonsterCount = 0;
    int MaxLineMonsteNumber = 1;
    int MaxEnemyCount;
	int TimeToSpawn;
	int LvlNumber;
	int LineCount;
    int MaxDifficultLevelToBattle;
    
    int difficultLevel = 1;
    int DifficultLevel {
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

	public void Init(int lvlNumber, Dictionary<int, int> plants)
	{
		//TODO init parameters

        foreach(var path in MonstersPaths)
        {
            AvailablePawnsToSpawn.Add(ResourceLoader.Load<PackedScene>(path).Instantiate<AIController>());
        }
	}

    private void initMonster()
    {
        var monstersAndLines = new Dictionary<int, AIController>();
        Random random = new Random();

        // Розподіл шансів спавну монстрів на інших лініях
        if (ChanceSpawnMonsterToOtherLines > 0)
        {
            double remainingChance = ChanceSpawnMonsterToOtherLines;

            for (int i = 0; i < LineCount; i++)
            {
                // Розрахунок шансу спавну монстрів на поточній лінії з рівномірним розподілом навантаження
                double chanceToSpawnOnCurrentLine = remainingChance / (LineCount - i);

                // Якщо шанс стає недостатнім, виходимо з циклу
                if (chanceToSpawnOnCurrentLine <= 0)
                    break;

                // Генерація кількості спавнів монстрів на поточній лінії
                int monstersToSpawn = random.Next(0, MaxLineMonsteNumber); // Випадкова кількість монстрів на лінії

                for (int j = 0; j < monstersToSpawn; j++)
                {
                    // Перевірка шансу спавну монстра на поточній лінії
                    if (random.NextDouble() <= chanceToSpawnOnCurrentLine)
                    {
                        monstersAndLines[i] = GetRandomAvailableMonster();
                    }
                }

                // Зменшення решти шансу на наступній лінії
                remainingChance -= chanceToSpawnOnCurrentLine;
            }
        }

        // Якщо жоден монстр не був спавнений, спавнемо хоча б одного випадкового монстра
        if (monstersAndLines.Count <= 0)
        {
            var line = random.Next(0, LineCount); // Випадкова лінія

            monstersAndLines[line] = GetRandomAvailableMonster();
        }


        SpawnMonsters(monstersAndLines);
    }

    private void SpawnMonsters(Dictionary<int, AIController> monstersAndLines)
    {
        //TODO Spawn monsters from dictionary on field

        SpawnedMonsterCount += monstersAndLines.Values.Count;

        RefreshDifficult();
    }

    private void RefreshDifficult()
    {
        var stepCount = MaxEnemyCount / Constants.MaxDifficultLevel - 1;
        DifficultLevel = Math.Min(SpawnedMonsterCount / stepCount, Constants.MaxDifficultLevel);

        if(DifficultLevel > Constants.MaxDifficultLevel)
        {
            DifficultLevel = Math.Min(DifficultLevel, Constants.MaxDifficultLevel);
        }
    }

    private AIController GetRandomAvailableMonster()
    {
        var difficultAvailablepawnToSpawn = AvailablePawnsToSpawn.Where(p => (p.Pawn as BaseMonster).DifficultLevel <= DifficultLevel);
        var random = new Random();
        var monsterIndex = random.Next(0, difficultAvailablepawnToSpawn.Count() - 1);

        return difficultAvailablepawnToSpawn.ElementAt(monsterIndex);
    }
}
