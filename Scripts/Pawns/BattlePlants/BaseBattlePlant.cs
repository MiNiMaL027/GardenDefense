using Components;
using Enums;
using Godot;

namespace Pawns.BattlePlants
{
    public abstract partial class BaseBattlePlant : Pawn
    { 
        public int Lvl { get; set; } = 1;

        public override void _Ready()
        {
            base._Ready();
        }
    }
}
