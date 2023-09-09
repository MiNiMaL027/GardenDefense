using Farm.Scripts.Widgets;
using System.Collections.Generic;

namespace Farm.Scripts.Comparers
{
    public class AmountComparers : IComparer<BaseSlot>
    {
        public int Compare(BaseSlot x, BaseSlot y)
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
