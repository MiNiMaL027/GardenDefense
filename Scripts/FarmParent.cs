using Components.PawnStats;
using Godot;
using Pawns.Monsters;

public partial class FarmParent : Node3D
{
    StatsComponent Stats { get; set; }
    Area3D HurtArea { get; set; }
    public HpBar HpBar { get; set; }
    public override void _Ready()
    {
        HurtArea = GetNode<Area3D>("FarmsHurtArea");
        HurtArea.BodyEntered += FarmsHurtArea_BodyEntered;

        AddHpBar();
        Stats = GetNode<StatsComponent>("Stats");

        Stats.HealthUpdated += Stats_HealthUpdated;
        Stats.HealthBelowZero += Stats_HealthBelowZero;

        Stats.SetMaxHealth(100);
        Stats.SetCurrentHealth(100);
    }

    private void Stats_HealthBelowZero()
    {
        GD.Print("GameOver");
    }

    private void Stats_HealthUpdated(int currentHealth, int maxHealth)
    {
        HpBar.Refresh(currentHealth, maxHealth);
    }

    private void FarmsHurtArea_BodyEntered(Node3D body)
    {      
        if(body is BaseMonster monster)
        {
            Stats.SetCurrentHealth(Stats.GetCurrentHealth() - monster.StatsComponent.GetBaseStrength());
            monster.QueueFree();
        }
    }

    public void AddHpBar()
    {
        HpBar = Scenes.Widgets.GardenWidgets.HpBar();
        HpBar.AnchorsPreset = 5;
        HpBar.Position = new Vector2(HpBar.Position.X, 20);
        AddChild(HpBar);
    }
}
