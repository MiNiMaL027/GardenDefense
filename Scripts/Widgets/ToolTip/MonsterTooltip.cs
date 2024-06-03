using Godot;
using Pawns;
using Pawns.Monsters;
using Widgets.ToolTip;

public partial class MonsterTooltip : BaseTooltip
{
    Label LabelItemName;
    public override void _Ready()
    {
        base._Ready();

        LabelItemName = GetNode<Label>("LabelItemName");
    }

    public virtual void ShowTooltip(BaseMonster monster)
    {
        LabelItemName.Text = monster.PawnName;
    }
}
