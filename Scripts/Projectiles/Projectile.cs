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

        private Vector3 initPosition;

        public override void _Ready()
        {
        }
        /// <summary>
        /// Manually set all properties. Use Init function if you want to read properties from scene
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="damageAreaType"></param>
        /// <param name="attackModify"></param>
        /// <param name="damageToSet"></param>
        /// <param name="maxTargetsToSet"></param>
        /// <param name="initialSpeedToSet"></param>
        /// <param name="maxDistanceOfProjectile"></param>
        public void FullInit(Pawn owner, DamageAreaType damageAreaType, AttackModify attackModify, int damageToSet, int maxTargetsToSet, int initialSpeedToSet, double maxDistanceOfProjectile = Projectile.DefaultMaxDistanceOfProjectile)
        {
            AreaOwner = owner;
            DamageAreaType = damageAreaType;
            AttackModify = attackModify;
            Damage = damageToSet;
            SquaredMaxDistanceOfProjectile = maxDistanceOfProjectile * maxDistanceOfProjectile;
            MaxTargets = maxTargetsToSet;
            InitialSpeed = initialSpeedToSet;
            initPosition = GlobalPosition;
            Enable();
        }
        /// <summary>
        /// Use properties from scene. Use FullInit function to set properties manually
        /// </summary>
        /// <param name="owner"></param>
        public void Init(Pawn owner)
        {
            AreaOwner = owner;
            initPosition = GlobalPosition;
            Enable();
        }
        public override void Enable()
        {
            base.Enable();
            SetProcess(true);
        }
        public override void Disable()
        {
            base.Disable();
            SetProcess(false);
        }
        public override void areaEnteredListener(Area3D a)
        {
            if (a is HitBoxArea hitBox)
            {
                if (hitBox.AreaOwner != this.AreaOwner && pawnsDamageDealt.Contains(hitBox.AreaOwner) == false && hitBox.AreaOwner.IsDead == false)
                {
                    if (DamageAreaType == DamageAreaType.Damage)
                    {
                        AreaOwner.DealDamage(hitBox.AreaOwner, Damage, AttackModify);
                    }
                    else if (DamageAreaType == DamageAreaType.Heal)
                    {
                        AreaOwner.Heal(hitBox.AreaOwner, Damage);
                    }
                    pawnsDamageDealt.Add(hitBox.AreaOwner);

                    if (pawnsDamageDealt.Count >= MaxTargets)
                    {
                        QueueFree();
                    }
                }
            }
        }

        public override void _Process(double delta)
        {
            GlobalTranslate(GlobalTransform.Basis.Z * InitialSpeed * (float)delta);

            if (GlobalPosition.DistanceSquaredTo(initPosition) >= SquaredMaxDistanceOfProjectile)
            {
                QueueFree();
            }
        }
    }

}
