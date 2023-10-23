using AI;
using Godot;
using Pawns;
using Pawns.Monsters;

namespace Controllers
{
    public partial class BattlePlantAIController : AIController
    {
        public StateController<AIController> StateMachine { get; set; }
        public override void ChangeState(State<AIController> newState)
        {
            StateMachine.ChangeState(newState);
        }
        public override void _Ready()
        {
            base._Ready();
            Pawn = GetNode<Pawn>("BattlePlant");
            AreaLineOfSight = GetNode<Area3D>("BattlePlant/AreaLineOfSight");
            Pawn.Died += Pawn_Died;
            Pawn.Controller= this;

            StateMachine = new StateController<AIController>(this);
            StateMachine.CurrentState = new DefaultBattlePlantIdle();
            StateMachine.CurrentState.Enter(this);
            AreaLineOfSight.BodyEntered += AreaLineOfSight_BodyEntered;
            AreaLineOfSight.BodyExited += AreaLineOfSight_BodyExited;
        }

        private void AreaLineOfSight_BodyExited(Node3D body)
        {
            if (body is BaseMonster baseMonster)
            {
                LineOfSightBodies.Remove(baseMonster);
            }
        }

        private void AreaLineOfSight_BodyEntered(Node3D body)
        {
            if (body is BaseMonster baseMonster)
            {
                LineOfSightBodies.Add(baseMonster);
            }
        }

        private void Pawn_Died()
        {
            QueueFree();
        }

        public override void UpdateAI(double delta)
        {
            StateMachine.Update(delta);
        }
    }
}
