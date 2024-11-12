using Enums;
using Godot;
using Pawns;
using Projectiles;

namespace Components
{
    public class ProjectileParameters:DamageParameters
    {
        public Pawn Owner { get; set; } //reference to pawn that launched projectile
        public int MaxTargets { get; set; }
        public int InitialSpeed { get; set; }
        public double MaxDistanceOfProjectile { get; set; } = Projectile.DefaultMaxDistanceOfProjectile;
        /// <summary>
        /// Used for self aiming at node in SelfAimingProjectile
        /// </summary>
        public Node3D Target { get; set; }
    }

    public class BallisticProjectileParameters : DamageParameters
    {
        public Pawn Owner { get; set; } //reference to pawn that launched projectile
        public Pawn TargetPawn { get; set; } //reference to pawn that launched projectile

        public Vector3? Target { get; set; }
    }
}
