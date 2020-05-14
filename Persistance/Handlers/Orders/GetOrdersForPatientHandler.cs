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
using System.Net.Http;

namespace UWPLockStep.Persistance.Handlers.Orders
{
    public class GetOrdersForPatientHandler : GetOrdersForPatient.IHandler
    {
        private readonly IFhirClient _client = new FhirClient("http://hapi.fhir.org/baseR4") { PreferredFormat = ResourceFormat.Json };

        public async Task<GetOrdersForPatient.Model> Handle(GetOrdersForPatient.Query request, CancellationToken cancellationToken)
        {
            var q = new SearchParams()
                             .Where($"patient={request.PatientId}")
                             .Include("MedicationRequest:medication").LimitTo(1000);//How do you handle these?
            var result = await _client.SearchAsync<MedicationRequest>(q);

            var rxcuis = new List<string>();//this is to be passed to the drug-drug interactions API method

            var medsReqs = new List<MedicationRequest>();
            var meds = new List<Medication>();
            foreach (var item in result.Entry)
            {
                try
                {
                    medsReqs.Add(item.Resource as MedicationRequest);
                    meds.Add(item.Resource as Medication);//This would work if there were Medications on these requests?

                }
                catch (Exception)
                {

                    throw;
                }
            }
            var restClient = new HttpClient() { BaseAddress = new Uri("https://rxnav.nlm.nih.gov/REST/rxcui.json" )};


            //var nanoanno = medsReqs
            //            .Select(m=>(CodeableConcept)m.Medication)
            //            .SelectMany(c=>c.Coding
            //            .Select(s =>

            //      new { Sys = s.System.ToLower(), CodeString = s.Code, Text = c.Text }));
            var responseList = new List<HttpResponseMessage>();
            foreach (var item in medsReqs)
            {
                try //need a try statement incase there in no CodeableConcept
                {

                    var code = item.Medication as CodeableConcept;
                    var anonList = code.Coding.Select(s =>
                      new { Sys = s.System.ToLower(), CodeString = s.Code, Text = code.Text }).ToList();

                    foreach (var anon in anonList)
                    {
                        //TODO: Make this Async?
                        switch (anon.Sys)
                        {
                            case "http://hl7.org/fhir/sid/ndc":
                                 responseList.Add(await restClient.GetAsync($"?idtype=NDC&id={anon.CodeString}"));
                                break;
                            case "http://snomed.info/sct":
                                responseList.Add(await restClient.GetAsync($"?idtype=SNOMEDCT&id={anon.CodeString}"));
                                break;
                            case "http://www.nlm.nih.gov/research/umls/rxnorm":
                                rxcuis.Add(anon.CodeString);
                                break;
                            default:
                                break;
                        }
                    }
                }
                catch (NullReferenceException)
                {


                }
            }

            responseList.ForEach(async r => JObject.Parse(await r.Content.ReadAsStringAsync())["idGroup"]["rxnormId"].Children().ToList().ForEach(x => rxcuis.Add(x.ToString()))
            );
            var orders = new List<OrderItem>();//TODO: THIS NEEDS MAPPING, this class not in use. Use broken out versions
            var model = new GetOrdersForPatient.Model();
            model.PatientOrders.AddRange(orders);
            return model;
        }
    }
}
