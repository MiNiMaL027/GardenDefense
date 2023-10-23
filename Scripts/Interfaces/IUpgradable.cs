namespace Interfaces
{
    public interface IUpgradable
    {
        public int CountOfAvalibalUpgrades { get; set; }

        public int CostToUpgrade { get; set; }

        public void Upgrade();
    }
}
