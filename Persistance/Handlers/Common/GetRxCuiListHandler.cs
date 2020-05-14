using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UWPLockStep.ApplicationLayer.Interfaces;
using UWPLockStep.ApplicationLayer.Policy.Queries;

using UWPLockStep.Persistance.Services;

using MediatR;
using UWPLockStep.ApplicationLayer.Patients.Queries;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Model;
using UWPLockStep.Domain.Common.State.PatientType;
using UWPLockStep.ApplicationLayer.Orders.Queries;
using UWPLockStep.Domain.Entities.Intermediates;
using Newtonsoft.Json.Linq;
using UWPLockStep.ApplicationLayer.FHIR.Queries;
using Microsoft.EntityFrameworkCore.Query;
using UWPLockStep.Persistance.Helpers;
using System.Net.Http;

namespace LockStepBlazor.Data
{
    public class GetRxCuiListHandler : GetRxCuiList.IHandler
    {
        public GetRxCuiListHandler(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }
        private readonly HttpClient _httpClient;
        private readonly List<string> RxCuiList = new List<string>();

        public async Task<GetRxCuiList.Model> Handle(GetRxCuiList.Query request, CancellationToken cancellationToken)
        {
            var anonList = new List<StringConcept>();

            foreach (var item in request.Requests)
            {
                try //need a try statement incase there in no CodeableConcept
                {

                    var code = item.Medication as CodeableConcept;
                    anonList = code.Coding.Select(s =>
                      new StringConcept() { Sys = s.System.ToLower(), CodeString = s.Code, Text = code.Text }).ToList();
                }
                catch (NullReferenceException)
                {
                    break;
                }
                var responseList = FetchRxCuis(anonList);
                //HACK: Is it OK to use Result here?

                await responseList.ForEachAwaitAsync(async r => JObject.Parse(await r.Content.ReadAsStringAsync())["idGroup"]["rxnormId"].Children().ToList().ForEach(x => RxCuiList.Add(x.ToString()))
        );


            }
                return new GetRxCuiList.Model() { RxCuis = RxCuiList };
        }
        //TODO: Doesthis need its own handler?
        private async IAsyncEnumerable<HttpResponseMessage> FetchRxCuis(List<StringConcept> anons)
        {
            foreach (var anon in anons)
            {
            
               switch (anon.Sys)
                {
                    case "http://hl7.org/fhir/sid/ndc":
                        yield return await _httpClient.GetAsync($"?idtype=NDC&id={anon.CodeString}");
                        
                        break;
                    case "http://snomed.info/sct":
                        yield return await (_httpClient.GetAsync($"?idtype=SNOMEDCT&id={anon.CodeString}"));
                        break;
                    case "http://www.nlm.nih.gov/research/umls/rxnorm":
                        RxCuiList.Add(anon.CodeString);
                        break;
                    default:
                        break;
                }
            }
        }


    }
}
