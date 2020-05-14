using System;
using System.Collections.Generic;
using System.Text;
using UWPLockStep.Domain.Interfaces;

namespace UWPLockStep.Domain.Common.Units
{
    /// <summary>
    /// This class does a good job of keeping the relationship between the two Unit in sync, but could a helper converter class also do this, and without the bulky class.
    /// This is an owned unit / ValueObject
    /// </summary>
    /// <typeparam name="TNumerator"></typeparam>
    /// <typeparam name="TDenominator"></typeparam>
    public class DerivedDimensionlessValue<TNumerator, TDenominator> /*: IUnitValue<UnitEnumeration> */
        where TNumerator : UnitEnumeration where TDenominator : UnitEnumeration

    {   public UnitEnumeration Unit { get; private set; } //this satisfies the IUnitValue interface

        public decimal Amount { get; private set; } //This is the same as the Amount on the MonoUnitValue i.e. How many (count) Units are there?

        //public IUnitValue<TNumerator> UnitValueNumerator { get; private set; } //can be a Base or Dervived UnitValue

        //public MonoUnitValue<TDenominator> UnitValueDenominator { get; private set; }
          

        //public DerivedDimensionlessValue(IUnitValue<UnitEnumeration> numerator, MonoUnitValue<TDenominator> denominator/*, decimal amount*/)
        //{
        //   Unit = new DerivedDimensionlessEnum<UnitEnumeration, TDenominator>(numerator, denominator);
        //}
        public DerivedDimensionlessValue(UnitEnumeration numerator, TDenominator denominator, decimal amount)
        {
            Unit = new DerivedDimensionlessEnum<UnitEnumeration, TDenominator>(numerator, denominator, amount);
        }
        public DerivedDimensionlessValue(UnitEnumeration derivedEnum)
        {
                
        }
        public void Convert(UnitEnumeration finish)//this needs to rely on the DDE for guidance on how a conversion would work; as UnitEnumeration has ToBase and FromBase
        {

        }

        //public void ConvertNumerator(TNumerator finish)//DDE is readonly immutable so new instance is needed when changes are made
        //{
        //    UnitValueNumerator.Convert(finish); //if this is a derived unit, how would this be programmed into a VM? 
        //    Unit = new DerivedDimensionlessEnum<TNumerator, TDenominator>(UnitValueNumerator, UnitValueDenominator);
        //}

        //public void ConvertDenominator(TDenominator finish)//needs to update the DimensionlessRatio on Unit and the Amount on top of the conversions
        //{
        //    UnitValueDenominator.Convert(finish); //this changes the Unit and scales the Value (e.g. 1000g to 1kg)
        //    Unit = new DerivedDimensionlessEnum<TNumerator, TDenominator>(UnitValueNumerator, UnitValueDenominator); //The DDEnum creates a new DimensionlessRatio and updates the Units, with ToString override
        //}
              
    }
}
