using Components;
using Enums;
using Godot;
using Pawns;
using System;
namespace Projectiles
{
    public partial class SelfAimingProjectile : Projectile
    {
        [Export]
        public Node3D Target;
        public override void FullInit(ProjectileParameters p)
        {

            Target = p.Target;
            base.FullInit(p);
        }
        public override void _PhysicsProcess(double delta)
        {
            if(GodotObject.IsInstanceValid(Target))
            {
                this.LookAt(Target.GlobalPosition, null, true);
            }
            base._PhysicsProcess(delta);
        }
    }

}
