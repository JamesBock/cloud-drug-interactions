using System;
using System.Collections.Generic;
using System.Text;
using UWPLockStep.Domain.Interfaces;

namespace UWPLockStep.Domain.Common.Units
{/// <summary>
/// Creates a new unit from two other Units. Different from PolyUnit, which translates multiple unit values and projects as a single unit
/// </summary>
    public class DerivedRatioValue /*: IUnitValue*/
    {
        private decimal _amount;

        public DerivedRatioUnit Unit { get; } //This represents a Multi Dimensional Ratio Unit that projects as one Dimension or the other.
        public decimal Amount => _amount; //what dose this amount represent? The Amount of the Unit that is being projected by the DerivedRatioUnit


        public DerivedRatioValue(DerivedRatioUnit unit, decimal amount)
        {
            Unit = unit;
            _amount = amount;
        }

        public void ShowNumeratorDimension()//these are just dimensions...how do Units get converted within dimensions? State may be able to fix it, but switching between dimension is more imporant upfront
        {
            Unit.ProjectNumerator(ref _amount);
        }

        public void ShowDenominatorDimension()
        {
            Unit.ProjectDenominator(ref _amount);
        }
    }
}
