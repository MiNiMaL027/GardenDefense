using Enums;

namespace Interfaces
{
    public interface IAttacking
    {
        public int Damage { get; set; }
        public AttackModify AttackModify { get; set; }
    }
}
