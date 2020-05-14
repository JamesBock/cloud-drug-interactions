using System;
using System.Collections.Generic;
using System.Text;

namespace UWPLockStep.Domain.Common.Units
{
    public class MolarMass : Mass
    {
        //public delegate decimal ElementDelegate(ElementEnumeration element);
        public static readonly MolarMass Milliequivalent = new MolarMass(6, "Milliequivalent", "mEq", 1000m, 2);
        public static readonly MolarMass Millimole = new MolarMass(6, "Millimole", "mmol", 1000m,2);

        private ElementEnumeration Element { get;  set; } 
        
        public override decimal FromBase(decimal amount)
        {
            return base.FromBase(amount) / (Element.AtomicWeight.Amount * Element.Valence);
        }

        public override decimal ToBase(decimal amount)
        {
            return base.ToBase(amount) * (Element.AtomicWeight.Amount / Element.Valence);
        }

        private MolarMass(){}
        private MolarMass(int value, string name, string abbreviation, decimal baseConversion) : base(value, name, abbreviation, baseConversion) { SetElement(); }
        
        // adding another parameter to the ctor will not work with how the static Objects are working
       // private MolarMass(int value, string name, string abbreviation, decimal baseConversion, ElementEnumeration element) : base(value, name, abbreviation, baseConversion) { }
        public void SetElement(ElementEnumeration element)
        {
            Element = element;
        }
    }





}
