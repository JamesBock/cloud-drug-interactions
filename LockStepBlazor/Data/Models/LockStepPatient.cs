using System;
using System.Collections.Generic;
using System.Linq;
using Hl7.Fhir.Model;
using UWPLockStep.Domain.Common;

namespace LockStepBlazor.Data.Models
{
    public class LockStepPatient : Patient
    {
        public LockStepPatient()
        {

        }
        public LockStepPatient(DateTimeOffset? birthday, DateTimeOffset? conception, List<string> givenNames, string lastName)// there should be a lot of information needed to create a new Patient.
        {
            LockStepId = Guid.NewGuid();
            //Orders = new List<OrderItemBase>();
            GivenNames = givenNames;
            LastName = lastName;


        }
        public Patient FhirPatient { get; set; }

        public List<MedicationConceptDTO> Medications { get; set; } = new List<MedicationConceptDTO>();
        public List<string> GivenNames { get; set; } = new List<string>();

        public string LastName { get; set; }

        public Guid LockStepId { get; }

        public int RoomId { get; set; }

        public DateTimeOffset? DateOfBirth { get; set; } = new DateTimeOffset();//duplicated on PatientTypeStateless

        public DateTimeOffset? DateOfConception { get; set; } = new DateTimeOffset();//duplicated on PatientTypeStateless

        public decimal DayOfLife { get; private set; }//duplicated on PatentTypeStateless as Age

        //public MonoUnitValue Weight { get; set; } //Weight will always be a MonoUnitValue. No reason to use UnitValueBase.

        //public User Practitioner { get; set; }//This is for navigation?

        // public ICollection<OrderItemBase> Orders { get; set; }

        //TODO: This is incomplete. 
        public LockStepPatient GetCopy()
        {
            var clone = new LockStepPatient(/*DateTime.Now - TimeSpan.FromDays(15), DateTime.Now - TimeSpan.FromDays(230), GivenNames, LastName*/);
            //clone.Orders = this.Orders.Select(o => o.GetCopy()).ToList();
            //clone.Weight = Weight;
            return clone;
        }

        public void SetBirthStatus(DateTimeOffset birthday)
        {
            DateOfBirth = birthday;


        }
    }
}
