using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LockStepBlazor.Application.Interfaces;

namespace LockStepBlazor.Handlers
{
    public abstract class GetDrugInteractionsHandler : IGetDrugInteractions.IHandler
    {
        protected readonly HttpClient _httpClient;

        public GetDrugInteractionsHandler(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public abstract Task<IGetDrugInteractions.Model> Handle(IGetDrugInteractions.Query request, CancellationToken cancellationToken);
        //{
        //    return null;
        //    //throw new NotImplementedException();
        //}
    }
}
