using Godot;
using Pawns;
using Pawns.Monsters;

public partial class MonsterSlot : Panel
{
    public TextureRect Texture { get; set; }
    public MonsterTooltip Tooltip { get; set; }
    public HBoxContainer IconsContainer { get; set; }

    public BaseMonster Monster;

    public override void _Ready()
    {
        base._Ready();

        Texture = GetNode<TextureRect>("MarginContainer/TextureRect");
        IconsContainer = GetNode<HBoxContainer>("MarginContainer/HBoxContainer");
        MouseEntered += MonsterSlot_MouseEntered;
        MouseExited += MonsterSlot_MouseExited;
    }

    public override void _ExitTree()
    {
        base._ExitTree();

        HideTooltip();
    }

    private void MonsterSlot_MouseExited()
    {
        HideTooltip();
    }

    private void MonsterSlot_MouseEntered()
    {
        ShowToolTip();
    }

    public void Init(BaseMonster monster)
    {
        Monster = monster;

        var monsterInfo = DbService.GetPawnDataById(monster.PawnId);
        Texture.Texture = monsterInfo.texture;

        IconsContainer.RemoveChildren();
        var icon = ResourceLoader.Load<Texture2D>($"res://raw assets/Images/Monsters/Type icon/{monster.MonsterType}.png");
        AddIcon(icon);
    }

    private void AddIcon(Texture2D icon)
    {
        var rect = new TextureRect();
        rect.CustomMinimumSize = new Vector2(15, 15);
        rect.ExpandMode = TextureRect.ExpandModeEnum.FitWidth;
        rect.Texture = icon;
        IconsContainer.AddChild(rect);
    }

    public void ShowToolTip()
    {
        Tooltip = Scenes.Widgets.ToolTip.MonsterTooltip();
        Vector2 globalMousePosition = GetViewport().GetMousePosition();

        AddChild(Tooltip);

        Tooltip.ShowTooltip(Monster);
        Tooltip.AdjustControlInViewport(globalMousePosition);
        Tooltip.PostInit();
    }

    public virtual void HideTooltip()
    {
        if (Tooltip != null)
        {
            Tooltip.HideTooltip();

            Tooltip = null;
        }
    }
}
