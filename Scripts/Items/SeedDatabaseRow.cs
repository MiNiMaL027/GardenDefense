using Enums;

namespace Items
{
    public class SeedDatabaseRow:ItemDatabaseRow
    {
        public SeedType SeedType;
        public int StagesAmount;
        public int MinSecondsToChangeState;
        public int MaxSecondsToChangeState;
        public int GrowUpId;

    }
}
