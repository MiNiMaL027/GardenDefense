using Components;
using Enums;
using Godot;

namespace Pawns.Monsters
{
    public partial class BaseMonster : Pawn
    {
        protected bool isAttacking = false;
        public virtual bool IsAttacking
        {
            get
            {
                return isAttacking;
            }
            set
            {
                isAttacking = false;
            }
        }
        public MovementComponent MovementComponent { get; set; }
        public override void BestiaryReady()
        {
            base.BestiaryReady();
            MovementComponent = GetNode<MovementComponent>("MovementComponent");
        }
        public override void _Ready()
        {
            base._Ready();
            MovementComponent = GetNode<MovementComponent>("MovementComponent");
            MovementComponent.Init(this);
        }
        protected override void healthBelowZeroListener()
        {
            base.healthBelowZeroListener();
            MovementComponent.Freeze();
        }
        public override void ApplyDamage(int countDamage, AttackModify attackModify)
        {
            base.ApplyDamage(countDamage, attackModify);
            if (attackModify == AttackModify.Knockback)
            {
                GlobalTranslate(-GlobalTransform.Basis.Z * 1);
            }

        }
    }
}
