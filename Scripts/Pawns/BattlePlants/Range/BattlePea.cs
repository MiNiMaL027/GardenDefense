using Projectiles;
using System;
using Enums;
using Godot;

namespace Pawns.BattlePlants.Range
{
    public partial class BattlePea : RangeBattlePlant
    {
        public override void _Ready()
        {   
            AttackType = Enums.AttackType.Earn;
            ProjectileCount = 1000;
            TimeToGrow = 2;
            RotateY(Mathf.Pi / 2);
            base._Ready();
            ConnectHitBoxes(this);
        }

        public override void Attack()
        {       
            Random rnd = new Random();

            if (rnd.Next(0,100) <= 11)
            {
                Projectile additional = Scenes.Projectiles.BattlePea.PeaAdditionalProjectile();
                GameInstance.World.AddChild(additional);
                additional.GlobalTransform = ProjectileSpawnPosition.GlobalTransform;
                additional.FullInit(this, DamageAreaType.Damage, AttackModify.Simple, StatsComponent.GetStrength() * 2, 1, 2);
            }
            else
            {
                Projectile standart = Scenes.Projectiles.BattlePea.PeaMainProjectile();
                GameInstance.World.AddChild(standart);
                standart.GlobalTransform = ProjectileSpawnPosition.GlobalTransform;
                standart.FullInit(this, DamageAreaType.Damage, AttackModify.Simple, StatsComponent.GetStrength(), 1, 1);
            }
        }
    }
}
