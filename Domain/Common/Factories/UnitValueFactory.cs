using System;
using System.Collections.Generic;
using System.Text;
using UWPLockStep.Domain.Common.Units;
using UWPLockStep.Domain.Interfaces;

namespace UWPLockStep.Domain.Common
{
    public static class UnitValueFactory
    {
        public static UnitValueBase Create(IUnit unit, decimal amount)
        {
            if (unit is PolyUnit)

                return new PolyUnitValue(unit.GetCopy(), amount);

            else

                return new MonoUnitValue((MonoUnit)unit.GetCopy(), amount);

        }
        public static UnitValueBase Create(UnitEnumeration unit, decimal amount)
        {

            return new MonoUnitValue(new MonoUnit(unit), amount);//This doesn't need a GetCopy because it is passing a UnitEnumerator and is also newing up instances 

        }

    }
}
