using MediatR;
using LockStepBlazor.Data.Models;

namespace LockStepBlazor.Application
{
    public class GetPatient
    {
        public class Query : IRequest<Model>
        {
            public string PatientId { get; set; }
        } 
        
        public class Model
        {
            public LockStepPatient QueriedPatient { get; set; }
        }

        public interface IHandler : IRequestHandler<Query, Model>
        {

        }
    }
}
