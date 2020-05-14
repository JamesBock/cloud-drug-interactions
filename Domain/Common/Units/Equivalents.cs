using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UWPLockStep.Domain.Common.Units
{
    public class Equivalents : UnitEnumeration
    {
        public static readonly Equivalents Milliequivalent
         = new Equivalents(0, "Milliequivalent", "mEq", 1m);

        //public override UnitEnumeration SetToBase()
        //{
             
        //    return Milliequivalent;
        //}

        public Equivalents() { }
        public Equivalents(int value, string name, string abbreviation, decimal baseConversion) : base(value, name, abbreviation, baseConversion) { }
    }
}
