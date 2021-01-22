using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Specification;

namespace LockStepBlazor.Shared
{
    public class GoogleFhirClient : FhirClient
    {       
        public GoogleFhirClient(Uri endpoint, FhirClientSettings settings = null, HttpMessageHandler messageHandler = null, IStructureDefinitionSummaryProvider provider = null) : base(endpoint, settings, messageHandler, provider)
        { 
            var creds = GoogleCredential.GetApplicationDefault().CreateScoped("https://www.googleapis.com/auth/cloud-platform");
            var toks = creds.UnderlyingCredential.GetAccessTokenForRequestAsync("https://oauth2.googleapis.com/token");

            RequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", toks.GetAwaiter().GetResult());
            Settings.PreferredFormat = ResourceFormat.Json;
            
        }   
       
    }
}
