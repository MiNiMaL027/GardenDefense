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
        public float AttackSpeed;
        public float AttackRange;
    }
    public class MonsterStats : Stats
    {
        public int MovementSpeed;
    }
}
