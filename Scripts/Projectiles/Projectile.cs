using Components;
using Enums;
using Godot;
using Pawns;
using System;
namespace Projectiles
{
    public partial class Projectile : DamageArea
    {
        [Export]
        public int MaxTargets = 1;
        [Export]
        public int InitialSpeed = 750;
        [Export]
        public double SquaredMaxDistanceOfProjectile = 40;
        public const double DefaultMaxDistanceOfProjectile = 20;

        protected Vector3 initPosition;

        public override void _Ready()
        {
        }
        /// <summary>
        /// Manually set all properties. Use Init function if you want to read properties from scene
        /// </summary>
        public virtual void FullInit(ProjectileParameters p)
        {
            AreaOwner = p.Owner;
            DamageAreaType = p.DamageAreaType;
            AttackModify = p.AttackModify;
            Damage = p.CountDamage;
            SquaredMaxDistanceOfProjectile = p.MaxDistanceOfProjectile * p.MaxDistanceOfProjectile;
            MaxTargets = p.MaxTargets;
            InitialSpeed = p.InitialSpeed;
            KnockbackDistance = p.KnockbackDistance;
            initPosition = GlobalPosition;
            Enable();
        }
        /// <summary>
        /// Use properties from scene. Use FullInit function to set properties manually
        /// </summary>
        /// <param name="owner"></param>
        public void Init(ProjectileParameters p)
        {
            AreaOwner = p.Owner;
            initPosition = GlobalPosition;
            Enable();
        }
        public override void Enable()
        {
            base.Enable();
            SetPhysicsProcess(true);
            SetProcess(true);
        }
        public override void Disable()
        {
            base.Disable();
            SetPhysicsProcess(false);
            SetProcess(false);
        }
        public override void areaEnteredListener(Area3D a)
        {
            if (a is HitBoxArea hitBox)
            {
                if (hitBox.AreaOwner != this.AreaOwner && pawnsDamageDealt.Contains(hitBox.AreaOwner) == false && hitBox.AreaOwner.GetType().IsSubclassOf(AreaOwner.Controller.EnemyType) && hitBox.AreaOwner.IsDead == false)
                {
                    hitBox.AreaOwner.LastTouchedPawn = AreaOwner;
                    AreaOwner.DealDamageOrHeal(hitBox.AreaOwner, GetDamageParameters());
                    pawnsDamageDealt.Add(hitBox.AreaOwner);

                    if (pawnsDamageDealt.Count >= MaxTargets)
                    {
                        QueueFree();
                    }
                }
            }
        }
        public override void _PhysicsProcess(double delta)
        {
            Translate(Vector3.Back * InitialSpeed * (float)delta);
        }
        public override void _Process(double delta)
        {
            if (GlobalPosition.DistanceSquaredTo(initPosition) >= SquaredMaxDistanceOfProjectile)
            {
                QueueFree();
            }
        }
    }

}
