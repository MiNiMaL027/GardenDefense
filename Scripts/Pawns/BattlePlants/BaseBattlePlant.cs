using Components;
using Controllers;
using Enums;
using Godot;
using System.Reflection.Emit;
using Widgets.ToolTip;
using static Scenes.Widgets;

namespace Pawns.BattlePlants
{
    public partial class BaseBattlePlant : Pawn
    { 
        public LvlComponent LvlComponent { get; set; }

        [Export]
        public PawnType PlantType { get; set; }
        public ProgressBar3D ActivationBar3D { get; set; }
        public BattlePlantAIController BattlePlantAIController { get; set; }
        public BattlePlantTooltip Tooltip { get; set; }
        public static StyleBoxFlat activationBarStyle { get; set; }

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
            SetProcess(false);
            ActivationBar3D.QueueFree();
        }
        public override void _Ready()
        {
            base._Ready();

            LvlComponent = GetNode<LvlComponent>("LvlComponent");
            LvlComponent.LvlUpMethod = LvlUp;

            MouseEntered += ShowTooltip;
            MouseExited += HideTooltip;
        }

        public virtual void LvlUp()
        {
            var label = new Label3D() { Text = "Lvl up", Billboard = BaseMaterial3D.BillboardModeEnum.Enabled, FontSize = 50 };
            AddChild(label);
            label.Position = Position + new Vector3(0, 2, 0);
            label.Modulate = new Color(label.Modulate.R, label.Modulate.G, label.Modulate.B, 1.0f);
            var tween = CreateTween();
            tween.TweenProperty(label, "position", label.Position +  new Vector3(0,1,0), 1.0f);
            tween.Finished += label.QueueFree;

            StatsComponent.SetModifierStrenght((StatsComponent.GetBaseStrength() / 2) + StatsComponent.GetModifierStrength());
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
    }
}
