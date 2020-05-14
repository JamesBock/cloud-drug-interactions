using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UWPLockStep.ApplicationLayer.FHIR.Queries;

namespace UWPLockStep.Persistance.Handlers.Common
{
    public class GetMedicationRequestsHandler : GetMedicationRequests.IHandler
    {
        private readonly IFhirClient _client = new FhirClient("http://hapi.fhir.org/baseR4") { PreferredFormat = ResourceFormat.Json };//TODO:Use DI Container


        public async Task<GetMedicationRequests.Model> Handle(GetMedicationRequests.Query request, CancellationToken cancellationToken)
        {
            var q = new SearchParams()
                             .Where($"patient={request.PatientId}")
                             .Include("MedicationRequest:medication").LimitTo(1000);//How do you handle these?
            var result = await _client.SearchAsync<MedicationRequest>(q);

            var medsReqs = new List<MedicationRequest>();
            //var meds = new List<Medication>();
            foreach (var item in result.Entry)
            {
                try
                {
                    medsReqs.Add(item.Resource as MedicationRequest);

                    //meds.Add(item.Resource as Medication);//Should I Update the model to include a list of these?
                }
                catch (Exception)
                {

                    break;
                }
              
            }

          return new GetMedicationRequests.Model() { Requests = medsReqs/*, Meds = meds*/ };
            
        }
    }
}
