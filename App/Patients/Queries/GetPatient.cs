using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPLockStep.Domain.Common;

using UWPLockStep.Domain.Entities.People;
using UWPLockStep.Domain.Interfaces;

namespace UWPLockStep.ApplicationLayer.Patients.Queries
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
