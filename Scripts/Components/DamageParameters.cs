using Enums;

namespace Components
{
    public class DamageParameters
    {
        public int CountDamage { get; set; } = 0;
        public float KnockbackDistance { get; set; } = 1f;
        public AttackModify AttackModify { get; set; } = AttackModify.Simple;
        public DamageAreaType DamageAreaType { get; set; } = DamageAreaType.Damage;
    }
}
