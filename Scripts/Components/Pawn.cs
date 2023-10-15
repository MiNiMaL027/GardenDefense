using Farm.Scripts.Controllers;
using Farm.Scripts.Enums;
using Farm.Scripts.Interfaces;
using Godot;

namespace Farm.Scripts.Components
{
    public abstract partial class Pawn : Item, IAttacking
    {
        public AiController Controller { get; set; }
        public int MaxHp { get; set; }
        public HealthComponent HealthComponent { get; set; }
        public AnimationPlayer Animations { get; set; }
        public int Damage { get; set; }
        public bool IsAttacking { get; set; }
        public DamageArea? DamageArea { get; set; }
        public int AttackSpeed { get; set; }
        public HitBoxArea HitBox { get; set; }
        public AttackType AttackType { get; set; }

        public virtual void init()
        {
            HealthComponent = GetNode<HealthComponent>("HealthComponent");
            HealthComponent.Init(this);
            HitBox = GetNode<HitBoxArea>("HitBoxArea");
            HitBox.Init(this);
            DamageArea = GetNode<DamageArea>("DamageArea");
            DamageArea.Init(this);
            Animations = GetNode<AnimationPlayer>("AnimationPLayer");
        }
    }
}
