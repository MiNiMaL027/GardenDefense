using Enums;

namespace Items
{
    internal class SeedDatabaseRow:ItemDatabaseRow
    {
        public SeedType SeedType;
        public int StagesAmount;
        public int MinSecondsToChangeState;
        public int MaxSecondsToChangeState;
        public int GrowUpId;

    }
}
