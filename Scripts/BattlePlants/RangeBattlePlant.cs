namespace Farm.Scripts.BattlePlants
{
    public abstract partial class RangeBattlePlant : BaseBattlePlant
    {
        public string MainProjectilePath { get; set; }
        public int ProjectileCount { get; set; }

        public override void _Ready()
        {
            base._Ready();          
        }
    }
}
