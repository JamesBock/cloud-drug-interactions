using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockStepBlazor.Application.Interfaces
{
    public interface IGetDrugInteractions
    {
        public class Query : IRequest<Model>
        {
            public Query(List<string> medDtos)
            {
                MedDtos = medDtos;
            }
            public List<string> MedDtos { get; set; } = new List<string>();
        }

        public class Model
        {
            public Task<string> Meds { get; set; }
        }

        public interface IHandler : IRequestHandler<Query, Model>
        {

        }
    }
}
