using System;
using System.Collections.Generic;
using System.Text;
using UWPLockStep.Domain.Common.Exceptions;
using UWPLockStep.Domain.Interfaces;

namespace UWPLockStep.Domain.Common.Units
{
    public class MonoUnit : IUnit
    {
        public UnitEnumeration UnitEnumerator { get; private set; }

        public MonoUnit(UnitEnumeration unit)
        {
            UnitEnumerator = unit;
        }
        public decimal Convert(decimal amount, UnitEnumeration newUnit)
        {
            if (UnitEnumerator.GetType() != newUnit.GetType())
            {
                throw new UnitConversionException(UnitEnumerator, newUnit);
            }

            var retValue = newUnit.FromBase(UnitEnumerator.ToBase(amount));
            UnitEnumerator = newUnit;
            return retValue;
        }
        public IUnit GetCopy()
        {
            return this.MemberwiseClone() as MonoUnit;
        }
    }
}
