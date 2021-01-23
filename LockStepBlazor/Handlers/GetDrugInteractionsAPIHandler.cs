using LockStepBlazor.Application.Interfaces;
using LockStepBlazor.Data;
using LockStepBlazor.Shared;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace LockStepBlazor.Handlers
{
    public class GetDrugInteractionsAPIHandler : GetDrugInteractionsHandler
    {
        
       
        public GetDrugInteractionsAPIHandler(HttpClient httpClient) : base(httpClient)
        {
            //httpClient = httpClient;
        }

        public override async Task<IGetDrugInteractions.Model> Handle(IGetDrugInteractions.Query request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response;
                        
            try
            {
                response = await _httpClient.GetAsync($"list.json?rxcuis={string.Join<string>("+", request.MedDtos)}").ConfigureAwait(false);//TODO this is where you would need to diverge to get streaming results
                response.EnsureSuccessStatusCode();
 
                
            }
            catch (HttpRequestException ex)
            {
                //TODO: catches error but doesnt display anything 
                var errorDrug = Task.FromResult(ex.Message); // Task<string>.Factory.StartNew(() =>"API Call Failed: " + ex.Message);
                

                return new IGetDrugInteractions.Model() { Meds = errorDrug };
            }
            return new IGetDrugInteractions.Model() { Meds = response.Content.ReadAsStringAsync() };
        }
    }
}
