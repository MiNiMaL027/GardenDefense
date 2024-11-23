using Godot;
using Godot.Collections;
using Pawns.BattlePlants;
using System.Collections.Generic;


public partial class SkillComponent : Node
{
    public BaseBattlePlant ComponentOwner { get; set; }
    public List<Skill> ActiveSkills { get; set; } = new List<Skill>();
    [Export]
    public Array<Skill> SkillPool { get; set; } = new Array<Skill>();
    public List<Skill> AvailableSkills { get; set; } = new List<Skill>();
    public int ChooseSkillCount = 3;

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
}

