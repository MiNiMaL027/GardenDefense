using Controllers;
using Enums;
using Interfaces;
using Godot;
using Components;
using System;
using Widgets.Bestiary;
using Pawns.Monsters;
using Pawns.BattlePlants;

namespace Pawns
{
    public abstract partial class Pawn : CharacterBody3D
    {
        [Signal]
        public delegate void DiedEventHandler();
        public bool IsDead { get; set; } = false;
        [Export]
        public string PawnName = "Nameless";
        public AIController Controller { get; set; }
        public StatsComponent StatsComponent { get; set; }

        /// <summary>
        /// Called before reading values for bestiary window
        /// </summary>
        public virtual void BestiaryReady()
        {
            StatsComponent = GetNode<StatsComponent>("StatsComponent");
            InitializeStats();
        }
        public AnimationPlayer Animation { get; set; }
        [Export]
        public float AttackSpeed { get; set; } = 1f;
        public HitBoxArea HitBox { get; set; }
        protected Node3D Mesh;

        public override void _Ready()
        {
            AddToGroup(Groups.Pawn);
            StatsComponent = GetNode<StatsComponent>("StatsComponent");
            StatsComponent.HealthBelowZero += healthBelowZeroListener;
            Animation = GetNode<AnimationPlayer>("AnimationPlayer");

            InitializeStats();
        }
        public virtual void InitializeStats()
        {
            StatsComponent.SetMaxHealth(100);
            StatsComponent.SetCurrentHealth(100);

            StatsComponent.SetStrength(10);
        }
        protected virtual void healthBelowZeroListener()
        {
            IsDead = true;
            EmitSignal(SignalName.Died);
            Animation.Play(AnimationNames.Die);
        }

        public virtual void DealDamage(Pawn target, int countDamage, AttackModify attackModify)
        {
            if (target.IsDead == true) { return; }

            target.ApplyDamage(countDamage, attackModify);
            
        }
        /// <summary>
        /// This function is virtual in order to affect movement component of monsters in derived classes
        /// </summary>
        /// <param name="countDamage"></param>
        /// <param name="attackModify"></param>
        public virtual void ApplyDamage(int countDamage, AttackModify attackModify)
        {
            if (countDamage > 0)
            {
                Animation.Play(AnimationNames.Hurt);
            }
            StatsComponent.SetCurrentHealth(StatsComponent.GetCurrentHealth() - countDamage);
            GD.Print("Health = " + StatsComponent.GetCurrentHealth());
        }
        public virtual void Heal(Pawn target, int countHealth)
        {
            if (target.IsDead == true) { return; }
            target.StatsComponent.SetCurrentHealth(target.StatsComponent.GetCurrentHealth() + countHealth);
        }
        /// <summary>
        /// Iterate through all children, searches hit boxes and set owner
        /// </summary>
        /// <param name="n"></param>
        public virtual void ConnectHitBoxes(Node n)
        {
            Godot.Collections.Array<Node> children = n.GetChildren();
            for (int i = 0; i < children.Count; i++)
            {
                if (children[i] is HitBoxArea hitBox)
                {
                    hitBox.Init(this);
                }
                else
                {
                    ConnectHitBoxes(children[i]);
                }
            }
        }
    }
}
