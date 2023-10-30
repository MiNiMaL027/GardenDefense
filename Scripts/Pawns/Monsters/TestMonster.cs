using Components;
using Components.PawnStats;
using Godot;

namespace Pawns.Monsters
{
    public partial class TestMonster : BaseMonster
    {
        public DamageArea DamageArea { get; set; }
        public Timer TimerAttack { get; set; }
        public override void _Ready()
        {
            RotateY(-Mathf.Pi / 2);
            AddToGroup(Groups.Pawn);
            MovementComponent = GetNode<MovementComponent>("MovementComponent");
            MovementComponent.Init(this);
            Mesh = GetNode<Node3D>("MeshInstance3D");
            Animation = GetNode<AnimationPlayer>("AnimationPlayer");

            StatsComponent = GetNode<StatsComponent>("StatsComponent");
            StatsComponent.HealthBelowZero += healthBelowZeroListener;
            TimerAttack = GetNode<Timer>("TimerAttack");
            TimerAttack.WaitTime = PawnStats.AttackSpeed;
            TimerAttack.Timeout += TimerAttack_Timeout;
            InitializeStatsComponent();
            DamageArea = GetNode<DamageArea>("DamageArea");
            DamageArea.Damage = StatsComponent.GetStrength();
            DamageArea.AreaOwner=this;
            ConnectHitBoxes(this);
        }
        public override void InitializeStats()
        {
            PawnStats = new MonsterStats()
            {
                MaxHealth = 100,
                Strength = 10,
                AttackSpeed = 0.5f,
                AttackRange = 1.8f,
                MovementSpeed=10
            };
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

}
