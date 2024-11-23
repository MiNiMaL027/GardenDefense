using Components;
using Components.PawnStats;
using Godot;
using System;
namespace Pawns.BattlePlants.Melee
{
    public partial class BattleCarrot : BaseBattlePlant
    {
        public DamageArea ForwardDamageArea { get; set; }
        public override void _Ready()
        {
            Animation = GetNode<AnimationPlayerBasicCallbacks>("BattleCarrot/AnimationPlayer");
            Animation.AnimationFinished += Animation_AnimationFinished;

            RotateY(Mathf.Pi / 2);
            HealthBar3D = GetNode<ProgressBar3D>("HealthBar3D");

            base._Ready();
            ConnectHitBoxes(this);
            ForwardDamageArea = GetNode<DamageArea>("ForwardDamageArea");
            ForwardDamageArea.Damage = StatsComponent.GetStrength();
            ForwardDamageArea.AreaOwner = this;
        }

        private void Animation_AnimationFinished(StringName animName)
        {
            if(animName == AnimationNames.Attack)
            {
                WeaponBoxEndAttack();
            }
        }

        private void Animation_AttackEnded()
        {
            
        }
        public void WeaponBoxStartAttack()
        {
            ForwardDamageArea.Enable();
        }
        public void WeaponBoxEndAttack()
        {
            ForwardDamageArea.Disable();
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
                    Animation.Play(AnimationNames.Attack);
                    WeaponBoxStartAttack();
                }
            }
        }
        public override void InitializeStats()
        {
            PawnStats = new Stats()
            {
                MaxHealth = 150,
                Strength = 20,
                AttackSpeed = 1,
                AttackRange = 1
            };
        }
    }
}

