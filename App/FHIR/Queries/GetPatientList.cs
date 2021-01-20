using Hl7.Fhir.Model;
using MediatR;
using System;
using System.Collections.Generic;
using UWPLockStep.ApplicationLayer.Interfaces.MediatR;
using UWPLockStep.Domain.Common;

namespace UWPLockStep.ApplicationLayer.FHIR.Queries
{
    public class GetPatientList
    {
        public class Query : IRequest<Model>
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        public class Model
        {
            public Bundle Patients { get; set; }

        }

        public interface IHandler : IRequestHandler<Query, Model>
        {

        }
    }
}