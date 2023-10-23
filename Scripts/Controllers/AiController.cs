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
        public Area3D AreaLineOfSight { get; set; }
        public List<Node3D> LineOfSightBodies { get; set; } = new List<Node3D>();

        [Export]
        public float AttackRange = 2.5f;
        public bool CanAttack { get; set; } = true;
        public abstract void UpdateAI(double delta);
        public override void _Process(double delta)
        {
            base._Process(delta);
            UpdateAI(delta);
        }
        public abstract void ChangeState(State<AIController> newState);
        public bool IsWithinDistanceToEnemy(float distance, uint targetCollisionLayer)
        {
            Vector3 ourPos = Pawn.GlobalTransform.Origin;
            Vector3 targetPos = Pawn.GlobalTransform.Origin + Pawn.Basis.Z * distance;
            PhysicsDirectSpaceState3D spaceState = GetWorld3D().DirectSpaceState;
            PhysicsRayQueryParameters3D physicsRayQueryParameters3D = new PhysicsRayQueryParameters3D();
            physicsRayQueryParameters3D.From = ourPos;
            physicsRayQueryParameters3D.To = targetPos;
            physicsRayQueryParameters3D.CollisionMask = targetCollisionLayer;
            physicsRayQueryParameters3D.CollideWithBodies = true;


            var result = spaceState.IntersectRay(physicsRayQueryParameters3D);
            if (result.Count <= 0)
            {
                return false;
            }
            return true;
        }
    }
}
