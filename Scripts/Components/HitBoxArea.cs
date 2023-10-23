using Godot;
using Pawns;

namespace Components
{
    public partial class HitBoxArea : Area3D
    {
        public Pawn AreaOwner { get; set; }
        public bool Block { get; set; }

        public void Init(Pawn pawn)
        {
            AreaOwner = pawn;
        }
    }
}
