using System;
using System.Collections.Generic;
using System.Text;
using UWPLockStep.Domain.Interfaces;

namespace UWPLockStep.Domain.Common.Units
{/// <summary>
/// Not technically a ValueObject because the Amount can change.
/// </summary>
    public class PolyUnitValue : UnitValueBase//, IComparable //Right now this is the only UnitValue...
    {
       // public override IUnit Unit { get; } //This represents a Multi Dimensional Ratio Unit that projects as one Dimension. As an IUnit, this cannot accept a UnitEnumeration directly. Will it ever actually need to? UnitEnumeratoin can be reworked to to be an IUnit but unclear of the usefulness of this.
        
        //public override decimal Amount { get; private set; } // The Amount of the Unit that is being projected by the PolyUnit


        public PolyUnitValue(IUnit unit, decimal amount)
        {
            Unit = unit;
            _amount = amount;
        }

        public override void Convert(UnitEnumeration newUnit)
        {
            _amount = Unit.Convert(Amount, newUnit);
        
        }

        
    }
}
