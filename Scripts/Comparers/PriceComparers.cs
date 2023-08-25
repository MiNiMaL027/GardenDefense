using System.Collections.Generic;

namespace Farm.Scripts.Comparers
{
    public class PriceComparers : IComparer<sell_slot>
    {
        public int Compare(sell_slot x, sell_slot y)
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
