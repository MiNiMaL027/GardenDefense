using Controllers;
using Pawns.BattlePlants;
using Pawns.BattlePlants.Range;
using Pawns.Monsters;

namespace AI
{
    public partial class BattleCornIdle : State<AIController>
    {
        public override void Enter(AIController aiController)
        {
        }

        public override void Execute(AIController aiController, double delta)
        {
            if (aiController.LineOfSightBodies.Count > 0)
            {
                aiController.ChangeState(new BattleCornAttack());
            }
        }

        public override void Exit(AIController aiController)
        {
        }
    }
    public partial class BattleCornAttack : State<AIController>
    {
        BattleCorn battleCorn;
        public override void Enter(AIController aiController)
        {
            BattleCorn baseBattlePlant = aiController.Pawn as BattleCorn;
            battleCorn = baseBattlePlant;
        }
        public override void Execute(AIController aiController, double delta)
        {
            if (aiController.CanAttack == true)
            {
                if (aiController.CanDealDamageToEnemy() == false)
                {
                    aiController.ChangeState(new BattleCornIdle());
                }
                else
                {
                    battleCorn.ClosestEnemy = aiController.GetClosestEnemy();
                    aiController.Pawn.IsAttacking = true;
                    aiController.CanAttack = false;
                }
            }
        }
        public override void Exit(AIController aiController)
        {
        }
    }
}
