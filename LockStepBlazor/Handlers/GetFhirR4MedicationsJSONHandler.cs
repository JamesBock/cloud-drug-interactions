using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Hl7.FhirPath.Sprache;
using LockStepBlazor.Data;
using LockStepBlazor.Data.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UWPLockStep.ApplicationLayer.Interfaces.MediatR;
using UWPLockStep.Domain.Common;
using Task = System.Threading.Tasks.Task;

namespace LockStepBlazor.Handlers
{
    public class GetFhirR4MedicationsJSONHandler : GetFhirMedicationsHandler
    {
        public GetFhirR4MedicationsJSONHandler(IFhirClient client) : base(client)
        {
        }

        public override async Task<IGetFhirMedications.Model> Handle(IGetFhirMedications.Query request, CancellationToken cancellationToken)
        {
            await ParseMedicationsAsync(MedicationJSONString.ParseMedsAsync());
            var res = new IGetFhirMedications.Model();

            while (channel.Reader.TryRead(out var dto))//behavior is odd under the debugger
            {
                res.Requests.Add(dto);//Channels are out of the practical scope of this project but it was a good way to learn how to use them...How would they be applicable?
            }

            return res;

        }



    }
}
