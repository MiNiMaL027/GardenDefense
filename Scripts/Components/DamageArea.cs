using Farm.Scripts.Components;
using Farm.Scripts.Enums;
using Godot;
using System.Collections.Generic;

public partial class DamageArea : Area3D
{
    private Pawn AreaOwner { get; set; }
    private CollisionShape3D CollisionShape { get; set; }

    private AttackType AttackType;
    private bool IsStatic;
    private bool IsProjectile;

    public List<HitBoxArea> EnteredHitBoxs = new List<HitBoxArea>();

    public void Init(Pawn pawn)
    {
        AreaOwner = pawn;
        CollisionShape = GetChild<CollisionShape3D>(0);

        AreaEntered += DamageArea_AreaEntered;
    }

    private void DamageArea_AreaEntered(Area3D area)
    {
        if(area is HitBoxArea hitboxArea)
        {
            EnteredHitBoxs.Add(hitboxArea);

            if (IsStatic || IsProjectile)
                Attack();          
        }
    }

    private void Attack()
    { 
        if (EnteredHitBoxs.Count != 0)
        {
            EnteredHitBoxs[0].TakeDamage(AreaOwner.Damage);

            EnteredHitBoxs.Clear();
        }

        if (IsProjectile)
            Owner.QueueFree();
    }

    public void ActivateDamageArea()
    {
        CollisionShape.Disabled = false;
    }

    public void DisableDamageArea()
    {
        CollisionShape.Disabled = true;
    }
}
