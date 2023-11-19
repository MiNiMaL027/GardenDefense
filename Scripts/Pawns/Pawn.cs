using Components;
using Components.PawnStats;
using Controllers;
using Enums;
using Godot;

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
        public Stats PawnStats;
        public StatsComponent StatsComponent { get; set; }
        public AnimationPlayer Animation { get; set; }
        public HitBoxArea HitBox { get; set; }
        public ProgressBar3D HealthBar3D { get; set; }
        protected Node3D Mesh;

        public override void _Ready()
        {
            AddToGroup(Groups.Pawn);
            StatsComponent = GetNode<StatsComponent>("StatsComponent");
            StatsComponent.HealthBelowZero += healthBelowZeroListener;
            Animation = GetNode<AnimationPlayer>("AnimationPlayer");
            HealthBar3D = GetNode<ProgressBar3D>("HealthBar3D");

            InitializeStatsComponent();
            StatsComponent.HealthUpdated += StatsComponent_HealthUpdated;
            StatsComponent_HealthUpdated(StatsComponent.GetCurrentHealth(), StatsComponent.GetMaxHealth());
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
