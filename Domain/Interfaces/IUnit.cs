using System;
using System.Collections.Generic;
using System.Text;
using UWPLockStep.Domain.Common.Units;

namespace UWPLockStep.Domain.Interfaces
{
    public interface IUnit //this is not meant to be interchangable with UnitEnumerator, it is representing Mass or Volume. PolyUnit also fits this definition. UnitEnumeration represents a single dimension. PolyUnit can have many. Convert methods should only take UniEnumerators but either could be used as a Unit in a ValueObject 
    {
        public UnitEnumeration UnitEnumerator { get; }
        public decimal Convert(decimal amount, UnitEnumeration newUnit);//A Unit will never need to Convert on its own, nor should it. Changing the Unit in name and not changing the amount of the associated Value is dangerous.
        public IUnit GetCopy();

        //public string Name { get; }
        //public string Abbreviation { get; }


    }
}
