using Godot;
using System;

namespace Components
{
    public partial class MovementComponent : Node
    {
        [Signal]
        public delegate void MovementInfoEventHandler(Vector3 velocity, bool grounded);
        public CharacterBody3D bodyToMove = null;
        [Export]
        public float MoveAccel
        {
            get
            {
                return moveAccel;
            }
            set
            {
                moveAccel = value;
                if (maxSpeed != 0)
                    drag = MoveAccel / maxSpeed;
            }
        }
        private float moveAccel = 4f;
        [Export]
        public int MaxSpeed
        {
            get
            {
                return maxSpeed;
            }
            set
            {
                maxSpeed = value;
                if (maxSpeed != 0)
                    drag = MoveAccel / maxSpeed;

            }
        }
        private int maxSpeed = 10;
        public float drag = 0.0f;
        [Export]
        public float JumpForce
        {
            get
            {
                return jumpForce;
            }
            set
            {
                jumpForce = value;
            }
        }
        private float jumpForce = 10f;

        [Export]
        public float Gravity
        {
            get
            {
                return gravity;
            }
            set
            {
                gravity = value;
            }
        }
        private float gravity = 30f;

        public bool pressedJump = false;
        public Vector3 moveVec = new Vector3();
        public Vector3 velocity = new Vector3();
        public Vector3 snapVector = new Vector3();

        [Export]
        public bool ignoreRotation = true;

        bool frozen = false;

        public override void _Ready()
        {
            drag = MoveAccel / MaxSpeed;

        }
        public void Init(CharacterBody3D bodyToSet)
        {
            bodyToMove = bodyToSet;
        }
        public void Jump()
        {
            pressedJump = true;
        }
        public void SetMoveVec(Vector3 vecToSet)
        {
            moveVec = vecToSet.Normalized();
        }
        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);
            if (frozen) { return; }
            Vector3 currentMoveVector = moveVec;
            if (ignoreRotation == false)
            {
                currentMoveVector = currentMoveVector.Rotated(Vector3.Up, bodyToMove.Rotation.Y);
            }
            velocity += MoveAccel * currentMoveVector - velocity * new Vector3(drag, 0, drag) + Gravity * Vector3.Down * (float)delta;
            bodyToMove.Velocity = velocity;
            bodyToMove.MoveAndSlide();
            bool grounded = bodyToMove.IsOnFloor();
            if (grounded)
            {
                velocity = new Vector3(velocity.X, -0.01f, velocity.Z);
            }
            if (grounded && pressedJump)
            {
                velocity = new Vector3(velocity.X, JumpForce, velocity.Z);
                bodyToMove.FloorSnapLength = 0f;
            }
            else
            {
                bodyToMove.FloorSnapLength = 0.1f;
            }
            pressedJump = false;
            EmitSignal(SignalName.MovementInfo, velocity, grounded);
        }
        public void Freeze()
        {
            frozen = true;
        }
        public void Unfreeze()
        {
            frozen = false;
        }
    }
}
