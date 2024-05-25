using Components;
using Components.PawnStats;
using Enums;
using Godot;

namespace Pawns.Monsters
{
    public partial class BaseMonster : Pawn
    {
        public int DifficultyLevel { get; set; }
        public MovementComponent MovementComponent { get; set; }
        public override void _Ready()
        {
            MovementComponent = GetNode<MovementComponent>("MovementComponent");
            MovementComponent.Init(this);
            MovementComponent.MovementInfo += MovementComponent_MovementInfo;
            base._Ready();


        }
        public override void InitializeStatsComponent()
        {
            base.InitializeStatsComponent();

            //MovementComponent.MaxSpeed = (PawnStats as MonsterStats).MovementSpeed;
            
        }
        private void MovementComponent_MovementInfo(Vector3 velocity, bool grounded)
        {
            if (velocity != Vector3.Zero)
            {
                AnimationNodeStateMachinePlayback.Travel(AnimationStates.Moving);
                GD.Print(velocity.Length());
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
