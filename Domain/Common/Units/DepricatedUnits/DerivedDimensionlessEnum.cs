using System;
using System.Collections.Generic;
using System.Text;
using UWPLockStep.Domain.Interfaces;

namespace UWPLockStep.Domain.Common.Units
{
    /// <summary>
    ///This needs not to be a UnitEnumeration but other class depend on it as such. An interface that defines the overlap in functionality of UnitEnum and this Class needs to be defined. It could be used in policy definitions
    /// </summary>
    /// <typeparam name="TNumerator"></typeparam>
    /// <typeparam name="TDenominator"></typeparam>
    public class DerivedDimensionlessEnum<TNumerator, TDenominator> : UnitEnumeration where TNumerator : UnitEnumeration where TDenominator : UnitEnumeration
    {
        public /*override*/ string Name { get; /*private set;*/ } //this is to hide the inheirited name 

        public /*override */string Abbreviation { get; /*private set;*/ }

        public TNumerator UnitNumerator { get; /*private set;*/ }

        public TDenominator UnitDenominator { get; /*private set; */}

        public decimal DimensionlessRatio { get;/* private set;*/ }

        //public override UnitEnumeration SetToBase()
        //{
        //    throw new NotImplementedException();
        //}
        /// <summary>
        /// In what scenario will two UnitValue objects with Values populated be available to form one of these units? In the DerivedDimensionlessValue Class's Convert method.
        /// In what scenario will a DDE be created outside of two Entites (Factor, Ingredient, Order, Patient)? This is important because there may need to be other information used in setting these units
        /// </summary>
        /// <param name="numerator"></param>
        /// <param name="denominator"></param>
        //public DerivedDimensionlessEnum(IUnitValue<TNumerator> numerator, IUnitValue<TDenominator> denominator)
        //{
        //    UnitNumerator = numerator.Unit;
        //    UnitDenominator = denominator.Unit;
        //    DimensionlessRatio = numerator.Amount / denominator.Amount;
        //    Name = string.Join(" per ", UnitNumerator.Name, UnitDenominator.Name);
        //    Abbreviation = string.Join("/", UnitNumerator.Abbreviation, UnitDenominator.Abbreviation);
        //}
        public DerivedDimensionlessEnum(TNumerator numerator, TDenominator denominator,  decimal ratio)
        {
            DimensionlessRatio = ratio;
            UnitNumerator = numerator;
            UnitDenominator = denominator;
            Name = string.Join(" per ", UnitNumerator.Name, UnitDenominator.Name);
            Abbreviation = string.Join("/", UnitNumerator.Abbreviation, UnitDenominator.Abbreviation);
        }

        // Unclear what the implementation of this method is. 
        ///// <summary>
        ///// This takes in
        ///// </summary>
        ///// <param name="denominator"></param>
        ///// <returns></returns>
        public decimal ConvertToNumerator(MonoUnitValue denominator)
        {
            return denominator.Amount * denominator.Unit.UnitEnumerator.BaseConversion * DimensionlessRatio * UnitDenominator.BaseConversion;
        }

        public override string ToString()
        {
            return string.Concat(Name," (",Abbreviation,")");
        }




        //public int CompareTo(object other)
        //{
        //    throw new NotImplementedException();
        //}

        //public decimal FromBase(decimal amount)//This is the same purpose as ConvertToNumerator and also uses a decimal f
        //{
        //    throw new NotImplementedException();
        //}

        //public decimal ToBase(decimal amount)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
