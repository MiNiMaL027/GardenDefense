using AI;
using Godot;
using Pawns;
using Pawns.Monsters;

namespace Controllers
{
    public partial class BattleCornAIController : AIController
    {
        public override void _Ready()
        {
            EnemyType = typeof(BaseMonster);
            Pawn = GetNode<Pawn>("BattlePlant");
            AttackRangeSquared = Pawn.PawnStats.AttackRange * Pawn.PawnStats.AttackRange;
            AreaLineOfSight = GetNode<Area3D>("BattlePlant/AreaLineOfSight");
            Pawn.Died += Pawn_Died;
            Pawn.Controller = this;

            AreaLineOfSight.BodyEntered += AreaLineOfSight_BodyEntered;
            AreaLineOfSight.BodyExited += AreaLineOfSight_BodyExited;
            StateMachine = new StateController<AIController>(this);
            StateMachine.CurrentState = new BattleCornIdle();
            StateMachine.CurrentState.Enter(this);

        }

        private void Pawn_Died()
        {
            QueueFree();
        }


        public override bool CanDealDamageToEnemy()
        {
            return LineOfSightBodies.Count > 0;
        }
    }
}

