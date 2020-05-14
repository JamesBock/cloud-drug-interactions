using Hl7.Fhir.Model;
using MediatR;
using System;
using System.Collections.Generic;
using UWPLockStep.ApplicationLayer.Interfaces.MediatR;
using UWPLockStep.Domain.Common;

namespace UWPLockStep.ApplicationLayer.FHIR.Queries
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
