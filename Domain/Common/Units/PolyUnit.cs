using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using UWPLockStep.Domain.Common.Exceptions;
using UWPLockStep.Domain.Interfaces;

namespace UWPLockStep.Domain.Common.Units
{/// <summary>
/// Defines a ratio between x number of Units and/or dimensions and how to project a Value as the selected Unit. Needs to accept a UnitValue in order to define the translations. 
/// 
/// This is intended to only describe a single entity like sodium as a Factor or NaCl as an Ingredient and not to have g of Sodium(which itself is a PolyUnit) and mL of NaCl. While this would work, there should be more structure in cases like g protein and g formula powder. Converting g of protein to mg of protein is appropriate but this class is not equiped to translate g or protein to g of formula...i.e. the Conversion Methods only check for Type of UnitEnumeration and not Identity because Identity does not exist here. 
/// </summary>
    public class PolyUnit : IUnit //this only has application on a Factor or other entity. 
    {
        public UnitEnumeration UnitEnumerator { get; private set; }

        public IDictionary<UnitEnumeration, decimal> TranslationDefinitions { get; }//this should NOT accept PolyUnits because the only use for that is ratios between entities and identity does not exist here!


        /// <summary>
        /// Each MonoUnitValue has to be of a Unique Dimension or relationships that don't make sense could be defined. Like 100g= 10mg. Every MonoUnitValue that is passed in needs to be equivalent in the context of the DomainObject.
        /// </summary>
        /// <param name="unit"></param>
        public PolyUnit(params UnitValueBase[] unit)//this builds the Dictionary appropriately but I cant tell why the test fails
        {
            if (unit.GroupBy(u => u.Unit.UnitEnumerator.GetType()).Count() != unit.Count() )
            {
                throw new NonUniqueDimensionException();
            }
            TranslationDefinitions = new Dictionary<UnitEnumeration, decimal>();

            foreach (var baseUnitValue in unit)
            {
                TranslationDefinitions.Add(new KeyValuePair<UnitEnumeration, decimal>(baseUnitValue.Unit.UnitEnumerator, baseUnitValue.Amount));
            }
            //var somer =  unit.Select(u => new { u.Unit, u.Amount });// new anonymous new feature
            
            UnitEnumerator = TranslationDefinitions.Keys.First();
        }
        
        
        static bool TypeCheck(UnitEnumeration unit, UnitEnumeration newUnit) =>
            unit.GetType() == newUnit.GetType();

        public decimal Convert(decimal amount, UnitEnumeration newUnit)
        {
            if (TypeCheck(UnitEnumerator, newUnit))
            {
                return ConvertUnit(amount, newUnit);
            }
            else
            {
                return ProjectNewUnit(amount, newUnit);
            }
        }

        /// <summary>
        /// TODO: Add exception handling for Key not in Dictionary. This should never happen though, because User will select Unit from list of Units provided by Dictionary
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="newUnit"></param>
        public decimal ProjectNewUnit(decimal amount, UnitEnumeration newUnit)
        {
            if (TranslationDefinitions.Keys.All(u => u.GetType() != newUnit.GetType())) //All is an extension method public static bool All<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate);

            //This list of Units may be too long though. List needs to be collapsable? 
            {
                throw new UnitConversionException(UnitEnumerator, newUnit);
            }
            //How could you refactor this to remove the repetitive UnitType checks?
            var retValue = 
            
            amount * newUnit.
                FromBase(
                TranslationDefinitions.Keys.First(u => TypeCheck(u, newUnit)).
                ToBase(
                TranslationDefinitions[TranslationDefinitions.Keys.First(u => TypeCheck(u, newUnit))]))
                /
                TranslationDefinitions[UnitEnumerator];

            UnitEnumerator = newUnit;
            return retValue;
        }

        public decimal ConvertUnit(decimal amount, UnitEnumeration newUnit)//Type is checked in Convert method so excpection should not be needed
        {   var retValue = newUnit.FromBase(UnitEnumerator.ToBase(amount));
            UnitEnumerator = newUnit;
            return retValue;
        }

        public IUnit GetCopy()
        {
            return this.MemberwiseClone() as PolyUnit;
        }
    }
}
