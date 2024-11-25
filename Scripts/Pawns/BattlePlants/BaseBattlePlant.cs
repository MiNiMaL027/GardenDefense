using BaseClasses;
using Components;
using Controllers;
using Enums;
using Godot;
using Interfaces;
using Widgets.ContextMenu;
using Widgets.ToolTip;

namespace Pawns.BattlePlants
{
    public partial class BaseBattlePlant : BaseOutlinePawn , IPressable
    { 
        public LvlComponent LvlComponent { get; set; }

        [Export]
        public PawnType PlantType { get; set; }
        public ProgressBar3D ActivationBar3D { get; set; }
        public BattlePlantAIController BattlePlantAIController { get; set; }
        public BattlePlantTooltip Tooltip { get; set; }
        public static StyleBoxFlat activationBarStyle { get; set; }        
        public SkillComponent SkillComponent { get; set; }
        public int SkillRequiredMutagen = 1;
        
        static BaseBattlePlant()
        {
            activationBarStyle = new StyleBoxFlat();
            activationBarStyle.BgColor = Color.Color8(255, 216, 0);
        }

        public virtual void OnActivation()
        {
            ActivationBar3D = Scenes.Components.ProgressBar3D();
            BattlePlantAIController = Controller as BattlePlantAIController;
            ActivationBar3D.Position=HealthBar3D.Position + new Vector3(0,0.5f,0);
            AddChild(ActivationBar3D);
            ActivationBar3D.ProgressBar.MaxValue = BattlePlantAIController.ActivationDelay;
            ActivationBar3D.ProgressBar.Set("theme_override_styles/fill", activationBarStyle);
            SetProcess(true);
        }
        public override void _Process(double delta)
        {
            base._Process(delta);
            ActivationBar3D.ProgressBar.Value = BattlePlantAIController.ActivationDelay - BattlePlantAIController.timerActivation.TimeLeft;
        }
        public virtual void Activated()
        {
            if (!IsInstanceValid(this))
                return;

            SetProcess(false);
            ActivationBar3D.QueueFree();
        }
        public override void _Ready()
        {
            base._Ready();

            LvlComponent = GetNode<LvlComponent>("LvlComponent");
            LvlComponent.LvlUpMethod = LvlUp;
            SkillComponent = GetNode<SkillComponent>("SkillComponent");
            SkillComponent.Init(this);

            StatsComponent.CustomStatUpdated += StatsComponent_CustomStatUpdated;
        }

        protected virtual void StatsComponent_CustomStatUpdated(string statName, int statValue) { } //TODO Add to some custom property to batlle plant classes and change it here


        public virtual void LvlUp()
        {
            var label = new Label3D() { Text = "Lvl up", Billboard = BaseMaterial3D.BillboardModeEnum.Enabled, FontSize = 50 };
            AddChild(label);
            label.Position = Position + new Vector3(0, 2, 0);
            label.Modulate = new Color(label.Modulate.R, label.Modulate.G, label.Modulate.B, 1.0f);
            var tween = CreateTween();
            tween.TweenProperty(label, "position", label.Position +  new Vector3(0,1,0), 1.0f);
            tween.Finished += label.QueueFree;

            StatsComponent.SetModifierStrength((StatsComponent.GetBaseStrength() / 2) + StatsComponent.GetModifierStrength());
        }

        public override void ApplyHeal(Pawn dealer, DamageParameters damageParameters)
        {
            if (IsDead == true) { return; }
            var currentHp = StatsComponent.GetCurrentHealth();
            StatsComponent.SetCurrentHealth(currentHp + damageParameters.CountDamage);
            if(dealer is BaseBattlePlant plant)
            {
                plant.LvlComponent.AddPoints(StatsComponent.GetCurrentHealth() - currentHp);
            }

            ShowCountOfHpChange(StatsComponent.GetCurrentHealth() - currentHp, false);
            
        }

        private void ShowTooltip()
        {
            Tooltip = Scenes.Widgets.ToolTip.BattlePlantTooltip();

            PlayerController playerController = this.GetPlayerController();
            Tooltip.timeToView = 1;
            playerController.Hud.AddChild(Tooltip);        
            Tooltip.Init(this);           
            playerController.Hud.AddAtMousePosition(Tooltip);
        }

        private void HideTooltip()
        {
            if (Tooltip != null)
            {
                Tooltip.HideTooltip();

                Tooltip = null;
            }
        }

        public override void MouseEnter()
        {
            ShowTooltip();
            base.MouseEnter();
        }

        public override void MouseLeave()
        {
            HideTooltip();
            base.MouseLeave();
        }

        public void LeftMouseDownListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
        {
            
        }

        public void RightMouseDownListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
        {
            BattlePlantContextMenu plantContentMenu = Scenes.Widgets.ContextMenu.BattlePlantContextMenu();

            playerController.OpenedContextMenu = plantContentMenu;
            playerController.Hud.AddChild(plantContentMenu);
            plantContentMenu.Init(this, playerController);
            playerController.Hud.AddAtMousePosition(plantContentMenu);
        }

        public void LeftMouseUpListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
        {
            
        }
    }
}
