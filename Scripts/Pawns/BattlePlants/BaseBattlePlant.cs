using Enums;
using Godot;

namespace Pawns.BattlePlants
{
    public partial class BaseBattlePlant : Pawn
    { 
        public int Lvl { get; set; } = 1;
        [Export]
        public PawnType PlantType { get; set; }

        public override void _Ready()
        {
            base._Ready();
        }
    }
}
