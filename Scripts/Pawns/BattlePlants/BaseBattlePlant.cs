using Components;
using Controllers;
using Enums;
using Godot;

namespace Pawns.BattlePlants
{
    public partial class BaseBattlePlant : Pawn
    { 
        public int Lvl { get; set; } = 1;
        [Export]
        public PawnType PlantType { get; set; }
        public ProgressBar3D ActivationBar3D { get; set; }
        public BattlePlantAIController BattlePlantAIController { get; set; }
        public static StyleBoxFlat activationBarStyle { get; set; }

        static BaseBattlePlant()
        {
            activationBarStyle=new StyleBoxFlat();
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
        }
    }
}
