using Godot;
using Pawns.BattlePlants;
using System.Collections.Generic;
using System.Linq;

public partial class ChooseSkillWidget : PanelContainer
{
	private BaseBattlePlant WidgetOwner; 
	private List<SkillWindow> AvailableSkills = new List<SkillWindow>();
	private SkillWindow SelectedSkill { get; set; }

	private HBoxContainer SkillsContainer;

	Button ApplyButton;
	Button SkipButton;
	Button RefreshButton;

    public override void _Ready()
    {
        base._Ready();

		SkillsContainer = GetNode<HBoxContainer>("MarginContainer/VBoxContainer/PanelContainer/MarginContainer/SkillsContainer");
		ApplyButton = GetNode<Button>("MarginContainer/VBoxContainer/HBoxContainer/ApplyButton");
		SkipButton = GetNode<Button>("MarginContainer/SkipButton");
		RefreshButton = GetNode<Button>("MarginContainer/VBoxContainer/HBoxContainer/RefreshButton");

        ApplyButton.Pressed += ApplyButton_Pressed;
        SkipButton.Pressed += SkipButton_Pressed;
        RefreshButton.Pressed += RefreshButton_Pressed;

        GetTree().Paused = true;
    }

    private void RefreshButton_Pressed()
    {
		RefreshSkills();
    }

    private void SkipButton_Pressed()
    {
        CloseWidget();
    }

    private void ApplyButton_Pressed()
    {
		if(SelectedSkill == null)
		{
			//TODO show mesage that any skill didn`t selecte
			return;
		}
			
		WidgetOwner.SkillComponent.ApplySkill(SelectedSkill.Skill);

		CloseWidget();
    }

	private void CloseWidget()
	{
        GetTree().Paused = false;
        GameInstance.Hud.BattlefieldWidget.CloseChooseSkillWidget();
	}

    public void Init(BaseBattlePlant owner)
	{
		WidgetOwner = owner;

		RefreshSkills();
	}

	public void RefreshSkills()
	{
        SkillsContainer.RemoveChildren();
		AvailableSkills.Clear();
		SelectedSkill = null;

        foreach (var skill in WidgetOwner.SkillComponent.GetAvailableSkills())
        {
            var skillWindow = Scenes.Widgets.Skills.SkillWindow();
            SkillsContainer.AddChild(skillWindow);
            skillWindow.Init(skill);
            skillWindow.SelectSkill += ChooseSkill;
            AvailableSkills.Add(skillWindow);
        }
    }

	public void ChooseSkill(SkillWindow skill)
	{
		SelectedSkill = skill;
		ActiveSkillWindow(skill);
	}

	private void ActiveSkillWindow(SkillWindow skillWindow)
	{
		foreach (var skill in AvailableSkills)
		{
			skill.IsActive = false;
		}

		skillWindow.IsActive = true;
	}
}
