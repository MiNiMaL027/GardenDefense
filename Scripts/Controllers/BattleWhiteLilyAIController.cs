using AI;
using Godot;
using Pawns.Monsters;
using Pawns;
using System;
using Pawns.BattlePlants;
namespace Controllers
{
    public partial class BattleWhiteLilyAIController : BattlePlantAIController
    {
        public override void _Ready()
        {
            EnemyType = typeof(BaseBattlePlant);
            Pawn = GetNode<Pawn>("BattlePlant");
            AttackRangeSquared = Pawn.PawnStats.AttackRange * Pawn.PawnStats.AttackRange;
            AreaLineOfSight = GetNode<Area3D>("BattlePlant/AreaLineOfSight");
            Pawn.Died += Pawn_Died;
            Pawn.Controller = this;

            AreaLineOfSight.BodyEntered += AreaLineOfSight_BodyEntered;
            AreaLineOfSight.BodyExited += AreaLineOfSight_BodyExited;
            SetProcess(false);

            base._Ready();

        }
        public override void Activated()
        {
            SetProcess(true);

            StateMachine = new StateController<AIController>(this);
            StateMachine.CurrentState = new BattleWhiteLilyIdle();
            StateMachine.CurrentState.Enter(this);
        }
        private void Pawn_Died()
        {
            QueueFree();
        }

        protected override void AreaLineOfSight_BodyEntered(Node3D body)
        {
            if (body != this.Pawn && body.GetType().IsSubclassOf(EnemyType))
            {
                LineOfSightBodies.Add(body);
            }
        }
        public override bool CanDealDamageToEnemy()
        {
            foreach(var b in LineOfSightBodies)
            {
                if(b is BaseBattlePlant plant)
                {
                    if(plant.StatsComponent.GetCurrentHealth() < plant.StatsComponent.GetMaxHealth())
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}

