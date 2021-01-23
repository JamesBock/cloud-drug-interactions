using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using MediatR;
using System;
using System.Collections.Generic;


namespace LockStepBlazor.Application.Fhir.Queries
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