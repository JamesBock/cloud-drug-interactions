using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UWPLockStep.ApplicationLayer.FHIR.Queries;
using UWPLockStep.ApplicationLayer.Interfaces.MediatR;
using UWPLockStep.Domain.Common;
using Task = System.Threading.Tasks.Task;

namespace LockStepBlazor.Handlers
{/// <summary>
/// TODO: This does not get the Medications the Patient was taking at home. This would come from MedicationUsage (http://hapi.fhir.org/baseR4/MedicationUsage?patient=921330&_list=$current-medications) for R5 or (http://hapi.fhir.org/baseR4/MedicationRequest?patient=921330&_list=$current-medications) for R4
/// this request returns the first 2 entries, with a link to the next page in Links>"relation":next. result = _client.Continue(result) would pull in the next page
/// </summary>
    public class GetFhirR4MedicationsAPIHandler : GetFhirMedicationsHandler
    {
        public GetFhirR4MedicationsAPIHandler(IFhirClient client) : base(client)
        {
        }

        public override async Task<IGetFhirMedications.Model> Handle(IGetFhirMedications.Query request, CancellationToken cancellationToken)
        {
            //implementation will vary based on FHIR version
            var q = new SearchParams()
                             .Where($"patient={request.PatientId}")
                             .Include("MedicationRequest:medication").LimitTo(1000);//These are only added to the bundle if there is a Medication Resource referenced, not if there is a CodeableConcept describing the Medication

            var t = new SearchParams()
                             .Where($"patient={request.PatientId}")
                             .Include("MedicationStatement:medication").LimitTo(1000);

            var trans = new TransactionBuilder(_client.Endpoint);
            trans = trans.Search(q, "MedicationRequest").Search(t, "MedicationStatement");
            //var result = await _client.SearchAsync<MedicationRequest>(q);
            var tResult = _client.TransactionAsync(trans.ToBundle());

            var res = new IGetFhirMedications.Model();
            
            await ParseMedicationsAsync(tResult).ConfigureAwait(false);//if this is not awaited, the  channel will start to TryRead before anything is in the channel and an empty res in returned

            while (channel.Reader.TryRead(out var dto))//what if this was switched to not closed and the ParseMedicationAsync isnt awaited?...Closes before it gets here
            {
                res.Requests.Add(dto);//Channels might be out of the practical scope of this project but it was a good way to learn how to use them...How would they be applicable?
            }
            //while (!channel.Reader.Completion.IsCompleted)//what if this was switched to not closed and the ParseMedicationAsync isnt awaited?
            //{
            //    res.Requests.Add(await channel.Reader.ReadAsync());//Channels might be out of the practical scope of this project but it was a good way to learn how to use them...How would they be applicable?
            //}


            return res;

        }

        
    }
}
