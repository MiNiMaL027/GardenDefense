using Components;
using Components.PawnStats;
using Godot;
namespace Pawns.Monsters
{
    public partial class AntDog : BaseMonster
    {
        public DamageArea DamageArea { get; set; }
        public override void _Ready()
        {
            Animation = GetNode<AnimationPlayerBasicCallbacks>("AntDog/AnimationPlayer");
            Animation.AttackEnded += Animation_AttackEnded;
            AnimationTree = GetNode<AnimationTree>("AntDog/AnimationTree");
            AnimationNodeStateMachinePlayback = AnimationTree.Get("parameters/playback").As<AnimationNodeStateMachinePlayback>();
            AnimationNodeStateMachinePlayback.Travel(AnimationStates.Idle);
            HealthBar3D = GetNode<ProgressBar3D>("HealthBar3D");


            base._Ready();
            RotateY(-Mathf.Pi / 2);
            Mesh = GetNode<Node3D>("AntDog");

            DamageArea = GetNode<DamageArea>("DamageArea");
            DamageArea.Damage = StatsComponent.GetStrength();
            DamageArea.AreaOwner = this;
            ConnectHitBoxes(this);
        }

        private void Animation_AttackEnded()
        {
            WeaponBoxEndAttack();
        }

        public override void InitializeStats()
        {
            PawnStats = new MonsterStats()
            {
                MaxHealth = 100,
                Strength = 10,
                AttackSpeed = 0.5f,
                AttackRange = 1.8f,
                MovementSpeed = 3
            };

            DifficultyLevel = 1;
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
                    AnimationTree.Set("parameters/Idle/OneShotAttack/request", (int)AnimationNodeOneShot.OneShotRequest.Fire);
                    AnimationTree.Set("parameters/Moving/OneShotAttack/request", (int)AnimationNodeOneShot.OneShotRequest.Fire);
                }
            }
        }
    }
}

