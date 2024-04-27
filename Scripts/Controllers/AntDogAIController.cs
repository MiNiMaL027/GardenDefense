using AI;
using Components;
using Godot;
using Pawns;
using Pawns.BattlePlants;
using Pawns.Monsters;
namespace Controllers
{
    public partial class AntDogAIController : AIController
    {
        public StateController<AIController> StateMachine { get; set; }
        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            EnemyType = typeof(BaseBattlePlant);
            AreaLineOfSight = GetNode<Area3D>("AntDog/AreaLineOfSight");
            Pawn = GetNode<Pawn>("AntDog");
            AttackRangeSquared = Pawn.PawnStats.AttackRange * Pawn.PawnStats.AttackRange;

            Pawn.Died += deathListener;
            Pawn.Controller = this;
            AreaLineOfSight.BodyEntered += AreaLineOfSight_BodyEntered;
            AreaLineOfSight.BodyExited += AreaLineOfSight_BodyExited;
            StateMachine = new StateController<AIController>(this);
            StateMachine.CurrentState = new AntRun();
            StateMachine.CurrentState.Enter(this);
        }

        private void deathListener()
        {
            QueueFree();
        }


        public override void UpdateAI(double delta)
        {
            StateMachine.Update(delta);
        }

        public override void ChangeState(State<AIController> newState)
        {
            StateMachine.ChangeState(newState);
        }
        public override bool CanDealDamageToEnemy()
        {
            DamageArea damageArea = (Pawn as AntDog).DamageArea;
            foreach (Node3D n in LineOfSightBodies)
            {
                if (n is Pawn p)
                {
                    foreach (HitBoxArea hitBoxArea in p.HitBoxes)
                    {
                        if (hitBoxArea.OverlapsArea(damageArea))
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
