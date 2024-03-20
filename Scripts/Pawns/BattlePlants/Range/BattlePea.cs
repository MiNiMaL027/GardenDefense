using Projectiles;
using System;
using Enums;
using Godot;
using Components.PawnStats;

namespace Pawns.BattlePlants.Range
{
    public partial class BattlePea : RangeBattlePlant
    {
        public override void _Ready()
        {
            Animation = GetNode<AnimationPlayerBasicCallbacks>("BattlePea/AnimationPlayer");
            Animation.ProjectileSpawn += Animation_ProjectileSpawn;
            ProjectileSpawnPosition = GetNode<Node3D>("BattlePea/Арматура/Skeleton3D/ProjectileSpawnBoneAttachment/ProjectileSpawnPosition");
            AttackType = Enums.AttackType.Earn;
            ProjectileCount = 1000;
            TimeToGrow = 2;
            RotateY(Mathf.Pi / 2);
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
                projectile.FullInit(this, DamageAreaType.Damage, AttackModify.Simple, StatsComponent.GetStrength() * 2, 1, 4);
            }
            else
            {
                Projectile projectile = Scenes.Projectiles.BattlePea.PeaMainProjectile();
                GameInstance.World.AddChild(projectile);
                projectile.GlobalPosition = ProjectileSpawnPosition.GlobalPosition;
                Transform3D globalTransform = projectile.GlobalTransform;
                globalTransform.Basis = GlobalTransform.Basis;
                projectile.GlobalTransform = globalTransform;
                projectile.FullInit(this, DamageAreaType.Damage, AttackModify.Simple, StatsComponent.GetStrength(), 1, 2);
            }
            //Random rnd = new Random();

            //Projectile projectile = rnd.Next(0, 100) <= 11 ?
            //    Scenes.Projectiles.BattlePea.PeaAdditionalProjectile() :
            //    Scenes.Projectiles.BattlePea.PeaMainProjectile();

            //GameInstance.World.AddChild(projectile);

            //projectile.GlobalPosition = ProjectileSpawnPosition.GlobalPosition;

            //Transform3D globalTransform = projectile.GlobalTransform;
            //globalTransform.Basis = GlobalTransform.Basis;
            //projectile.GlobalTransform = globalTransform;

            //projectile.FullInit(this, DamageAreaType.Damage, AttackModify.Simple, StatsComponent.GetStrength(), 1, 1);
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
        public override void Attack()
        {
            Animation.Play(AnimationNames.Attack);
        }
    }
}
