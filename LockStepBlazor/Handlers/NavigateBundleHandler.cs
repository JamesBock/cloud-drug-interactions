using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Model;
using System.Collections.Generic;
using LockStepBlazor.Application.Fhir.Queries;

namespace LockStepBlazor.Handlers
{
    public class NavigateBundleHandler : NavigateBundle.IHandler
    {
        private readonly FhirClient client;

        public NavigateBundleHandler(FhirClient client)
        {
            this.client = client;

        }

        public async Task<NavigateBundle.Model> Handle(NavigateBundle.Query request, CancellationToken cancellationToken)
        {        
              var bundle = client.Continue(request.Bundle, request.Nav);
         
            return new NavigateBundle.Model() {Bundle = bundle};


        
        }


    }

}

