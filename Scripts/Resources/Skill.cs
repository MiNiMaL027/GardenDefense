using Enums;
using Godot;
using Godot.Collections;


[GlobalClass]
public partial class Skill : Resource
{
    [Export]
    public Rarity SkillRarity { get; set; }
    [Export]
    public string Name { get; set; }
    [Export]
    public string Description { get; set; }
    [Export]
    public Texture2D Icon { get; set; }
    [Export]
    public bool isUnique { get; set; }
    [Export]
    public Array<Skill> DependenceSkills { get; set; }
    [Export]
    public Array<Stat> SkillProperties { get; set; } = new Array<Stat>();
}

