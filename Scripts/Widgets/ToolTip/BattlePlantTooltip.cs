using Godot;
using Pawns.BattlePlants;

namespace Widgets.ToolTip
{
    public partial class BattlePlantTooltip : BaseTooltip
    {
        Label PlantNameLabel;
        TextureRect PlantIconRect;
        HBoxContainer StatsContainer;

        Label LvlLabel;
        ProgressBar LvlProgresBar;
        Label CurrentXpLabel;
        Label MaxXpLabel;

        public override void _Ready()
        {
            base._Ready();

            PlantNameLabel = GetNode<Label>("MarginContainer/VBoxContainer/HBoxContainer2/Panel/MarginContainer/HBoxContainer/Label");
            PlantIconRect = GetNode<TextureRect>("MarginContainer/VBoxContainer/HBoxContainer2/Panel/MarginContainer/HBoxContainer/TextureRect");
            StatsContainer = GetNode<HBoxContainer>("MarginContainer/VBoxContainer/HBoxContainer");
            LvlLabel = GetNode<Label>("MarginContainer/VBoxContainer/HBoxContainer2/VBoxContainer/lvlLabel");
            LvlProgresBar = GetNode<ProgressBar>("MarginContainer/VBoxContainer/HBoxContainer2/VBoxContainer/HBoxContainer/ProgressBar");
            CurrentXpLabel = GetNode<Label>("MarginContainer/VBoxContainer/HBoxContainer2/VBoxContainer/HBoxContainer/CurrentXp");
            MaxXpLabel = GetNode<Label>("MarginContainer/VBoxContainer/HBoxContainer2/VBoxContainer/HBoxContainer/MaxXp");
        }

        public void Init(BaseBattlePlant battlePlant)
        {
            PlantNameLabel.Text = battlePlant.PawnName;
            PlantIconRect.Texture = battlePlant.Icon;
            LvlLabel.Text = "Lvl: " + battlePlant.LvlComponent.CurrentLvl.ToString();
            LvlProgresBar.MaxValue = battlePlant.LvlComponent.PointsToNextLvl;
            LvlProgresBar.Value = battlePlant.LvlComponent.CurrentPoints;
            CurrentXpLabel.Text = battlePlant.LvlComponent.CurrentPoints.ToString();
            MaxXpLabel.Text = battlePlant.LvlComponent.PointsToNextLvl.ToString();

            if (battlePlant.StatsComponent.GetStrength() > 0)
            {
                var container = StatsContainer.GetNode<HBoxContainer>("Strenght");
                container.GetNode<Label>("BaseStat").Text = battlePlant.StatsComponent.GetBaseStrength().ToString();

                if(battlePlant.StatsComponent.GetModifierStrength() > 0)
                {
                    var stat = container.GetNode<Label>("CurrentStat");
                    stat.Text = "(" + battlePlant.StatsComponent.GetStrength().ToString() + ")";
                    stat.Visible = true;
                }
            }
            if(battlePlant.StatsComponent.GetMaxHealth() > 0)
            {
                var container = StatsContainer.GetNode<HBoxContainer>("Health");
                container.GetNode<Label>("BaseStat").Text = battlePlant.StatsComponent.GetBaseMaxHealth().ToString();

                if (battlePlant.StatsComponent.GetModifierMaxHealth() > 0)
                {
                    var stat = container.GetNode<Label>("CurrentStat");
                    stat.Text = "(" + battlePlant.StatsComponent.GetMaxHealth().ToString() + ")";
                    stat.Visible = true;
                }
            }
            
            var rangeContainer = StatsContainer.GetNode<HBoxContainer>("Range");
            rangeContainer.GetNode<Label>("BaseStat").Text = battlePlant.PawnStats.AttackRange.ToString();

            var attackSpeedContainer = StatsContainer.GetNode<HBoxContainer>("AttackSpeed");
            attackSpeedContainer.GetNode<Label>("BaseStat").Text = battlePlant.PawnStats.AttackSpeed.ToString();
        }
    }
}
