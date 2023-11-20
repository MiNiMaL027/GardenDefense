using System.Collections.Generic;
using Widgets;

namespace Comparers
{
    public class AmountDecsComparers : IComparer<BaseSlot>
    {
        public int Compare(BaseSlot x, BaseSlot y)
        {
            if(x.Amount > y.Amount)
            {
                return 1;
            }
            if(x.Amount < y.Amount)
            {
                return -1;
            }

            return 0;
        }
    }
}
