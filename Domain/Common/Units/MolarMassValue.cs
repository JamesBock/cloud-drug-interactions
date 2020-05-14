using System;
using System.Collections.Generic;
using System.Text;

namespace UWPLockStep.Domain.Common.Units
{
    /// <summary>
    /// Unclear i
    /// </summary>
    public class MolarMassValue
    {

        public BaseUnitValue<Mass> MassValue { get; private set; }

        public BaseUnitValue<AmountOfElement> AmountValue { get; private set; }

        private decimal DimensionlessRatio { get; set; }

        public MolarMassValue(BaseUnitValue<Mass> massValue, BaseUnitValue<AmountOfElement> amountValue)
        {
            DimensionlessRatio = massValue.Amount / amountValue.Amount;
        }
    }
}
