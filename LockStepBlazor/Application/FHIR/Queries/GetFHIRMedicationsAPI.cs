using Hl7.Fhir.Model;
using LockStepBlazor.Application.Interfaces;
using LockStepBlazor.Data.Models;
using MediatR;
using System;
using System.Collections.Generic;

namespace LockStepBlazor.Application.Fhir.Queries
{
    public class GetFhirMedicationsAPI : IGetFhirMedications//Having this abstraction allows you to swap out other handlers for the same process, potentially for different versions of FHIR.
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
