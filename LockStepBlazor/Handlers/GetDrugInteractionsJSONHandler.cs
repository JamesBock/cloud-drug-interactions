using LockStepBlazor.Application.Interfaces;
using LockStepBlazor.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LockStepBlazor.Handlers
{
    public class GetDrugInteractionsJSONHandler : GetDrugInteractionsHandler
    {
        //private HttpClient _httpClient;//this is needed for the Mediatr DI to work 
        public GetDrugInteractionsJSONHandler(HttpClient httpClient) : base(httpClient)
        {
          
        }

        public override async Task<IGetDrugInteractions.Model> Handle(IGetDrugInteractions.Query request, CancellationToken cancellationToken)
        { 
            return new IGetDrugInteractions.Model() { Meds = Task.Run(() => DrugInteractionString.InteractionString) };
        }
    }
}
