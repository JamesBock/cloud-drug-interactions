using System;
using System.Collections.Generic;
using System.Text;



namespace UWPLockStep.Domain.Common.Units
{
    public class Volume : UnitEnumeration//, IUnit
    {

        public static readonly Volume Liter
            = new Volume(0, "Liter", "L", 1m);

        public static readonly Volume Ounce
            = new Volume(1, "Ounce", "oz", 33.814m);
        
        public static readonly Volume Milliliter
            = new Volume(2, "Milliliter", "mL", 1000m);

        /// <summary>
        /// Amount of the unit to be converted, the destination unit, the starting unit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="amount"></param>
        /// <param name="destination"></param>
        /// <param name="starting"></param>
        /// <returns></returns>
        //public  decimal Convert<Volume>(decimal amount, UnitEnumeration destination, UnitEnumeration starting)
        //{
        //    //var first = starting.ToBase(amount);
        //    //return destination.FromBase(first);

        //    return destination.FromBase(starting.ToBase(amount));  //same as above but a little faster

        //}

        //public override UnitEnumeration SetToBase()
        //{

        //    return Liter;
        //}

        public override decimal FromBase(decimal amount)
        {
           return base.FromBase(amount);
        }

        public override decimal ToBase(decimal amount)
        {
            return base.ToBase(amount);
        }

        private Volume() {}
        private Volume(int value, string name, string abbreviation, decimal baseConversion) : base (value, name, abbreviation, baseConversion) { }
    }
}
