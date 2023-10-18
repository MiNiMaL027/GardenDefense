using Farm.Scripts.Components;
using Farm.Scripts.Enums;
using Farm.Scripts.Interfaces;
using Godot;
using System.Collections.Generic;

public partial class DamageArea : Area3D
{
    private IAttacking AreaOwner { get; set; }
    private CollisionShape3D CollisionShape { get; set; }

    public bool IsStatic;
    public bool IsProjectile;

    private List<HitBoxArea> EnteredHitBoxs = new List<HitBoxArea>();

    public void Init(IAttacking pawn)
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

    public void Attack()
    { 
        if (EnteredHitBoxs.Count != 0)
        {           
            if (AreaOwner.AttackModify != AttackModify.Simple)
            {
                SpecialAction();
                return;
            }

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

    private void SpecialAction()
    {
        switch (AreaOwner.AttackModify)
        {
            case AttackModify.Knockback:
                EnteredHitBoxs[0].GlobalPosition += new Vector3(1f, 0, 0);
                EnteredHitBoxs[0].TakeDamage(AreaOwner.Damage);
                break;
            case AttackModify.Heal:
                EnteredHitBoxs[0].Heal(AreaOwner.Damage);
                break;              
        }

        EnteredHitBoxs.Clear();
    }
}
