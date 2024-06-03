using System;

namespace Enums
{
    [Flags]
    public enum PawnClass
    {
        Block = 1 << 1,
        Support = 1 << 2,
        Damage = 1 << 3,
        Range = 1 << 4,
        Melee = 1 << 5,
        Heal = 1 << 6,
    }
}
