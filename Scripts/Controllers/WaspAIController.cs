using AI;
using Components;
using Godot;
using Pawns.BattlePlants;
using Pawns.Monsters;
using Pawns;
using System;
namespace Controllers
{
    public partial class WaspAIController : AIController
    {
        public StateController<AIController> StateMachine { get; set; }
        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            EnemyType = typeof(BaseBattlePlant);
            AreaLineOfSight = GetNode<Area3D>("Wasp/AreaLineOfSight");
            Pawn = GetNode<Pawn>("Wasp");
            AttackRangeSquared = Pawn.PawnStats.AttackRange * Pawn.PawnStats.AttackRange;

            Pawn.Died += deathListener;
            Pawn.Controller = this;
            AreaLineOfSight.BodyEntered += AreaLineOfSight_BodyEntered;
            AreaLineOfSight.BodyExited += AreaLineOfSight_BodyExited;
            StateMachine = new StateController<AIController>(this);
            StateMachine.CurrentState = new DefaultMonsterRun();
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
            return IsWithinDistanceToEnemy(AttackRangeSquared);
        }
    }
}
