using System.Collections.Generic;

namespace Farm.Scripts.Comparers
{
    public class AmountComparers : IComparer<sell_slot>
    {
        public int Compare(sell_slot x, sell_slot y)
        {
            if(x.Amount > y.Amount)
            {
                return -1;
            }
            if(x.Amount < y.Amount)
            {
                return 1;
            }

            return 0;
        }
    }
}
