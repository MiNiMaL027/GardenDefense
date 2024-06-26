using System;

namespace Enums
{
    [Flags]
    public enum SlotBlocker
    {
        None = 0,
        Cooldown = 1 << 1,
        EnergyOut = 1 << 2
    }
}
