using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using LockStepBlazor.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace LockStepBlazor.Handlers
{/// <summary>
/// TODO: This does not get the Medications the Patient was taking at home. This would come from MedicationUsage (http://hapi.fhir.org/baseR4/MedicationUsage?patient=921330&_list=$current-medications) for R5 or (http://hapi.fhir.org/baseR4/MedicationRequest?patient=921330&_list=$current-medications) for R4
/// this request returns the first 2 entries, with a link to the next page in Links>"relation":next. result = _client.Continue(result) would pull in the next page
/// </summary>
    public class GetFhirR4MedicationsAPIHandler : GetFhirMedicationsHandler
    {
        public GetFhirR4MedicationsAPIHandler(FhirClient client) : base(client)
        {
        }

        public override async Task<IGetFhirMedications.Model> Handle(IGetFhirMedications.Query request, CancellationToken cancellationToken)
        {
            var taskList = new List<Task<Bundle>>();//launch multiple http request from here
            //implementation will vary based on FHIR version
            var medRequestQuery = new SearchParams()
                             .Where($"patient={request.PatientId}")
                             .Include("MedicationRequest:medication")
                            //  .Include("MedicationRequest:requester")
                             .LimitTo(100);//These are only added to the bundle if there is a Medication Resource referenced, not if there is a CodeableConcept describing the Medication

            var medStatementQuery = new SearchParams()
                             .Where($"patient={request.PatientId}")
                             .Include("MedicationStatement:medication")
                            //  .Include("MedicationStatement:requester")
                             .LimitTo(100);
            
            taskList.Add(client.SearchAsync<MedicationRequest>(medRequestQuery));
            taskList.Add(client.SearchAsync<MedicationStatement>(medStatementQuery));

            var res = new IGetFhirMedications.Model();
            
            var meds =  await ParseMedicationsAsync(taskList).ConfigureAwait(false);//if this is not awaited, the  channel will start to TryRead before anything is in the channel and an empty res in returned

            meds.ToList().ForEach(dto => 
            
                res.Requests.Add(dto));
            
            return res;

        }

        
    }
}
