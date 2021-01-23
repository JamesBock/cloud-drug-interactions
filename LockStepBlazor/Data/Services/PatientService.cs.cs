using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using LockStepBlazor.Shared;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LockStepBlazor.Data.Services
{
    public class PatientService
    {
        private readonly FhirClient client;

        public PatientService(FhirClient client)
        {
            this.client = client;

        }

        public List<Patient> PatientList { get; set; }

        public async virtual Task<List<Patient>> Search(string term = null)
        {
            if (!string.IsNullOrEmpty(term))
            {
                term = term.ToLower();
                term = term.Trim();

            // var trans = new TransactionBuilder(client.Endpoint);
            // trans = trans.Search(q, "MedicationRequest").Search(t, "MedicationStatement");
            // //var result = await _client.SearchAsync<MedicationRequest>(q);
            // var tResult = _client.TransactionAsync(trans.ToBundle());

                // var result =
                //     PatientList
                //     .Where(x =>
                //         x.GivenNames.Select(n=> n.ToLower()).Contains(term) ||
                //         x.LastName.ToLower().Contains(term)
                //     )
                //     .ToList();

                var queryNames = new SearchParams()
                        .Where($"given={term}")
                        .Where($"last={term}")
                        .LimitTo(50);
                var searchCall = await client.SearchAsync<Patient>(queryNames);


                List<Patient> patients = new List<Patient>();
                while (searchCall != null)
                {
                    foreach (var e in searchCall.Entry)
                    {
                        Patient p = (Patient)e.Resource;
                        patients.Add(p);
                    }
                    searchCall = client.Continue(searchCall, PageDirection.Next);
                }
                return patients;
            }

            return PatientList;
        }
    }
}