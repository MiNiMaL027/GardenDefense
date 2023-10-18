using System;

namespace Farm.Scripts.BattlePlants.Range
{
    public partial class BattlePea : RangeBattlePlant
    {
        string AditionalProjectilePath { get; set; }

        public override void _Ready()
        {          
            Damage = 10;
            AttackType = Enums.AttackType.Earn;
            AttackRange = 5;
            AttackSpeed = 3;
            MaxHp = 20;
            ProjectileCount = 1000;
            TimeToGrow = 2;
            AttackModify = Enums.AttackModify.Simple;

            MainProjectilePath = "res://Scenes/Projectailes/PeaMainProjectile.tscn";
            AditionalProjectilePath = "";

            base._Ready();

            Attack();
            StartAttack();
        }

        public override void Attack()
        {       
            Random rnd = new Random();
            BaseProjectile projectile = Scenes.Battle.Projectile();

            AddChild(projectile);

            if (rnd.Next(0,100) <= 10)
            {
                projectile.Init(AditionalProjectilePath, Damage, AttackModify, AttackRange);
            }
            else
            {
                projectile.Init(MainProjectilePath, Damage, Enums.AttackModify.Knockback, AttackRange);
            }

            //Animation.Play("Attack");

            projectile.Launch();
        }
    }
}
