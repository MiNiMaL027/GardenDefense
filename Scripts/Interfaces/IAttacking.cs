using Farm.Scripts.Enums;

namespace Farm.Scripts.Interfaces
{
    public interface IAttacking
    {
        public int Damage { get; set; }
        public AttackType AttackType { get; set; }
    }
}
