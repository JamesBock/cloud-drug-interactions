using System;
using System.Collections.Generic;
using System.Text;
using UWPLockStep.ApplicationLayer.Interfaces;
using UWPLockStep.Domain.Common;

namespace UWPLockStep.Domain.Common
{/// <summary>
/// Call any Volume Type to access methods. Generic constructor is private. 
/// </summary>
    public class Volume : UnitEnumeration//, IUnit
    {

        public static readonly Volume Liter
            = new Volume(0, "Liter", "L", 1m);

        public static readonly Volume Ounce
            = new Volume(1, "Ounce", "oz", 33.814m);
        
        public static readonly Volume Mililiter
            = new Volume(2, "Mililiter", "mL", 1000m);
        /// <summary>
        /// Amount of the unit to be converted, the destination unit, the starting unit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="amount"></param>
        /// <param name="destination"></param>
        /// <param name="starting"></param>
        /// <returns></returns>
        public override decimal Convert<T>(decimal amount, T destination, T starting)  //where T : Volume //need c# 8.0 for this =(. Not using because of UWP
        {
            //var first = starting.ToBase(amount);
            //return destination.FromBase(first);

            return destination.FromBase(starting.ToBase(amount));  //same as above but a little faster

        }

        public override decimal FromBase(decimal amount)
        {
            return BaseConversion * amount;
        }

        public override decimal ToBase(decimal amount)
        {
            return amount / BaseConversion;
        }

        private Volume() {}
        private Volume(int value, string name, string abbreviation, decimal baseConversion) : base (value, name, abbreviation, baseConversion) { }
    }
}
