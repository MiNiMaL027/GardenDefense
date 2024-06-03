using Components;
using Components.PawnStats;
using Controllers;
using Enums;
using Godot;
using System.Collections.Generic;

namespace Pawns
{
    public abstract partial class Pawn : CharacterBody3D
    {
        [Signal]
        public delegate void DiedEventHandler();
        public bool IsDead { get; set; } = false;
        [Export]
        public PawnClass Class { get; set; }
        [Export]
        public string PawnName = "Nameless";
        [Export]
        public int PawnId = 0;
        public AIController Controller { get; set; }
        public Stats PawnStats;
        public StatsComponent StatsComponent { get; set; }
        public AnimationPlayerBasicCallbacks Animation { get; set; }
        public AnimationTree AnimationTree { get; set; }
        public AnimationNodeStateMachinePlayback AnimationNodeStateMachinePlayback { get; set; }
        public List<HitBoxArea> HitBoxes { get; set; } = new List<HitBoxArea>();
        public ProgressBar3D HealthBar3D { get; set; }
        protected Node3D Mesh;

        public override void _Ready()
        {
            AddToGroup(Groups.Pawn);
            StatsComponent = GetNode<StatsComponent>("StatsComponent");
            StatsComponent.HealthBelowZero += healthBelowZeroListener;

            InitializeStatsComponent();
            StatsComponent.HealthUpdated += StatsComponent_HealthUpdated;
            StatsComponent_HealthUpdated(StatsComponent.GetCurrentHealth(), StatsComponent.GetMaxHealth());
        }
        protected bool isAttacking = false;
        public virtual bool IsAttacking
        {
            get
            {
                return isAttacking;
            }
            set
            {
                isAttacking = true;
            }
        }
        private void StatsComponent_HealthUpdated(int currentHealth, int maxHealth)
        {
            HealthBar3D.UpdateProgressBar(currentHealth, maxHealth);
        }

        public Pawn()
        {
            InitializeStats();
        }
        public virtual void InitializeStatsComponent()
        {
            StatsComponent.SetMaxHealth(PawnStats.MaxHealth);
            StatsComponent.SetCurrentHealth(PawnStats.MaxHealth);

            StatsComponent.SetStrength(PawnStats.Strength);
        }
        public virtual void InitializeStats()
        {
            PawnStats = new Stats()
            {
                MaxHealth = 100,
                Strength = 10,
                AttackSpeed = 1f,
                AttackRange = 2.5f
            };
        }
        protected virtual void healthBelowZeroListener()
        {
            IsDead = true;
            EmitSignal(SignalName.Died);
            if(AnimationNodeStateMachinePlayback != null)
            {
                AnimationNodeStateMachinePlayback.Travel(AnimationStates.Die);
            }
            else
            {
                Animation.Play(AnimationNames.Die);
            }
        }

        public virtual void DealDamageOrHeal(Pawn target, DamageParameters damageParameters)
        {
            if(damageParameters.DamageAreaType == DamageAreaType.Damage)
            {
                target.ApplyDamage(damageParameters);
            }
            else
            {
                target.ApplyHeal(damageParameters);
            }

        }
        /// <summary>
        /// This function is virtual in order to affect movement component of monsters in derived classes
        /// </summary>
        /// <param name="countDamage"></param>
        /// <param name="attackModify"></param>
        public virtual void ApplyDamage(DamageParameters damageParameters)
        {
            if(IsDead == true) { return; }
            if (damageParameters.CountDamage > 0)
            {
                if(AnimationNodeStateMachinePlayback != null && damageParameters.AttackModify == AttackModify.Interrupt)
                {
                    AnimationTree.Set("parameters/Idle/OneShotHurt/request", (int)AnimationNodeOneShot.OneShotRequest.Fire);
                    AnimationTree.Set("parameters/Moving/OneShotHurt/request", (int)AnimationNodeOneShot.OneShotRequest.Fire);
                }
                else
                {
                    //Animation.Play(AnimationNames.Hurt);
                }
            }
            StatsComponent.SetCurrentHealth(StatsComponent.GetCurrentHealth() - damageParameters.CountDamage);
        }
        public virtual void ApplyHeal(DamageParameters damageParameters)
        {
            if (IsDead == true) { return; }
            StatsComponent.SetCurrentHealth(StatsComponent.GetCurrentHealth() + damageParameters.CountDamage);
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
                    HitBoxes.Add(hitBox);
                }
                else
                {
                    ConnectHitBoxes(children[i]);
                }
            }
        }
    }
}
