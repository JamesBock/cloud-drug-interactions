using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockStepBlazor.Data.Models
{
    /// <summary>
    /// this is essentially a view model class
    /// </summary>
    public class MedicationInteractionPair
    {
        public Guid InteractionId { get; set; }

        public string Comment { get; set; }

        public (MedicationInteractionViewModel, MedicationInteractionViewModel) MedicationPair { get; set; }//Medication pair string tuple was problematic in the form because i need sto have more data related to each specific entity not the pair as a whole. 

        public List<InteractionDetail> DrugInteractionDetails { get; set; } = new List<InteractionDetail>();


        public class InteractionDetail
        {
            public string InteractionAssertion { get; set; }

            public string Severity { get; set; }

            public string Description { get; set; }

            public List<(string, Uri)> LinkTupList { get; set; }
        }

        public class MedicationInteractionViewModel
        {
            public Guid InteractionId { get; set; }

            public string DisplayName { get; set; }

            public string RxCui { get; set; }

            public string FhirType { get; set; }

            public DateTimeOffset? TimeOrdered { get; set; }

            public string ResourceId { get; set; }

            public string Prescriber { get; set; }
        }
    }
}
