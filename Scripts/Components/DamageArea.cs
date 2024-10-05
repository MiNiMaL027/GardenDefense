using Controllers;
using Enums;
using Godot;
using Interfaces;
using Pawns;
using System.Collections.Generic;
namespace Components
{
    public partial class DamageArea : Area3D
    {
        [Export]
        public DamageAreaType DamageAreaType;
        [Export]
        public AttackModify AttackModify;
        [Export]
        public int Damage;
        [Export]
        public float KnockbackDistance = 0f;

        public DamageParameters GetDamageParameters()
        {
            return new DamageParameters()
            {
                CountDamage = Damage,
                DamageAreaType = this.DamageAreaType,
                AttackModify = this.AttackModify,
                KnockbackDistance = this.KnockbackDistance
            };
        }
        public Pawn AreaOwner { get; set; }
        protected List<Pawn> pawnsDamageDealt = new List<Pawn>(); //this list contains list of pawns damage dealt in one attack
        public virtual void Enable()
        {
            pawnsDamageDealt.Clear();
            Connect("area_entered", new Callable(this, nameof(areaEnteredListener)));
            Godot.Collections.Array<Area3D> overlappedAreas = this.GetOverlappingAreas();
            for(int i =0;i<overlappedAreas.Count;i++)
            {
                areaEnteredListener(overlappedAreas[i]);
            }
        }
        public virtual void Disable()
        {
            Disconnect("area_entered", new Callable(this, nameof(areaEnteredListener)));
        }
        public virtual void areaEnteredListener(Area3D a)
        {
            if (a is HitBoxArea hitBox)
            {
                if (hitBox.AreaOwner != this.AreaOwner && pawnsDamageDealt.Contains(hitBox.AreaOwner) == false && hitBox.AreaOwner.GetType().IsSubclassOf(AreaOwner.Controller.EnemyType) && hitBox.AreaOwner.IsDead == false)
                {
                    hitBox.AreaOwner.LastTouchedPawn = AreaOwner;
                    AreaOwner.DealDamageOrHeal(hitBox.AreaOwner, GetDamageParameters());
                    pawnsDamageDealt.Add(hitBox.AreaOwner);
                }
            }
        }
    }
}

