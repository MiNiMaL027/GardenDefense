using Godot;
public partial class SkillWindow : PanelContainer
{
    TextureRect SkillIconTextureRect;
    TextureRect SelectedIconTextureRect;
    Label SkillNameLabel;
    Label SkillDescriptionLabel;
	public Skill Skill { get; set; }

    public Color HoverColor = new Color(0.933f, 0.98f, 0.71f);

    private bool isActive;
    public bool IsActive
    {
        get { return isActive; } 
        set 
        {
            if (value)
                SelectedIconTextureRect.Visible = true;
            else
                SelectedIconTextureRect.Visible = false;

            isActive = value;
        }
    }

    [Signal]
    public delegate void SelectSkillEventHandler(SkillWindow skill);
    public override void _Ready()
	{
        SkillIconTextureRect = GetNode<TextureRect>("MarginContainer/VBoxContainer/MarginContainer/PanelContainer/SkillIcon");
        SkillNameLabel = GetNode<Label>("MarginContainer/VBoxContainer/PanelContainer/MarginContainer/SkillName");
        SkillDescriptionLabel = GetNode<Label>("MarginContainer/VBoxContainer/SkillDesc");
        SelectedIconTextureRect = GetNode<TextureRect>("MarginContainer/SelectedIcon");

        MouseEntered += SkillWindow_MouseEntered;
        MouseExited += SkillWindow_MouseExited;
	}

    private void SkillWindow_MouseExited()
    {
        Modulate = new Color(1, 1, 1);
    }

    private void SkillWindow_MouseEntered()
    {
        Modulate = HoverColor;
    }

    public void Init(Skill skill)
	{
		Skill = skill;

        SelfModulate = ExtensionMethods.GetColorByRarity(skill.SkillRarity);
        SkillNameLabel.Text = skill.Name;
        SkillDescriptionLabel.Text = skill.Description;
        SkillIconTextureRect.Texture = skill.Icon;
	}

    public override void _GuiInput(InputEvent @event)
    {
        if(@event is InputEventMouseButton mouseButton)
        {
            if(mouseButton.ButtonIndex == MouseButton.Left && mouseButton.Pressed)
            {
                EmitSignal(SignalName.SelectSkill, this);
            }
        }
    }
}
