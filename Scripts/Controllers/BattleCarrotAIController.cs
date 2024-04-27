using AI;
using Godot;
using Pawns.Monsters;
using Pawns;
using System;
using Components;
using Pawns.BattlePlants.Melee;
namespace Controllers
{
    public partial class BattleCarrotAIController : AIController
    {
        public StateController<AIController> StateMachine { get; set; }
        public override void ChangeState(State<AIController> newState)
        {
            StateMachine.ChangeState(newState);
        }
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
            StateMachine.CurrentState = new BattleCarrotIdle();
            StateMachine.CurrentState.Enter(this);

        }

        private void Pawn_Died()
        {
            QueueFree();
        }

        public override void UpdateAI(double delta)
        {
            StateMachine.Update(delta);
        }

        public override bool CanDealDamageToEnemy()
        {
            DamageArea damageArea = (Pawn as BattleCarrot).ForwardDamageArea;
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
