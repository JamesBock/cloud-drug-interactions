using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Hl7.FhirPath.Sprache;
using LockStepBlazor.Application.Interfaces;
using LockStepBlazor.Data;
using LockStepBlazor.Data.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace LockStepBlazor.Handlers
{
    public class GetFhirR4MedicationsJSONHandler : GetFhirMedicationsHandler
    {
        public GetFhirR4MedicationsJSONHandler(FhirClient client) : base(client)
        {
        }

        public override async Task<IGetFhirMedications.Model> Handle(IGetFhirMedications.Query request, CancellationToken cancellationToken)
        {
            //var meds =  await ParseMedicationsAsync(MedicationJSONString.ParseMedsAsync());
            var res = new IGetFhirMedications.Model();

            //meds.ToList().ForEach(dto=> res.Requests.Add(dto));
            
            return res;

        }



    }
}
