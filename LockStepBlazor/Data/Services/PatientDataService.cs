using Hl7.Fhir.Model;
using LockStepBlazor.Data.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using UWPLockStep.ApplicationLayer.DrugInteractions;
using UWPLockStep.ApplicationLayer.FHIR.Queries;
using UWPLockStep.ApplicationLayer.Patients.Queries;
using UWPLockStep.Domain.Common;
using UWPLockStep.ApplicationLayer.MediatrExtensions;
using UWPLockStep.ApplicationLayer.Interfaces.MediatR;
using Hl7.Fhir.Rest;

namespace LockStepBlazor.Data.Services
{
    public class PatientDataService : IPatientDataService
    {
        private readonly ILogger<PatientDataService> logger;
        private readonly IMediator mediator;
       

        public PatientDataService(ILogger<PatientDataService> logger, IMediator mediator, IGetFhirMedications getMedications)
        {
            this.logger = logger;
            this.mediator = mediator;
         
        }

        public async Task<IGetFhirMedications.Model> GetMedicationRequestsAsync(string id)
        {
            
            return await mediator.Send(new IGetFhirMedications.Query()
            {
                PatientId = id
            }).ConfigureAwait(false);

        }

        public async Task<GetRxCuiListAPI.Model> GetRxCuisAsync(List<MedicationConceptDTO> requests)
        {
            return await mediator.Send(new GetRxCuiListAPI.Query()
            { 
                Requests = requests
            }).ConfigureAwait(false);

        }

        /// <summary>
        /// Using an extension method in the body of this method to delegate the creation of the Query to the extension method. Really unsure if this is beneficial
        /// </summary>
        /// <param name="medDtos"></param>
        /// <returns></returns>
        public async Task<IGetDrugInteractions.Model> GetDrugInteractionListAsync(List<string> medDtos)
        {
            return await mediator.GetDrugInteractions(medDtos).ConfigureAwait(false);
        }

        //public async IAsyncEnumerable<DrugInteraction> GetDrugInteractionList(IEnumerable<string> drugs)
        //{
        //    var apiBase = new Uri("https://rxnav.nlm.nih.gov/REST/interaction/");
        //    var restClient = new HttpClient() { BaseAddress = apiBase };

        //}

        public async Task<GetPatient.Model> GetPatientAsync(string id)
        {
            return await mediator.Send(new GetPatient.Query()
            {
                PatientId = id
            }).ConfigureAwait(false);

        }
        public async Task<GetPatientList.Model> GetPatientListAsync(string firstName, string lastName)
        {
            return await mediator.Send(new GetPatientList.Query()
            {
                FirstName = firstName,
                LastName = lastName
            }).ConfigureAwait(false);
        }
        public async Task<NavigateBundle.Model> NavigateBundleAsync(Bundle bundle, PageDirection nav)
        {
            return await mediator.Send(new NavigateBundle.Query()
            {
                Bundle = bundle,
                Nav = nav
            }).ConfigureAwait(false);
        }

    }
}
