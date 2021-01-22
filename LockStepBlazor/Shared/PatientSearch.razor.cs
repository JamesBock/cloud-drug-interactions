using Hl7.Fhir.Rest;
using LockStepBlazor.Data;
using LockStepBlazor.Data.Models;
using LockStepBlazor.Data.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPLockStep.Domain.Common;
using UWPLockStep.Domain.Entities.People;

namespace LockStepBlazor.Shared
{
    public partial class PatientSearch
    {
        [Inject]
        protected IPatientDataService PatientService { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        protected LockStepPatient patientdata;
        protected string SearchFirst { get; set; }
        protected string SearchLast { get; set; }
        protected Hl7.Fhir.Model.Bundle Bundle { get; set; }
        protected List<LockStepPatient> Patients { get; set; } = new List<LockStepPatient>();
        public bool CanMoveNext { get; set; }
        public bool CanMovePrevious { get; set; }
        public bool CanMoveFirst { get; set; }
        public bool CanMoveLast { get; set; }
        protected readonly List<MedicationConceptDTO> meds = new List<MedicationConceptDTO>();
        // protected override async Task OnInitializedAsync()
        // {
        //     //var pat = await PatientService.GetPatientAsync("921330");
        //     //patientdata = pat.QueriedPatient;
        // }

        protected async Task SearchForPatients()
        {
            var list = await PatientService.GetPatientListAsync(SearchFirst, SearchLast);
             GetPatientsFromBundle(list.Patients);

        }
        protected async Task NavigateBundleNext()
        {
            var bun = await PatientService.NavigateBundleAsync(Bundle, PageDirection.Next);
             GetPatientsFromBundle(bun.Bundle);
        }
        protected async Task NavigateBundleLast()
        {
            var bun = await PatientService.NavigateBundleAsync(Bundle, PageDirection.Last);
            GetPatientsFromBundle(bun.Bundle);
        }
        protected async Task NavigateBundleFirst()
        {
            var bun = await PatientService.NavigateBundleAsync(Bundle, PageDirection.First);
             GetPatientsFromBundle(bun.Bundle);
        }
        protected async Task NavigateBundlePrevious()
        {
            var bun = await PatientService.NavigateBundleAsync(Bundle, PageDirection.Previous);
             GetPatientsFromBundle(bun.Bundle);
        }

        protected void NavigateToInteractions(string id)
        {
            NavigationManager.NavigateTo($"DrugInteractions/{id}");
        }
        private void GetPatientsFromBundle(Hl7.Fhir.Model.Bundle bundle)
        {
            Bundle = bundle;
          
            CanMoveFirst = Bundle.FirstLink == null;
            CanMovePrevious = Bundle.PreviousLink == null;
            CanMoveNext = Bundle.NextLink == null;
            CanMoveLast = Bundle.LastLink == null;

            if (Patients.Count > 0)
            {
                Patients.Clear();
            }
            foreach (var e in Bundle.Entry)
            {
                var patient = new LockStepPatient();
                Hl7.Fhir.Model.Patient p = (Hl7.Fhir.Model.Patient)e.Resource;
                //var meds = PatientService.GetMedicationRequestsAsync(p.Id).GetAwaiter().GetResult();
                patient.FhirPatient = p;
                // patient.Medications = meds.Requests;
                Patients.Add(patient);
            }
        }

    }
}