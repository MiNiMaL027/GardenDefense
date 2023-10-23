using Controllers;
using Godot;
using Pawns.Monsters;
namespace AI
{
    public class DefaultMonsterRun : State<AIController>
    {
        public override void Enter(AIController aiController)
        {
            (aiController.Pawn as BaseMonster).MovementComponent.SetMoveVec(aiController.Pawn.Basis.Z);

        }

        public override void Execute(AIController aiController, double delta)
        {
            if (aiController.IsWithinDistanceToEnemy(aiController.AttackRange, 3))
            {
                aiController.ChangeState(new DefaultMonsterAttack());
            }
        }

        public override void Exit(AIController aiController)
        {
            (aiController.Pawn as BaseMonster).MovementComponent.SetMoveVec(Vector3.Zero);

        }
    }
    public class DefaultMonsterAttack : State<AIController>
    {
        public override void Enter(AIController aiController)
        {
        }

        public override void Execute(AIController aiController, double delta)
        {
            if (aiController.CanAttack == true)
            {
                if (aiController.IsWithinDistanceToEnemy(aiController.AttackRange, 3) == false)
                {
                    aiController.ChangeState(new DefaultMonsterRun());
                }
                else
                {
                    (aiController.Pawn as BaseMonster).IsAttacking = true;
                    aiController.CanAttack = false;
                }
            }

        }

        public override void Exit(AIController aiController)
        {
        }
    }
}
