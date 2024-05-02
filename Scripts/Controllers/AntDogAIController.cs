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
            StateMachine.CurrentState = new DefaultMonsterRun();
            StateMachine.CurrentState.Enter(this);
        }

        private void deathListener()
        {
            QueueFree();
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
