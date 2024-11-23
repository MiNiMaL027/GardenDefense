using Enums;
using Godot;
using Godot.Collections;
using Pawns.BattlePlants;
using System;
using System.Collections.Generic;
using System.Linq;


public partial class SkillComponent : Node
{
    public BaseBattlePlant ComponentOwner { get; set; }
    public List<Skill> ActiveSkills { get; set; } = new List<Skill>();
    [Export]
    public Array<Skill> SkillPool { get; set; } = new Array<Skill>();
    public List<Skill> AvailableSkills { get; set; } = new List<Skill>();
    public int ChooseSkillCount = 3;

    public static System.Collections.Generic.Dictionary<Rarity, int> Weights = new System.Collections.Generic.Dictionary<Rarity, int>
    {
        { Rarity.Common, 60 },
        { Rarity.Rare, 25 },
        { Rarity.Epic, 10 },
        { Rarity.Legendary, 5 }
    };

    public void Init(BaseBattlePlant componentOwner)
    {
        ComponentOwner = componentOwner;

        AvailableSkills.AddRange(SkillPool);
    }

    public void ApplySkill(Skill skill)
    {
        //Delete from pool unique skill
        if (skill.isUnique)
            AvailableSkills.Remove(skill);

        //Add new dependence skills to pool
        AvailableSkills.AddRange(skill.DependenceSkills);

        //Apply skill effects
        foreach (var stat in skill.SkillProperties)
        {
            ComponentOwner.StatsComponent.SetCustomStat(stat.StatName, ComponentOwner.StatsComponent.GetCustomStat(stat.StatName) + stat.StatValue);
        }

        //Add skill to activated list
        ActiveSkills.Add(skill);
    }

    public List<Skill> GetAvailableSkills()
    {
        // Перевірка, чи є доступні скіли
        if (AvailableSkills == null || AvailableSkills.Count == 0)
            return new List<Skill>();

        // Створюємо список для обраних скілів
        List<Skill> selectedSkills = new List<Skill>();

        // Створюємо список скілів з їх вагами
        List<(Skill skill, int weight)> weightedSkills = new List<(Skill, int)>();
        foreach (var skill in AvailableSkills)
        {
            weightedSkills.Add((skill, Weights[skill.SkillRarity]));           
        }

        // Вибір зважених скілів
        Random random = new Random();
        for (int i = 0; i < ChooseSkillCount; i++)
        {
            if (weightedSkills.Count == 0)
                break;

            int totalWeight = weightedSkills.Sum(ws => ws.weight);
            int randomValue = random.Next(0, totalWeight);

            Skill selected = null;
            int cumulativeWeight = 0;

            foreach (var (skill, weight) in weightedSkills)
            {
                cumulativeWeight += weight;
                if (randomValue < cumulativeWeight)
                {
                    selected = skill;
                    break;
                }
            }

            if (selected != null)
            {
                selectedSkills.Add(selected);
                weightedSkills.RemoveAll(ws => ws.skill == selected); // Видаляємо, щоб уникнути повторів
            }
        }

        return selectedSkills;
    }
}

