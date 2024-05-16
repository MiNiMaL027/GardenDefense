using Projectiles;
using System;
using Enums;
using Godot;
using Components.PawnStats;
using Components;
using static Scenes;

namespace Pawns.BattlePlants.Range
{
    public partial class BattlePea : RangeBattlePlant
    {
        public override void _Ready()
        {
            ProjectileSpawnPosition = GetNode<Node3D>("BattlePea/Арматура/Skeleton3D/ProjectileSpawnBoneAttachment/ProjectileSpawnPosition");
            Animation = GetNode<AnimationPlayerBasicCallbacks>("BattlePea/AnimationPlayer");
            Animation.ProjectileSpawn += Animation_ProjectileSpawn;
            Animation.AnimationFinished += Animation_AnimationFinished;

            RotateY(Mathf.Pi / 2);
            HealthBar3D = GetNode<ProgressBar3D>("HealthBar3D");

            base._Ready();
            ConnectHitBoxes(this);
        }

        public void Animation_ProjectileSpawn()
        {
            Random rnd = new Random();

            if (rnd.Next(0, 100) <= 11)
            {
                Projectile projectile = Scenes.Projectiles.BattlePea.PeaAdditionalProjectile();
                GameInstance.World.AddChild(projectile);
                projectile.GlobalPosition = ProjectileSpawnPosition.GlobalPosition;
                Transform3D globalTransform = projectile.GlobalTransform;
                globalTransform.Basis = GlobalTransform.Basis;
                projectile.GlobalTransform = globalTransform;
                ProjectileParameters p = new ProjectileParameters()
                {
                    Owner = this,
                    DamageAreaType = DamageAreaType.Damage,
                    AttackModify = AttackModify.Knockback,
                    CountDamage = StatsComponent.GetStrength() * 2,
                    MaxTargets = 1,
                    InitialSpeed = 6,
                    KnockbackDistance = 1,
                    MaxDistanceOfProjectile = 5
                };
                projectile.FullInit(p);
            }
            else
            {
                Projectile projectile = Scenes.Projectiles.BattlePea.PeaMainProjectile();
                GameInstance.World.AddChild(projectile);
                projectile.GlobalPosition = ProjectileSpawnPosition.GlobalPosition;
                Transform3D globalTransform = projectile.GlobalTransform;
                globalTransform.Basis = GlobalTransform.Basis;
                projectile.GlobalTransform = globalTransform;
                ProjectileParameters p = new ProjectileParameters()
                {
                    Owner = this,
                    DamageAreaType = DamageAreaType.Damage,
                    AttackModify = AttackModify.Knockback,
                    CountDamage = StatsComponent.GetStrength(),
                    MaxTargets = 1,
                    InitialSpeed = 10,
                    KnockbackDistance = 0,
                    MaxDistanceOfProjectile = 5
                };
                projectile.FullInit(p);
            }
        }

        public override void InitializeStats()
        {
            PawnStats = new Stats()
            {
                MaxHealth = 100,
                Strength = 10,
                AttackSpeed = 1f,
                AttackRange = 5f
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
        private void Animation_AnimationFinished(StringName animName)
        {
            if (animName == AnimationNames.Attack)
            {
                IsAttacking = false;
                Controller.CanAttack = true;
            }
        }
    }
}
