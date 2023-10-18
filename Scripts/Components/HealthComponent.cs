
namespace Farm.Scripts.Components
{
    public class HealthComponent
    {
        private Pawn Owner { get; set; }

        private int _maxHp;
        public int MaxHp
        {
            get => _maxHp;

            set
            {
                _maxHp = value;
                CurrentHp = _maxHp;
            }
        }

        public int CurrentHp { get; set; }

        public bool IsDead { get; set; } = false;
        public bool IsHurt { get; set; } = false;

        public void Init(Pawn owner)
        {
            Owner = owner;
            MaxHp = Owner.MaxHp;
        }

        public void TakeDamage(int damage)
        {                  
             CurrentHp -= damage;                   

            if (CurrentHp <= 0)
            {
                Owner.Animations.Play("Dead");
                IsDead = true;
            }
            else
            {
                Owner.Animations.Play("Hurt");
                IsHurt = true;
            }               
        }

        public void HealHp(int hp)
        {
            CurrentHp += hp;

            if (CurrentHp > MaxHp)
                CurrentHp = MaxHp;
        }

        public void AddHp(int hp)
        {
            MaxHp += hp;
        }

        public void SubtractionHp(int hp)
        {
            _maxHp -= hp;

            if (_maxHp <= 0)
                _maxHp = 1;     
        }
    }
}
