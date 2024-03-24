using Godot;
using Controllers;
using Pawns.BattlePlants;

namespace AI
{
    public partial class DefaultBattlePlantIdle : State<AIController>
    {
        public override void Enter(AIController aiController)
        {
        }

        public override void Execute(AIController aiController, double delta)
        {
            if (aiController.LineOfSightBodies.Count > 0)
            {
                aiController.ChangeState(new DefaultBattlePlantAttack());
            }
        }

        public override void Exit(AIController aiController)
        {
        }
    }
    public partial class DefaultBattlePlantAttack : State<AIController>
    {
        public override void Enter(AIController aiController)
        {
            BaseBattlePlant baseBattlePlant = aiController.Pawn as BaseBattlePlant;
            baseBattlePlant.StartAttack();
        }
        public override void Execute(AIController aiController, double delta)
        {
            if (aiController.LineOfSightBodies.Count <= 0)
            {
                aiController.ChangeState(new DefaultBattlePlantIdle());
            }
        }
        public override void Exit(AIController aiController)
        {
            BaseBattlePlant baseBattlePlant = aiController.Pawn as BaseBattlePlant;
            baseBattlePlant.StopAttack();
        }
    }
}
