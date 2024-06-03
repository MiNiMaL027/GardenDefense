using System;

namespace Enums
{
    [Flags]
    public enum PawnType
    {
        Ground = 1 << 1,
        Flying = 1 << 2,
    }
}
