using System;
using System.Collections.Generic;
using System.Text;

namespace UWPLockStep.Domain.Common.Units
{/// <summary>
/// Amounts in this program do not account for the actual number the Unit represents as is serves no benefit. Other Amount Units potentially like count or Units shold be a seperate class as there is no reason to ever convert one to the other.
/// </summary>
    public class AmountOfElement : UnitEnumeration
    {

        public static readonly AmountOfElement Mole 
            = new AmountOfElement(0, "Mole", "mol", 1m);
        public static readonly AmountOfElement Millimole
            = new AmountOfElement(1, "Millimole", "mmol", 1000m);
       
        //public override UnitEnumeration SetToBase()
        //{

        //    return Mole;
        //}

        public AmountOfElement() {}
        public AmountOfElement(int value, string name, string abbreviation, decimal baseConversion) : base(value, name, abbreviation, baseConversion) { }
    }
}
