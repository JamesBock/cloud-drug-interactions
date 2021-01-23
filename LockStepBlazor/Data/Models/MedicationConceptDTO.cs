using System;
using System.Net.Http;

namespace LockStepBlazor.Data.Models
{
    public class MedicationConceptDTO
    {
        public string Sys { get; set; }
        public string CodeString { get; set; }
        public string Text { get; set; }
        public string FhirType { get; set; }
        public string ResourceId { get; set; }
        public string Prescriber { get; set; }
        public DateTimeOffset? TimeOrdered { get; set; }
        //public HttpResponseMessage RxCuiResponse { get; set; }
        public string RxCui { get; set; }
    }
}