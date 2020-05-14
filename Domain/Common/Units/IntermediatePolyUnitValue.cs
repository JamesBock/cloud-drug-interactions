using System;
using System.Collections.Generic;
using System.Text;

namespace UWPLockStep.Domain.Common.Units
{
    public class IntermediatePolyUnitValue
    {
        //TODO: Added setter, check if this is OK and if IntermediateUNit needs overhaul
        public IntermediatePolyUnit Unit { get; }
        public decimal Amount { get; set; }

        public IntermediatePolyUnitValue(IntermediatePolyUnit unit, decimal amount)
        {
            Amount = amount;
            Unit = unit;
        }

    }
}
