using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using MediatR;
using System;
using System.Collections.Generic;
using UWPLockStep.ApplicationLayer.Interfaces.MediatR;
using UWPLockStep.Domain.Common;

namespace UWPLockStep.ApplicationLayer.FHIR.Queries
{
    public class NavigateBundle
    {
        public class Query : IRequest<Model>
        {
            public Bundle Bundle { get; set; }
            public PageDirection Nav { get; set; }
        }

        public class Model
        {
            public Bundle Bundle { get; set; }

        }

        public interface IHandler : IRequestHandler<Query, Model>
        {

        }
    }
}