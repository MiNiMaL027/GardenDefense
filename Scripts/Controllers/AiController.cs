using AI;
using Godot;
using Pawns;
using Pawns.Monsters;
using System;
using System.Collections.Generic;

namespace Controllers
{
    public abstract partial class AIController : Node3D
    {
        public StateController<AIController> StateMachine { get; set; }
        public void ChangeState(State<AIController> newState)
        {
            StateMachine.ChangeState(newState);
        }
        public Pawn Pawn { get; set; }
        public Type EnemyType { get; set; }
        public Area3D AreaLineOfSight { get; set; }
        protected virtual void AreaLineOfSight_BodyExited(Node3D body)
        {
            if (body.GetType().IsSubclassOf(EnemyType))
            {
                LineOfSightBodies.Remove(body);
            }
        }
        protected virtual void AreaLineOfSight_BodyEntered(Node3D body)
        {
            if (body.GetType().IsSubclassOf(EnemyType))
            {
                LineOfSightBodies.Add(body);
            }
        }
        public List<Node3D> LineOfSightBodies { get; set; } = new List<Node3D>();

        public float AttackRangeSquared;
        public bool CanAttack { get; set; } = true;
        public void UpdateAI(double delta)
        {
            StateMachine.Update(delta);
        }
        public override void _Process(double delta)
        {
            base._Process(delta);
            UpdateAI(delta);
        }
        public bool IsWithinDistanceToEnemy(float squaredDistance)
        {
            foreach(Node3D n in LineOfSightBodies)
            {
                if (Pawn.GlobalPosition.DistanceSquaredTo(n.GlobalPosition)<= squaredDistance)
                {
                    return true;
                }
            }
            return false;
        }
        public Pawn GetClosestEnemy()
        {
            double minRange = Double.MaxValue;
            Pawn closestEnemy = null;
            foreach (Node3D n in LineOfSightBodies)
            {
                if (n.GetType().IsSubclassOf(EnemyType))
                {
                    Pawn p = n as Pawn;
                    double range = Pawn.GlobalPosition.DistanceSquaredTo(p.GlobalPosition);
                    if (range < minRange)
                    {
                        minRange = range;
                        closestEnemy = p;
                    }
                }
            }
            return closestEnemy;
        }
        public abstract bool CanDealDamageToEnemy();
    }
}
