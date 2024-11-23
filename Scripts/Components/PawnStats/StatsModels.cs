using System;

namespace Components.PawnStats
{
    /// <summary>
    /// This class is used for initializing StatsComponent and AIController
    /// </summary>
    public class Stats
    {
        public int MaxHealth;
        public int Strength;
        public int AttackSpeed;
        public int AttackRange;
    }
    public class MonsterStats : Stats
    {
        public int MovementSpeed;
    }
}
