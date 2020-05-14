using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPLockStep.Domain.Entities.People;
using UWPLockStep.Domain.Interfaces;

namespace UWPLockStep.Domain.Common.Units
{
    public class IntermediatePolyWithUnitValue
    {
        public IDictionary<Guid, UnitValueBase> UnitPairs { get; }//Replaces <UnitEnum, decimal> KVP with IUnitValue. Unclear why this was not done to start. this would help maintain Units and allow for easy conversion.

        public ILookup<Guid, UnitValueBase> ElementTable { get; } //This may be easier to work with...will hold this for now and implement as appropriate.


        //public UnitEnumeration Unit { get; private set; }//Could be used for display purposes if needed


        public IntermediatePolyWithUnitValue(decimal numeratorAmount, DomainObject numerator, DomainObject denominator)
        {
            UnitPairs = new Dictionary<Guid, UnitValueBase>();

            UnitPairs.Add(new KeyValuePair<Guid, UnitValueBase>(numerator.Id, UnitValueFactory.Create(numerator.Unit, numeratorAmount)));
            UnitPairs.Add(new KeyValuePair<Guid, UnitValueBase>(denominator.Id, UnitValueFactory.Create(denominator.Unit.UnitEnumerator, 1m)));


            //ElementTable = UnitPairs
            //    .ToLookup(
            //    x => x.Key, x => Tuple.Create(x.Value.Key, x.Value.Value));
        }
        /// <summary>
        /// this would be used in Policies where weight is needed in theory
        /// </summary>
        /// <param name="numeratorAmount"></param>
        /// <param name="numerator"></param>
        /// <param name="denominator"></param>
        public IntermediatePolyWithUnitValue(decimal numeratorAmount, DomainObject numerator, LockStepPatient denominator)
        {
            UnitPairs = new Dictionary<Guid, UnitValueBase>();


            UnitPairs.Add(new KeyValuePair<Guid, UnitValueBase>(numerator.Id, UnitValueFactory.Create(numerator.Unit, numeratorAmount)));

            UnitPairs.Add(new KeyValuePair<Guid, UnitValueBase>(denominator.LockStepId, UnitValueFactory.Create(denominator.Weight.Unit.UnitEnumerator, 1m)));


            //ElementTable = UnitPairs
            //    .ToLookup(
            //    x => x.Key, x => Tuple.Create(x.Value.Key, x.Value.Value));
        }

        public void ConvertNumerator(UnitEnumeration unit) 
        {
            UnitPairs.ElementAt(0).Value.Convert(unit);
            //return new IntermediatePolyWithUnitValue()
        }
    }
}
