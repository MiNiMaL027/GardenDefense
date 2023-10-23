using Godot;

namespace Pawns.BattlePlants
{
    public abstract partial class RangeBattlePlant : BaseBattlePlant
    {
        public int ProjectileCount { get; set; }
        public Node3D ProjectileSpawnPosition { get; set; }

        public override void _Ready()
        {
            base._Ready();
            ProjectileSpawnPosition = GetNode<Node3D>("ProjectileSpawnPosition");
        }
    }
}
