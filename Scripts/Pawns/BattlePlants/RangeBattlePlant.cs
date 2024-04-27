using Godot;

namespace Pawns.BattlePlants
{
    public abstract partial class RangeBattlePlant : BaseBattlePlant
    {
        public Node3D ProjectileSpawnPosition { get; set; }

        public override void _Ready()
        {
            base._Ready();
        }
    }
}
