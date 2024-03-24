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
        public override void _Ready()
        {
            base._Ready();
            MovementComponent = GetNode<MovementComponent>("MovementComponent");
            MovementComponent.Init(this);
            MovementComponent.MovementInfo += MovementComponent_MovementInfo;

        }
        private void MovementComponent_MovementInfo(Vector3 velocity, bool grounded)
        {
            if (velocity != Vector3.Zero)
            {
                AnimationNodeStateMachinePlayback.Travel(AnimationStates.Moving);
                AnimationTree.Set("parameters/Moving/BlendSpaceMovementSpeed/blend_position", velocity.Length());
            }
            else
            {
                AnimationNodeStateMachinePlayback.Travel(AnimationStates.Idle);
            }
            AxisLockLinearY = grounded;
        }
        protected override void healthBelowZeroListener()
        {
            base.healthBelowZeroListener();
            MovementComponent.Freeze();
        }
        public override void ApplyDamage(DamageParameters damageParameters)
        {
            if (IsDead == true) { return; }
            base.ApplyDamage(damageParameters);
            if (damageParameters.AttackModify == AttackModify.Knockback)
            {
                Translate(Vector3.Forward*damageParameters.KnockbackDistance);
            }

        }
    }
}
