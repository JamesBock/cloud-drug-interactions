using Hl7.Fhir.Model;
using LockStepBlazor.Application.Fhir.Queries;
using LockStepBlazor.Data.Models;
using Microsoft.AspNetCore.Http.Connections;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace LockStepBlazor.Handlers
{
    /// <summary>
    /// This Handler does ont output the correct Dtos. This MedicationStatements and Requests are not in the same order everytime (nor should they need to be in the cause of live API) as long as the DTO and the RxCUIs are matched
    /// </summary>
    public class GetRxCuiListMockHandler : GetRxCuiListAPI.IHandler
    {
        private List<string> _rxCuis = new List<string>();
        protected readonly Channel<(Task<HttpResponseMessage>, MedicationConceptDTO)> c = Channel.CreateUnbounded<(Task<HttpResponseMessage>, MedicationConceptDTO)>();
        
        string[] array = new string[] { //anons and rxcuis are not matched w/o the context from the codable concept
                "310964",
                "1113397",
                "1247756",
                "313992",
                "104895",
                "330765",
                "608930",
                "141962",
                "1161682",
                "800405",
                "608680"};

        async public Task<GetRxCuiListAPI.Model> Handle(GetRxCuiListAPI.Query request, CancellationToken cancellationToken)
        {
            await FetchRxCuis(request.Requests);
            request.Requests = new List<MedicationConceptDTO>();
            int o = 0;
            while (c.Reader.TryRead(out var item))
            {
                //var item = await c.Reader.ReadAsync();

                item.Item1.GetAwaiter().GetResult();

                item.Item2.RxCui = array[o];
                _rxCuis.Add(array[o]);

                o++;

                request.Requests.Add(item.Item2);
            }
            return new GetRxCuiListAPI.Model() { MedDtos = _rxCuis };
        }
        async Task FetchRxCuis(List<MedicationConceptDTO> anons) //Switching this to List from IAsyncEnumerable sped up performance significantly. Ave around 3.5 sec on first hit and upper 900ms in subsequent pings, vs 5 -6 sec and 2+ subsequent pings
        {
        int i = 0;


            foreach (var anon in anons) //Removed IAsyncEnumerable and switched this to a foreach from an await foreach. The awaiter creates a new state in the async state machine.  
            {
                await c.Writer.WriteAsync(
                     (Task.Run(() => { return new HttpResponseMessage(System.Net.HttpStatusCode.OK) /*{ Content = new StringContent(array[i], Encoding.UTF8) }*/; }), anon)
                 );

                i++;
            }
        }
    }
}