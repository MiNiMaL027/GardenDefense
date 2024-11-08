using Controllers;
using Pawns.BattlePlants.Range;

namespace AI
{
    public partial class BattleWhiteLilyIdle : State<AIController>
    {
        public override void Enter(AIController aiController)
        {
        }

        public override void Execute(AIController aiController, double delta)
        {
            if (aiController.LineOfSightBodies.Count > 0)
            {
                aiController.ChangeState(new BattleWhiteLilyAttack());
            }
        }

        public override void Exit(AIController aiController)
        {
        }
    }
    public partial class BattleWhiteLilyAttack : State<AIController>
    {
        BattleWhiteLily battleWhiteLily;
        public override void Enter(AIController aiController)
        {
            BattleWhiteLily baseBattlePlant = aiController.Pawn as BattleWhiteLily;
            battleWhiteLily = baseBattlePlant;
        }
        public override void Execute(AIController aiController, double delta)
        {
            if (aiController.CanAttack == true)
            {
                if (aiController.CanDealDamageToEnemy() == false)
                {
                    aiController.ChangeState(new BattleWhiteLilyIdle());
                }
                else
                {
                    battleWhiteLily.ClosestTarget = aiController.GetClosestEnemy();
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
