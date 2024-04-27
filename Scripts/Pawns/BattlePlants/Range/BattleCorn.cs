using Components;
using Components.PawnStats;
using Enums;
using Godot;
using Pawns.BattlePlants;
using Projectiles;
using System;
using System.Linq;
namespace Pawns.BattlePlants.Range
{
    public partial class BattleCorn : RangeBattlePlant
    {
        public Pawn ClosestEnemy;
        public override void _Ready()
        {
            Animation = GetNode<AnimationPlayerBasicCallbacks>("BattleCorn/AnimationPlayer");
            Animation.ProjectileSpawn += Animation_ProjectileSpawn;
            Animation.AnimationFinished += Animation_AnimationFinished;
            ProjectileSpawnPosition = GetNode<Node3D>("BattleCorn/Арматура/Skeleton3D/ProjectileSpawnAttachment3D/ProjectileSpawnPosition");
            RotateY(Mathf.Pi / 2);
            HealthBar3D = GetNode<ProgressBar3D>("HealthBar3D");

            base._Ready();
            ConnectHitBoxes(this);
        }

        private void Animation_AnimationFinished(StringName animName)
        {
            if(animName== AnimationNames.Attack)
            {
                IsAttacking = false;
                Controller.CanAttack = true;
            }
        }

        public void Animation_ProjectileSpawn()
        {
            SelfAimingProjectile projectile = Scenes.Projectiles.BattleCorn.CornMainProjectile();
            GameInstance.World.AddChild(projectile);
            projectile.GlobalPosition = ProjectileSpawnPosition.GlobalPosition;
            Transform3D globalTransform = projectile.GlobalTransform;
            globalTransform.Basis = GlobalTransform.Basis;
            projectile.GlobalTransform = globalTransform;
            ProjectileParameters p = new ProjectileParameters()
            {
                Owner = this,
                DamageAreaType = DamageAreaType.Damage,
                AttackModify = AttackModify.Simple,
                CountDamage = StatsComponent.GetStrength(),
                MaxTargets = 1,
                InitialSpeed = 10,
                MaxDistanceOfProjectile = 20,
                Target = ClosestEnemy?.HitBoxes.FirstOrDefault()
            };
            projectile.FullInit(p);
        }

        public override void InitializeStats()
        {
            PawnStats = new Stats()
            {
                MaxHealth = 100,
                Strength = 10,
                AttackSpeed = 1f,
                AttackRange = 20f
            };
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
                }
            }
        }
    }

}
