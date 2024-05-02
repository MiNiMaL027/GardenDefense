using AI;
using Components;
using Godot;
using Pawns;
using Pawns.BattlePlants.Range;
using Pawns.Monsters;

namespace Controllers
{
    public partial class BattlePeaAIController : AIController
    {
        public override void _Ready()
        {
            EnemyType = typeof(BaseMonster);
            Pawn = GetNode<Pawn>("BattlePlant");
            AttackRangeSquared = Pawn.PawnStats.AttackRange * Pawn.PawnStats.AttackRange;
            AreaLineOfSight = GetNode<Area3D>("BattlePlant/AreaLineOfSight");
            Pawn.Died += Pawn_Died;
            Pawn.Controller= this;

            AreaLineOfSight.BodyEntered += AreaLineOfSight_BodyEntered;
            AreaLineOfSight.BodyExited += AreaLineOfSight_BodyExited;
            StateMachine = new StateController<AIController>(this);
            StateMachine.CurrentState = new DefaultPlantIdle();
            StateMachine.CurrentState.Enter(this);
            
        }

        private void Pawn_Died()
        {
            QueueFree();
        }


        public override bool CanDealDamageToEnemy()
        {
            foreach (Node3D n in LineOfSightBodies)
            {
                if (n is Pawn p)
                {
                    foreach (HitBoxArea hitBoxArea in p.HitBoxes)
                    {
                        if (hitBoxArea.OverlapsArea(AreaLineOfSight))
                        {

                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
