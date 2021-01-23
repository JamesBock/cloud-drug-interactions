using LockStepBlazor.Data.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LockStepBlazor.Application.Interfaces
{
    /// <summary>
    /// This interface abstracts the query for medications away from retrieving them from a FHIR server. This seems to be getting injected through the built-in DI container to MediatR but unsure how
    /// </summary>
    public interface IGetFhirMedications
    {
        public class Query : IRequest<Model>
        {
            public string PatientId { get; set; }
        }

        public class Model
        {
            public List<MedicationConceptDTO> Requests { get; set; } = new List<MedicationConceptDTO>();

        }

        public interface IHandler : IRequestHandler<Query, Model>
        {

        }
    }
}
