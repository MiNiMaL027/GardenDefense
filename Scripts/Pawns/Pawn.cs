using Components;
using Components.PawnStats;
using Controllers;
using Enums;
using Godot;
using Pawns.BattlePlants;
using System;
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
        [Export]
        public Texture2D Icon;
        public AIController Controller { get; set; }
        public Stats PawnStats;
        public StatsComponent StatsComponent { get; set; }
        public AnimationPlayerBasicCallbacks Animation { get; set; }
        public AnimationTree AnimationTree { get; set; }
        public AnimationNodeStateMachinePlayback AnimationNodeStateMachinePlayback { get; set; }
        public List<HitBoxArea> HitBoxes { get; set; } = new List<HitBoxArea>();
        public ProgressBar3D HealthBar3D { get; set; }
        public ProgressBar3D ArmorBar3D { get; set; }
        protected Node3D Mesh;
        protected MeshInstance3D mesh;
        bool isArmored;

        public Pawn LastTouchedPawn;

        Timer HealthRegenTimer;
        Timer ArmorRegenTimer;

        Tween ArmorRegenTween;
       

        public override void _Ready()
        {
            mesh = GetChild<Node3D>(0).FindNthChild<MeshInstance3D>();
            AddToGroup(Groups.Pawn);
            StatsComponent = GetNode<StatsComponent>("StatsComponent");
            StatsComponent.HealthBelowZero += healthBelowZeroListener;
            StatsComponent.ArmorUpdated += StatsComponent_ArmorUpdated;
            InitializeStatsComponent();
            StatsComponent.HealthUpdated += StatsComponent_HealthUpdated;
            StatsComponent_HealthUpdated(StatsComponent.GetCurrentHealth(), StatsComponent.GetMaxHealth());
 
            HealthRegenTimer = new Timer();
            AddChild(HealthRegenTimer);
            HealthRegenTimer.WaitTime = 5 / StatsComponent.GetHealthRegenRate();
            StatsComponent.HealthRegenRateUpdate += StatsComponent_HealthRegenRateUpdate;
            HealthRegenTimer.Start();
            HealthRegenTimer.Timeout += HealthRegenTimer_Timeout;

            ArmorRegenTimer = new Timer();
            AddChild(ArmorRegenTimer);
            ArmorRegenTimer.WaitTime = StatsComponent.GetArmorRegenDelay();
            ArmorRegenTimer.Timeout += ArmorRegenTimer_Timeout;
            ArmorRegenTimer.OneShot = true;
            StatsComponent.ArmorRegenDelayUpdated += StatsComponent_ArmorRegenDelayUpdated;
        }

        private void StatsComponent_ArmorRegenDelayUpdated(int newArmorRegenDelay)
        {
            ArmorRegenTimer.WaitTime = newArmorRegenDelay;
        }

        private void ArmorRegenTimer_Timeout()
        {
            if (ArmorRegenTween == null)
            {
                ArmorRegenTween = CreateTween();
            }

            int currentArmor = StatsComponent.GetCurrentArmor();
            int maxArmor = StatsComponent.GetMaxArmor();
            float regenRate = StatsComponent.GetArmorRegenRate();

            ArmorRegenTween.TweenMethod(
                Callable.From((float target) => StatsComponent.SetCurrentArmor((int)target)), 
                currentArmor,
                maxArmor,
                regenRate);
        }

        private void StatsComponent_ArmorUpdated(int currentArmor, int maxArmor)
        {
            if (ArmorBar3D == null)
            {
                ArmorBar3D = Scenes.Components.ProgressBar3D();
                ArmorBar3D.Position = HealthBar3D.Position;
                ArmorBar3D.SortingOffset = 1;
                
                AddChild(ArmorBar3D);
                ArmorBar3D.InitTexure(null, ResourceLoader.Load<Texture2D>("res://raw assets/Images/Info/Armor.png"));                       
            }

            ArmorBar3D.UpdateProgressBar(currentArmor, maxArmor);

            if(currentArmor != maxArmor)
                HealthBar3D.Show();
            else
                HealthBar3D.Hide();

            if (maxArmor >= 1 && currentArmor > 0)
            {
                if (!isArmored)
                {
                    AddArmorMaterial();
                    isArmored = true;
                }
            }
            else
            {
                if (isArmored)
                {
                    RemoveArmorMaterial();
                    isArmored = false;
                }
            }
        }

        private void AddArmorMaterial()
        {
            for (int i = 0; i < mesh.GetSurfaceOverrideMaterialCount(); i++)
            {
                var mat = mesh.Mesh.SurfaceGetMaterial(i).Duplicate() as StandardMaterial3D;
                mat.NextPass = ResourceLoader.Load<ShaderMaterial>("res://Meterials/Pawn/PawnArmor.tres");
                mesh.Mesh.SurfaceSetMaterial(i, mat);
            }
        }

        private void RemoveArmorMaterial()
        {
            for (int i = 0; i < mesh.GetSurfaceOverrideMaterialCount(); i++)
            {
                mesh.Mesh.SurfaceGetMaterial(i).NextPass = null;
            }
        }

        private void HealthRegenTimer_Timeout()
        {
            if (StatsComponent.GetHealthRegen() <= 0)
                return;

            StatsComponent.AddCurrentHealth(StatsComponent.GetHealthRegen());

            if(StatsComponent.GetCurrentHealth() < StatsComponent.GetMaxHealth() && StatsComponent.GetHealthRegen() > 0)
                ShowCountOfHpChange(StatsComponent.GetHealthRegen(), false);
        }

        private void StatsComponent_HealthRegenRateUpdate(int newHealthRegenRate)
        {
            HealthRegenTimer.WaitTime = 5 / newHealthRegenRate;
           
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
            StatsComponent.SetAttackRange(PawnStats.AttackRange);

            StatsComponent.SetHealthRegenRate(1);

            StatsComponent.SetBaseMaxArmor(100);
            StatsComponent.SetCurrentArmor(100);

            StatsComponent.SetArmorRegenDelay(1);
            StatsComponent.SetArmorRegenRate(2);
        }
        public virtual void InitializeStats()
        {
            PawnStats = new Stats()
            {
                MaxHealth = 100,
                Strength = 10,
                AttackSpeed = 1,
                AttackRange = 2
            };
        }
        protected virtual void healthBelowZeroListener()
        {
            IsDead = true;

            if (LastTouchedPawn != null && LastTouchedPawn is BaseBattlePlant battlePlants)
            {
                battlePlants.LvlComponent.AddPoints();
            }

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
                target.ApplyHeal(this, damageParameters);
            }

        }
        /// <summary>
        /// This function is virtual in order to affect movement component of monsters in derived classes
        /// </summary>
        /// <param name="countDamage"></param>
        /// <param name="attackModify"></param>
        public virtual void ApplyDamage(DamageParameters damageParameters)
        {
            if (IsDead) return;

            int currentArmor = StatsComponent.GetCurrentArmor();
            int damageToApply = damageParameters.CountDamage;

            if (StatsComponent.GetMaxArmor() > 0)
            {
                ArmorRegenTimer.Start(0);
                if(ArmorRegenTween != null)
                {
                    ArmorRegenTween.Kill();
                    ArmorRegenTween = null;
                }
                    
            }

            if (damageToApply > 0)
            {
                if (currentArmor > 0)
                {
                    int armorDamage = Math.Min(currentArmor, damageToApply);
                    StatsComponent.SetCurrentArmor(currentArmor - armorDamage);
                    damageToApply -= armorDamage;
                }

                if (damageToApply > 0)
                {
                    if (AnimationNodeStateMachinePlayback != null && damageParameters.AttackModify == AttackModify.Interrupt)
                    {
                        AnimationTree.Set("parameters/Idle/OneShotHurt/request", (int)AnimationNodeOneShot.OneShotRequest.Fire);
                        AnimationTree.Set("parameters/Moving/OneShotHurt/request", (int)AnimationNodeOneShot.OneShotRequest.Fire);
                    }
                    else
                    {
                        // Animation.Play(AnimationNames.Hurt);
                    }

                    StatsComponent.SetCurrentHealth(StatsComponent.GetCurrentHealth() - damageToApply);
                    ShowCountOfHpChange(damageToApply);
                }
            }

           
        }
        public virtual void ApplyHeal(Pawn dealer, DamageParameters damageParameters)
        {
            if (IsDead == true) { return; }

            ShowCountOfHpChange(damageParameters.CountDamage, false);
            StatsComponent.SetCurrentHealth(StatsComponent.GetCurrentHealth() + damageParameters.CountDamage);
        }
        protected void ShowCountOfHpChange(int count, bool isDamage = true)
        {                
            var label = new Label3D
            {
                Text = count.ToString(),
                Billboard = BaseMaterial3D.BillboardModeEnum.Enabled,
                Modulate = isDamage ? new Color(0.776f, 0.212f, 0.176f) : new Color(0.259f, 0.671f, 0.129f),
                FontSize = 100
            };

            this.FindParentOfType<World>().AddChild(label);
            label.Position = GlobalPosition;

            var tween = label.CreateTween();

            var startPosition = label.Position;
            var endPosition = startPosition + new Vector3(0, 2, 0); 

            tween.TweenProperty(label, "position", endPosition, 1.0f); 

            tween.Finished += () => label.QueueFree();

            tween.Play();
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
