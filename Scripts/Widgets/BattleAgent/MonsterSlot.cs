using Godot;
using Pawns;
using Pawns.Monsters;
using System;

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
        foreach(var pawnType in monster.MonsterType.GetFlags())
        {
            var icon = ResourceLoader.Load<Texture2D>($"res://raw assets/Images/Monsters/Type icon/{pawnType}.png");
            IconsContainer.AddIcon(icon, new Vector2(15, 15), pawnType.ToString());

        }
        foreach (var pawnClass in monster.Class.GetFlags())
        {
            var classIcon = ResourceLoader.Load<Texture2D>($"res://raw assets/Images/Monsters/Type icon/{pawnClass}.png");
            IconsContainer.AddIcon(classIcon, new Vector2(12, 12), pawnClass.ToString());
        }
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
