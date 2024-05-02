using Controllers;

namespace AI
{
    public partial class DefaultPlantIdle : State<AIController>
    {
        public override void Enter(AIController aiController)
        {
        }

        public override void Execute(AIController aiController, double delta)
        {
            if (aiController.CanDealDamageToEnemy())
            {
                aiController.ChangeState(new DefaultPlantAttack());
            }
        }

        public override void Exit(AIController aiController)
        {
        }
    }
    public partial class DefaultPlantAttack : State<AIController>
    {
        public override void Enter(AIController aiController)
        {
        }
        public override void Execute(AIController aiController, double delta)
        {
            if (aiController.CanAttack == true)
            {
                if (aiController.CanDealDamageToEnemy() == false)
                {
                    aiController.ChangeState(new DefaultPlantIdle());
                }
                else
                {
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
