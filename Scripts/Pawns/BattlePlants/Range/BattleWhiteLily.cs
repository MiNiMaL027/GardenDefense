using Components;
using Components.PawnStats;
using Enums;
using Godot;
using Projectiles;
namespace Pawns.BattlePlants.Range
{
    public partial class BattleWhiteLily : RangeBattlePlant
    {
        public Pawn Target;
        public override void _Ready()
        {
            Animation = GetNode<AnimationPlayerBasicCallbacks>("BattleLily/AnimationPlayer");
            Animation.ProjectileSpawn += Animation_ProjectileSpawn;
            Animation.AnimationFinished += Animation_AnimationFinished;
            ProjectileSpawnPosition = GetNode<Node3D>("BattleLily/Арматура/Skeleton3D/ProjectileSpawnAttachment3D/ProjectileSpawnPosition");
            RotateY(Mathf.Pi / 2);
            HealthBar3D = GetNode<ProgressBar3D>("HealthBar3D");

            base._Ready();
            ConnectHitBoxes(this);
        }

        private void Animation_AnimationFinished(StringName animName)
        {
            if (animName == AnimationNames.Attack)
            {
                IsAttacking = false;
                Controller.CanAttack = true;
            }
        }

        public void Animation_ProjectileSpawn()
        {
            BallisticProjectile projectile = Scenes.Projectiles.WhiteLily.WhiteLilyMainProjectile();
            GameInstance.World.AddChild(projectile);
            projectile.GlobalPosition = ProjectileSpawnPosition.GlobalPosition;
            Transform3D globalTransform = projectile.GlobalTransform;
            globalTransform.Basis = GlobalTransform.Basis;
            projectile.GlobalTransform = globalTransform;
            BallisticProjectileParameters p = new BallisticProjectileParameters()
            {
                Owner = this,
                DamageAreaType = DamageAreaType.Heal,
                AttackModify = AttackModify.Simple,
                CountDamage = StatsComponent.GetStrength(),
                Target = Target?.GlobalPosition,
                TargetPawn=Target
            };
            projectile.FullInit(p);
        }

        public override void InitializeStats()
        {
            PawnStats = new Stats()
            {
                MaxHealth = 100,
                Strength = 30,
                AttackSpeed = 1,
                AttackRange = 1
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

