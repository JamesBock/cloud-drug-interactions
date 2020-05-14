using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hl7.Fhir.Model;
using Newtonsoft.Json.Linq;
using UWPLockStep.ApplicationLayer.FHIR.Queries;
using System.Net.Http;
using System.Diagnostics;
using System.Threading.Channels;
using Task = System.Threading.Tasks.Task;
using UWPLockStep.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace LockStepBlazor.Handlers
{
    public class GetRxCuiListAPIHandler : GetRxCuiListAPI.IHandler
    {
        private readonly HttpClient _httpClient;

        protected readonly Channel<(Task<HttpResponseMessage>, MedicationConceptDTO)> c = Channel.CreateUnbounded<(Task<HttpResponseMessage>, MedicationConceptDTO)>();

        //private readonly List<MedicationConceptDTO> _medDtos = new List<MedicationConceptDTO>();

        private List<string> _rxCuis = new List<string>();

        public GetRxCuiListAPIHandler(HttpClient httpClient)
        {
            _httpClient = httpClient;

        }
        public async Task<GetRxCuiListAPI.Model> Handle(GetRxCuiListAPI.Query request, CancellationToken cancellationToken)
        {
          /*  var cOut = */FetchRxCuis(request.Requests); //TODO: use the list, then clear it? Why?? Instead of replacing an entry or updating one, we are removing all then replacing one by one
            #region String only RxCui implementation. Will need to Join Drug Interaction data and MedicationRequest info together at a later time on RxCui
            request.Requests = new List<MedicationConceptDTO>();
            #endregion


            //This implementation uses Task<HttpResponseMessage> instead of awaiting the Task in the called method and returning the HttpResponseMessage
            //await foreach (var item in c.Reader.ReadAllAsync())
            //{

            //    if (item.Item1 != null)
            //    {
            //        var t = await item.Item1; //does this await allow for other items in the IAsyncEnumerable to be read?
            //        JObject.Parse(await t.Content
            //               .ReadAsStringAsync())["idGroup"]["rxnormId"].Children().ToList()
            //               .ForEach(x =>
            //               {
            //                   item.Item2.RxCui = x.ToString();
            //                   _rxCuis.Add(x.ToString());

            //               });
            //    }
            //    else
            //    {
            //        _rxCuis.Add(item.Item2.CodeString);
            //        item.Item2.RxCui = item.Item2.CodeString;
            //    }
            //    request.Requests.Add(item.Item2);
            //};
            //return new GetRxCuiList.Model() { MedDtos = _rxCuis };

            while (c.Reader.TryRead(out var item))
            {
                //var item = await c.Reader.ReadAsync();
                if (item.Item1 != null)
                {

                    JObject.Parse(await item.Item1.GetAwaiter().GetResult().Content
                            .ReadAsStringAsync().ConfigureAwait(false))["idGroup"]["rxnormId"].Children().ToList()
                         .ForEach(x =>
                       {
                           item.Item2.RxCui = x.ToString();
                           _rxCuis.Add(x.ToString());
                           //await channel.Writer.WriteAsync(x.ToString());


                           //TODO: does this need its own Channel to send to the individualt API. How is this different than simple Async methods?
                           //Debug.WriteLine(x.ToString() + $"{DateTime.Now:hh:mm:ss}");
                       });

                }
                else
                {
                    _rxCuis.Add(item.Item2.CodeString);
                    item.Item2.RxCui = item.Item2.CodeString;
                }
                request.Requests.Add(item.Item2);
            }
            return new GetRxCuiListAPI.Model() { MedDtos = _rxCuis };
            //channel.Writer.Complete();
        }

        //}
        #region assignment implementation: This allows the Requests list to be returned and keeps the RxCUIs aligned with the MedicationRequest info. Channels may not be needed here.

        //await foreach (var item in cOut.ReadAllAsync()) //this runs through an AsyncEnumerable
        //{
        //    JObject.Parse(await item.RxCuiResponse.Content
        //           .ReadAsStringAsync())["idGroup"]["rxnormId"].Children().ToList()
        //           .ForEach(x =>
        //          {
        //              item.RxCui = x.ToString(); //TODO: does this need its own Channel to send to the individualt API. How is this different than simple Async methods?
        //            Debug.WriteLine(x.ToString() + $"{DateTime.Now:hh:mm:ss}");
        //          });

        //}
        #endregion

        #region IAsyncEnumerable implementation

        //await FetchRxCuis(anonList)
        //        .ForEachAwaitAsync(async r => JObject.Parse(await r.Content
        //        .ReadAsStringAsync())["idGroup"]["rxnormId"].Children().ToList()//RxCUIs are sent as a batch to the DrugInteraction 
        //        .ForEach(x =>
        //        {
        //            _rxCuiList.Add(x.ToString());
        //            Debug.WriteLine(x.ToString() + $"{DateTime.Now:hh:mm:ss}");
        //        }));
        #endregion


        //How could you show these as they came in from the drug intereactin API? YOU CANNOT AS LONG AS THE DRUGINTERACTIONS ARE SENT TOGETHER TO THE INTERACTION API! If you use an internal database that uses single interaction calls you would need to query the list of each drug's interactions for matches to each of the other drugs. There is an api to return a list of a drug's interactions as RxNorm codes. Could cross reference that list and the returned RxCUI list made in this Handler but unclear if that would be as extensive a search as the interactions list api. Does the list api include drug interactions of the drugs with the same ingredient as the drug in question?

           /// <summary>
           /// this didnt need to be async or return a Task because the caller can handle the Task from the channel.
           /// </summary>
           /// <param name="anons"></param>
         void FetchRxCuis(List<MedicationConceptDTO> anons) //Switching this to List from IAsyncEnumerable sped up performance significantly. Ave around 3.5 sec  first hit and upper 900ms in subsequent pings, vs 5 -6 sec and 2+ subsequent pings
        {

            foreach (var anon in anons) //Removed IAsyncEnumerable and switched this to a foreach from an await foreach. The awaiter creates a new state in the async state machine.  
            //Task.Run(async () =>
            //{
            {

                switch (anon.Sys)
                {
                    case "http://hl7.org/fhir/sid/ndc":
                         c.Writer.WriteAsync((_httpClient.GetAsync($"?idtype=NDC&id={anon.CodeString}"), anon));
                        break;

                    case "http://snomed.info/sct":
                         c.Writer.WriteAsync((_httpClient.GetAsync($"?idtype=SNOMEDCT&id={anon.CodeString}"), anon));
                        break;

                    case "http://www.nlm.nih.gov/research/umls/rxnorm":
                         c.Writer.WriteAsync((null, anon));
                        break;

                    default: //TODO: what should happen if the code is not from any of these systems?
                        break;
                }
            }
            //c.Writer.Complete();

            ////}
            //);
            //return c.Reader;

        }
        //TODO: Does this need its own handler? This is one place a stream might be noticable since each RxCUI needs its own API call
        //This does not have concurrency so items are returned one by one...but once an item is returned, it can be used by calling code, and the next API call can execute?
        //private async IAsyncEnumerable<HttpResponseMessage> FetchRxCuis(List<StringConcept> anons)
        //{
        //    foreach (var anon in anons)
        //    {

        //        switch (anon.Sys)
        //        {
        //            case "http://hl7.org/fhir/sid/ndc":
        //                yield return await _httpClient.GetAsync($"?idtype=NDC&id={anon.CodeString}");
        //                break;

        //            case "http://snomed.info/sct":
        //                yield return await _httpClient.GetAsync($"?idtype=SNOMEDCT&id={anon.CodeString}");
        //                break;

        //            case "http://www.nlm.nih.gov/research/umls/rxnorm":
        //                _rxCuiList.Add(anon.CodeString);
        //                break;

        //            default: //TODO: what should happen if the code is not from any of these systems?
        //                break;
        //        }
        //    }
        //}
        #region Channel for RxCUI strings to send to individual drug api
        //the channel pipes the responses from the API to the NewtonSoft Parser...as they come in?

        //No matter which method is used, the RxCuis are sent to and returned from the Drug interaction API without any other context and will need to be joined to the MedicationRequest info either way. The RxCuis should be added to the MedicatoinDTO and the GetRxCuiList.Model can be a List of strings only.
        //The above is a hybrid of the two methods. can remove HttpResponse from DTO




        #endregion

        #region assigning to DTO HTTP
        //ChannelReader<MedicationConceptDTO> FetchRxCuis(IList<MedicationConceptDTO> anons)
        //{
        //    var c = Channel.CreateUnbounded<MedicationConceptDTO>();

        //    Task.Run(async () =>
        //    {
        //        foreach (var anon in anons)
        //        {

        //            switch (anon.Sys)
        //            {
        //                case "http://hl7.org/fhir/sid/ndc":
        //                    anon.RxCuiResponse = await _httpClient.GetAsync($"?idtype=NDC&id={anon.CodeString}");
        //                    await c.Writer.WriteAsync(anon);
        //                    break;

        //                case "http://snomed.info/sct":
        //                    anon.RxCuiResponse = await _httpClient.GetAsync($"?idtype=SNOMEDCT&id={anon.CodeString}");
        //                    await c.Writer.WriteAsync(anon);
        //                    break;

        //                case "http://www.nlm.nih.gov/research/umls/rxnorm":
        //                    anon.RxCui = anon.CodeString;
        //                    break;

        //                default: //TODO: what should happen if the code is not from any of these systems?
        //                    break;
        //            }
        //        }
        //        c.Writer.Complete();

        //    }
        //    );
        //    return c.Reader;

        //}
        #endregion


    }
}
