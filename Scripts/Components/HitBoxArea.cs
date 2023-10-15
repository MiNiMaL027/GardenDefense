using Godot;

namespace Farm.Scripts.Components
{
    public partial class HitBoxArea : Area3D
    {
        public Pawn AreaOwner { get; set; }
        public bool Block { get; set; }

        public void Init(Pawn pawn)
        {
            AreaOwner = pawn;
        }

        public void TakeDamage(int damage)
        {
            if (Block)
                return;

            AreaOwner.HealthComponent.TakeDamage(damage);
        }

        public void Heal(int healValue)
        {
            AreaOwner.HealthComponent.HealHp(healValue);
        }
    }
}
