using Farm.Scripts.Controllers;
using Farm.Scripts.Enums;
using Farm.Scripts.Interfaces;
using Godot;

namespace Farm.Scripts.Components
{
    public abstract partial class Pawn : RigidBody3D, IAttacking
    {
        public AiController Controller { get; set; }
        public int MaxHp { get; set; }
        public HealthComponent HealthComponent { get; set; }
        public AnimationPlayer Animation { get; set; }
        public int Damage { get; set; }
        public int AttackSpeed { get; set; }
        public HitBoxArea HitBox { get; set; }
        public RangeArea RangeArea { get; set; }
        public AttackModify AttackModify { get; set; }

        public virtual void init()
        {
            HealthComponent = new HealthComponent();
            HealthComponent.Init(this);
            HitBox = GetNode<HitBoxArea>("hit_box_area");
            HitBox.Init(this);
            Animation = GetNode<AnimationPlayer>("AnimationPlayer");
            RangeArea = GetNode<RangeArea>("attack_range_area");
            RangeArea.Init(this);
        }
    }
}
