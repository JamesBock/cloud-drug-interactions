using System;
using System.Collections.Generic;
using System.Text;

namespace UWPLockStep.Domain.Common.Units
{   /// <summary>
/// Replaced by the generic UnitValue<T></T>
/// </summary>
    public class AmountValue : IComparable<AmountValue>
    {

        public AmountEnumeration Amount { get; private set; }
        public decimal Quantity { get; private set; }


               
        public int CompareTo(AmountValue other)
        {
            throw new NotImplementedException();
        }
    }
}
