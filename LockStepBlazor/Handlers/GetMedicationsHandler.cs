using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hl7.Fhir.Model;
using Newtonsoft.Json.Linq;



namespace LockStepBlazor.Handlers
{
    /// <summary>
    /// Unclear if this class is needed
    /// </summary>
    public class GetMedicationsHandler //: GetMedications.IHandler
    {
        //public async Task<GetMedications.Model> Handle(GetMedications.Query request, CancellationToken cancellationToken)
        //{
        //    var meds =  new List<Medication>();

        //    request.Requests.ForEach(r => meds.Add(

        //        new Medication()
        //        {
        //            Code = r.Medication as CodeableConcept

        //        }));
        //    return new GetMedications.Model() { Meds = meds };
        //}
        //private async IAsyncEnumerable<Medication> FetchMeds(IAsyncEnumerable<MedicationRequest> reqs)
        //{
        //    yield return await reqs.ForEachAwaitAsync(r =>

        //        new Medication()
        //        {
        //            Code = r.Medication as CodeableConcept

        //        });
        //}
    }
}
