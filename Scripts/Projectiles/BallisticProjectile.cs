using Components;
using Godot;
using Pawns;

namespace Projectiles
{
    /// <summary>
    /// Designed to deliver damage area with ballistic rules
    /// </summary>
    public partial class BallisticProjectile:RigidBody3D
    {
        public BallisticDamageArea DamageArea;
        public virtual void FullInit(BallisticProjectileParameters p)
        {
            DamageArea.AreaOwner = p.Owner;
            DamageArea.DamageAreaType = p.DamageAreaType;
            DamageArea.AttackModify = p.AttackModify;
            DamageArea.Damage = p.CountDamage;
            DamageArea.KnockbackDistance = p.KnockbackDistance;
            TargetPosition = p.Target;
            DamageArea.TargetPawn = p.TargetPawn;
            Enable();
        }
        public Vector3? TargetPosition;

        public override void _Ready()
        {
            DamageArea = GetNode<BallisticDamageArea>("DamageArea");
            Godot.Timer timer = new Godot.Timer();
            timer.OneShot = true;
            timer.WaitTime = 50;
            timer.Timeout += Timer_Timeout;
            AddChild(timer);
            timer.Start();
        }

        private void Timer_Timeout()
        {
            QueueFree();
        }

        public virtual void Enable()
        {
            // Обчислюємо початкову швидкість для досягнення TargetPosition
            Vector3 initialVelocity = CalculateLaunchVelocity(GlobalTransform.Origin, TargetPosition.Value, 45f);
            LinearVelocity = initialVelocity;
            DamageArea.Enable();
            SetPhysicsProcess(true);
            SetProcess(true);
        }
        public virtual void Disable()
        {
            DamageArea.Disable();
            SetPhysicsProcess(false);
            SetProcess(false);
        }


        private Vector3 CalculateLaunchVelocity(Vector3 start, Vector3 target, float angleDegrees)
        {
            Vector3 direction = target - start;
            float gravity = 9.8f; // Get the magnitude of gravity
            float angleRadians = Mathf.DegToRad(angleDegrees);

            // Extract horizontal distance
            float Dxz = Mathf.Sqrt(direction.X * direction.X + direction.Z * direction.Z);
            float Dy = direction.Y;

            float cosTheta = Mathf.Cos(angleRadians);
            float sinTheta = Mathf.Sin(angleRadians);
            float tanTheta = Mathf.Tan(angleRadians);

            float v0SquaredNumerator = gravity * Dxz * Dxz;
            float v0SquaredDenominator = 2 * cosTheta * cosTheta * (tanTheta * Dxz - Dy);

            if (v0SquaredDenominator <= 0)
            {
                return Vector3.Zero;
            }

            float v0Squared = v0SquaredNumerator / v0SquaredDenominator;

            if (v0Squared <= 0)
            {
                return Vector3.Zero;
            }

            float v0 = Mathf.Sqrt(v0Squared);

            // Compute the direction angles
            float phi = Mathf.Atan2(direction.Z, direction.X);

            // Compute initial velocity components
            float V0x = v0 * cosTheta * Mathf.Cos(phi);
            float V0y = v0 * sinTheta;
            float V0z = v0 * cosTheta * Mathf.Sin(phi);

            return new Vector3(V0x, V0y, V0z);
        }
    }
}
