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
            if (aiController.IsWithinDistanceToEnemy(aiController.AttackRangeSquared))
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
            GD.Print("DefaultMonsterAttack entered");
        }

        public override void Execute(AIController aiController, double delta)
        {
            if (aiController.CanAttack == true)
            {
                if (aiController.IsWithinDistanceToEnemy(aiController.AttackRangeSquared) == false)
                {
                    aiController.ChangeState(new DefaultMonsterRun());
                }
                else
                {
                    GD.Print("DefaultMonsterAttack attack");
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
