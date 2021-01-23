using LockStepBlazor.Application.Interfaces;
using LockStepBlazor.Data.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace LockStepBlazor.Application.Fhir.Queries
{
    public class GetFhirMedicationsJSON : IGetFhirMedications//Having this abstraction allows you to swap out other handlers for the same process, potentially for different versions of FHIR.
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
