using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Text;
using UWPLockStep.Domain.Interfaces;

namespace UWPLockStep.Domain.Common
{
    public abstract class DomainObject //Designing this class for entities that can be mixed in a PolyUnit. The Properties of this class will define the identity of the Entity. This is the base class for Entities like Factor, Ingredient and Order(maybe)
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual IUnit Unit { get; set; }
        /// <summary>
        /// Use this instead of ID and Name
        /// </summary>
        public CodeableConcept TerminologyDefinition { get; set; }

        public DomainObject()
        {
            Id = Guid.NewGuid();
           
        }

        public virtual DomainObject GetCopy()
        {
            return MemberwiseClone() as DomainObject;
        }
        
        //public virtual DomainObject ShallowClone() { return (DomainObject)this.MemberwiseClone(); }
        //public virtual DomainObject DeepCopy() 
        //{ 
            
            
        //    DomainObject domainObject = (DomainObject)this.MemberwiseClone();
        //    domainObject.Id = Guid.NewGuid();
        //    domainObject.Name = String.Copy(Name);
        //    domainObject.Unit = Unit;
        //    return domainObject;

        //}
    }
}
