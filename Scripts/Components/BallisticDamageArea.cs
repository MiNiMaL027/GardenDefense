using Components;
using Godot;
using Projectiles;

namespace Components
{
    public partial class BallisticDamageArea:DamageArea
    {
        public override void areaEnteredListener(Area3D a)
        {
            if (a is HitBoxArea hitBox)
            {
                if (hitBox.AreaOwner != this.AreaOwner && pawnsDamageDealt.Contains(hitBox.AreaOwner) == false && hitBox.AreaOwner.GetType().IsSubclassOf(AreaOwner.Controller.EnemyType) && hitBox.AreaOwner.IsDead == false)
                {
                    hitBox.AreaOwner.LastTouchedPawn = AreaOwner;
                    AreaOwner.DealDamageOrHeal(hitBox.AreaOwner, GetDamageParameters());
                    GetParent<BallisticProjectile>().QueueFree();
                }
            }
        }
    }
}
