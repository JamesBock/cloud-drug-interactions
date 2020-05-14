using System;
using System.Collections.Generic;
using System.Text;
using UWPLockStep.Domain.Interfaces;

namespace UWPLockStep.Domain.Common.Units
{ 
    public class MonoUnitValue : UnitValueBase
    {
        //public IUnit Unit { get; private set; }
       // public decimal Amount { get; private set; }

        public MonoUnitValue(MonoUnit unit, decimal amount)
        {
            Unit = unit;
            _amount = amount;
        }

        public override void Convert(UnitEnumeration newUnit) //if you used out parameter modifier, this could possibly be moved.
        {
            _amount = Unit.Convert(Amount, newUnit);

        }
        private MonoUnitValue() { }
    }
}
