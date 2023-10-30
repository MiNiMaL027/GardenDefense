using AI;
using Godot;
using Pawns;
using System;
using System.Collections.Generic;

namespace Controllers
{
    public abstract partial class AIController : Node3D
    {
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
        public abstract void UpdateAI(double delta);
        public override void _Process(double delta)
        {
            base._Process(delta);
            UpdateAI(delta);
        }
        public abstract void ChangeState(State<AIController> newState);
        public bool IsWithinDistanceToEnemy(float distance)
        {
            foreach(Node3D n in LineOfSightBodies)
            {
                if (Pawn.GlobalPosition.DistanceSquaredTo(n.GlobalPosition)<= distance)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
