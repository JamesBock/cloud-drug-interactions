using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UWPLockStep.Domain.Entities.People;
using UWPLockStep.Domain.Interfaces;

namespace UWPLockStep.Domain.Common.Units
{
    public class IntermediatePolyUnit //: IUnit
    {
        public IDictionary<Guid, KeyValuePair<UnitEnumeration, decimal>> UnitPairs { get; }//The reason this is a nested dictionary is that this maintains the relationship between two identities as they are described in SPECIFIC Units, not necessarily the Units the objects are currently being shown as.

        public ILookup<Guid,Tuple<UnitEnumeration, decimal>> ElementTable { get; } //This may be easier to work with...will hold this for now and implement as appropriate.


        //public UnitEnumeration Unit { get; private set; }//Could be used for display purposes if needed


        public IntermediatePolyUnit(decimal numeratorAmount, DomainObject numerator, DomainObject denominator)
        {
            UnitPairs = new Dictionary<Guid, KeyValuePair<UnitEnumeration, decimal>>();

            UnitPairs.Add(new KeyValuePair<Guid, KeyValuePair<UnitEnumeration, decimal>>(numerator.Id, new KeyValuePair<UnitEnumeration, decimal>(numerator.Unit.UnitEnumerator, numeratorAmount)));
            UnitPairs.Add(new KeyValuePair<Guid, KeyValuePair<UnitEnumeration, decimal>>(denominator.Id, new KeyValuePair<UnitEnumeration, decimal>(denominator.Unit.UnitEnumerator, 1m)));

            
            ElementTable = UnitPairs
                .ToLookup(
                x => x.Key, x=> Tuple.Create(x.Value.Key , x.Value.Value));
        }
        /// <summary>
        /// this would be used in Policies where weight is needed in theory
        /// </summary>
        /// <param name="numeratorAmount"></param>
        /// <param name="numerator"></param>
        /// <param name="denominator"></param>
        public IntermediatePolyUnit(decimal numeratorAmount, DomainObject numerator, LockStepPatient denominator)
        {
            UnitPairs = new Dictionary<Guid, KeyValuePair<UnitEnumeration, decimal>>();

            UnitPairs.Add(new KeyValuePair<Guid, KeyValuePair<UnitEnumeration, decimal>>(numerator.Id, new KeyValuePair<UnitEnumeration, decimal>(numerator.Unit.UnitEnumerator, numeratorAmount)));
            UnitPairs.Add(new KeyValuePair<Guid, KeyValuePair<UnitEnumeration, decimal>>(denominator.LockStepId, new KeyValuePair<UnitEnumeration, decimal>(denominator.Weight.Unit.UnitEnumerator, 1m)));


            ElementTable = UnitPairs
                .ToLookup(
                x => x.Key, x => Tuple.Create(x.Value.Key, x.Value.Value));
        }

    }
}
