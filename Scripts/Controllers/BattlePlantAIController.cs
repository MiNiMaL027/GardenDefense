using Godot;
using Pawns.BattlePlants;

namespace Controllers
{
    public abstract partial class BattlePlantAIController : AIController
    {
        public int ActivationDelay { get; set; }
        public SceneTreeTimer timerActivation;
        public async override void _Ready()
        {
            timerActivation = GetTree().CreateTimer(ActivationDelay);
            (Pawn as BaseBattlePlant).OnActivation();
            await ToSignal(timerActivation, SceneTreeTimer.SignalName.Timeout);
            (Pawn as BaseBattlePlant).Activated();
            Activated();         
            base._Ready();
        }
      
        public virtual void Activated()
        {
        }
    }
}
