using System;
using System.Collections.Generic;
using System.Text;
using UWPLockStep.Domain.Interfaces;

namespace UWPLockStep.Domain.Common.Units
{   /// <summary>
/// this class is meant to aid the conversion of Factors that can be described in different dimensions. It maintains the ratio but only displays as one Unit at a time. This is close to fixing the mEq problem. if Valence is a part of AmountOfElement this would work
/// </summary>
    public class DerivedRatioUnit //: IUnit
    {
        public UnitEnumeration Unit { get; private set; }//If this isn't a UnitEnumerator, this can no longer be a IUnit. This may need to take in a derivedUnit but the purpose of this class is to prevent any Factor from being a derived Unit and then causing three or more UNits to be stacked. Currently does not have a default projected UNit but should likely be the Numerator
        public UnitEnumeration Numerator { get; } //this is intended to accomodate for derived units
        public UnitEnumeration Denominator { get; }
        public decimal Ratio { get; }
        public string Name { get; }
        public string Abbreviation { get; }


        public DerivedRatioUnit(UnitEnumeration numerator, UnitEnumeration denominator, decimal ratio)
        {
            Unit = numerator;
            Numerator = numerator;
            Denominator = denominator;
            Ratio = ratio;
            Name = string.Join(" per ", Numerator.Name, Denominator.Name);
            Abbreviation = string.Join("/", Numerator.Abbreviation, Denominator.Abbreviation);
        }
        public DerivedRatioUnit(MonoUnitValue numerator, MonoUnitValue denominator)//If parameters are always passed as Values, there could be a many Units as you wanted and this could project as any of them, IF the Amounts of each Units are equivalent (22g, 1mEq, 0.15mL) 22g = 0.15mL = 1mEq AND there is a Collection of UnitEnumerators...However this may not be approprate in some cases? As in g/kg
        {
            Unit = numerator.Unit.UnitEnumerator;
            Ratio = numerator.Amount / denominator.Amount;
            Name = string.Join(" per ", Numerator.Name, Denominator.Name);
            Abbreviation = string.Join("/", Numerator.Abbreviation, Denominator.Abbreviation);
        }

        public void ProjectNumerator(ref decimal amount) //this is written with the intent that this class will always be a part of a ValueObject
        {
            if (Unit != Denominator)
            {
                return;
            }
            Unit = Numerator;
            amount *= Ratio;
        }
        public void ProjectDenominator(ref decimal amount)
        {
            if (Unit != Numerator)
            {
                return;
            }
            Unit = Denominator;
            amount /= Ratio;
        }
        public override string ToString()
        {
            return string.Concat(Name, " (", Abbreviation, ")");
        }

    }
}
