using Components;
using Godot;
using Pawns.Monsters;

public partial class TestMonster : BaseMonster
{
    public DamageArea DamageArea { get; set; }
    public Timer TimerAttack { get; set; }
    public override void _Ready()
    {
        RotateY(-Mathf.Pi / 2);
        AddToGroup(Groups.Pawn);
        MovementComponent = GetNode<MovementComponent>("MovementComponent");
        DamageArea = GetNode<DamageArea>("DamageArea");
        MovementComponent.Init(this);
        Mesh = GetNode<Node3D>("MeshInstance3D");
        Animation = GetNode<AnimationPlayer>("AnimationPlayer");

        StatsComponent = GetNode<StatsComponent>("StatsComponent");
        StatsComponent.HealthBelowZero += healthBelowZeroListener;
        TimerAttack = GetNode<Timer>("TimerAttack");
        TimerAttack.Timeout += TimerAttack_Timeout;
        InitializeStats();
        ConnectHitBoxes(this);
    }

    private void TimerAttack_Timeout()
    {
        WeaponBoxEndAttack();
    }

    public void WeaponBoxStartAttack()
    {
        DamageArea.Enable();
    }
    public void WeaponBoxEndAttack()
    {
        DamageArea.Disable();
        IsAttacking = false;
        Controller.CanAttack = true;
    }
    public override bool IsAttacking
    {
        get => isAttacking;
        set
        {
            isAttacking = value;
            if (isAttacking == true)
            {
                WeaponBoxStartAttack();
                TimerAttack.Start();
            }
        }
    }
}
