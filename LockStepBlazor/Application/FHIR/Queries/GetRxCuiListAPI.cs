using Hl7.Fhir.Model;
using LockStepBlazor.Data.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace LockStepBlazor.Application.Fhir.Queries
{
    /// <summary>
    /// Takes in a List of MedicationRequests and returns a coresponding list of RxCUIs from the RxNorm API
    /// </summary>
    public class GetRxCuiListAPI
    {
        public class Query : IRequest<Model>
        {
            public List<MedicationConceptDTO> Requests { get; set; }
        }

        public class Model
        {
            public List<string> MedDtos { get; set; } = new List<string>();
        }

        public interface IHandler : IRequestHandler<Query, Model>
        {

        }
    }
}
