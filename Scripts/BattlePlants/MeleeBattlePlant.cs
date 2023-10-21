
namespace Farm.Scripts.BattlePlants
{
    public abstract partial class MeleeBattlePlant : BaseBattlePlant
    {
        public DamageArea DamageArea { get; set; }

        public override void _Ready()
        {
            base._Ready();

            DamageArea = GetNode<DamageArea>("DamageArea");
            DamageArea.Init(this);
        }
    }
}
