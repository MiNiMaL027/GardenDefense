using Farm.Scripts.Controllers;
using Godot;

namespace Farm.Scripts.Components
{
    public partial class RangeArea : Area3D
    {
        public Pawn AreaOwner { get; set; }
        public int AimCount { get; set; } 
        public float AttackRange { get; set; }

        public void Init(Pawn owner)
        {
            AreaOwner = owner;
            AttackRange = (GetChild<CollisionShape3D>(0).Shape as BoxShape3D).Size.Z;

            AreaEntered += RangeArea_AreaEntered;
            AreaExited += RangeArea_AreaExited;
        }

        private void RangeArea_AreaExited(Area3D area)
        {
            if(area is HitBoxArea)
            {
                AimCount--;

                if(AimCount <= 0)
                {
                    //AreaOwner.IsAttacking = false;
                }
            }
        }

        private void RangeArea_AreaEntered(Area3D area)
        {
            if(area is HitBoxArea)
            {
                AimCount++;
                //AreaOwner.IsAttacking = true;
            }
        }
    }
}
