using System;
using System.Collections.Generic;
using System.Text;
using UWPLockStep.Domain.Interfaces;

namespace UWPLockStep.Domain.Common.Units
{
    public class Mass : UnitEnumeration
    {
        public static readonly Mass Gram //Rename to Base?
          = new Mass(0, "Gram", "g", 1m);

        public static readonly Mass Milligram
            = new Mass(1, "Milligram", "mg", 1000m);

        public static readonly Mass Pound
            = new Mass(2, "Pound", "lb", 0.0022046m);

        public static readonly Mass Kilogram
          = new Mass(3, "Kilogram", "kg", 0.001m);

        public static readonly Mass Microgram
          = new Mass(4, "Microgram", "mcg", 1000000m);

        public static readonly Mass Ounce
          = new Mass(5, "Ounce", "oz", 0.035274m);

        //public override UnitEnumeration SetToBase()
        //{            
        //    return Gram;
        //}

        public override decimal FromBase(decimal amount)
        {
            return base.FromBase(amount);
        }

        public override decimal ToBase(decimal amount)
        {
            return base.ToBase(amount);
        }
        /// <summary>
        /// As is, if another UnitEnum is passed in like Volume, it would run...this cannot work like this. Convert method in UnitValue class is type safe too but isn't without generics.
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="unit2"></param>
        /// <returns></returns>
        //public static MonoUnitValue<Mass> Convert(MonoUnitValue<Mass> unit, Mass unit2)
        //{ 
        //    return new MonoUnitValue<Mass>(unit2, unit.Unit.FromBase(unit2.ToBase(unit.Amount)));
            
        //}
        protected Mass() { }
        protected Mass(int value, string name, string abbreviation, decimal baseConversion) : base(value, name, abbreviation, baseConversion) { }
    }
}
