using System;
using System.Collections.Generic;
using System.Text;
using UWPLockStep.Domain.Common.Units;
using UWPLockStep.Domain.Interfaces;

namespace UWPLockStep.Domain.Common
{
    public static class UnitFactory
    {
        public static IUnit Create(params UnitValueBase[] units)
        {

            return new PolyUnit(units);

        }

        public static IUnit Create(params KeyValuePair<UnitEnumeration, decimal>[] units)
        {
            var uArray = Array.Empty<UnitValueBase>();
            {
                foreach (var unit in units)
                {
                    new MonoUnitValue(new MonoUnit(unit.Key), unit.Value);
                }
            };

            return new PolyUnit(uArray);
        }

      

        public static IUnit Create(UnitEnumeration unit)
        {
            return new MonoUnit(unit);
        }
    }
}
