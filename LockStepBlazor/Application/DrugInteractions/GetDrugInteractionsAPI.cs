using LockStepBlazor.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockStepBlazor.Application.DrugInteractions   
{
    public class GetDrugInteractionsAPI : IGetDrugInteractions
    {
        public class Query : IRequest<Model>
        {
            public Query(List<string> medDtos)
            {
                MedDtos = medDtos;
            }
            public List<string> MedDtos { get; set; } = new List<string>();
        }

        /// <summary>
        /// From the API
        /// </summary>
        public class Model
        {
            public string Meds { get; set; }
        }

        public interface IHandler : IRequestHandler<Query, Model>
        {

        }
        
    }
}
