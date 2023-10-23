using Widgets;
using System.Collections.Generic;

namespace Comparers
{
    public class PriceComparers : IComparer<BaseSlot>
    {
        public int Compare(BaseSlot x, BaseSlot y)
        {
            if(x.ItemDatabaseRow.SellPrice >  y.ItemDatabaseRow.SellPrice)
            {
                return -1;
            }
            if(x.ItemDatabaseRow.SellPrice < y.ItemDatabaseRow.SellPrice)
            {
                return 1;
            }

            return 0;
        }
    }
}
