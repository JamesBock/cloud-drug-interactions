using LockStepBlazor.Data;
using LockStepBlazor.Data.Models;
using LockStepBlazor.Data.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPLockStep.Domain.Entities.People;

namespace LockStepBlazor.Shared
{
    public partial class MainLayout : LayoutComponentBase
    {
        [Inject]
        protected IPatientDataService PatientService { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        protected LockStepPatient patientdata;
        protected string SearchFirst { get; set; }
        protected string SearchLast { get; set; }
        protected List<Hl7.Fhir.Model.Patient> Patients = new List<Hl7.Fhir.Model.Patient>();

        // protected override async Task OnInitializedAsync()
        // {
        //     //var pat = await PatientService.GetPatientAsync("921330");
        //     //patientdata = pat.QueriedPatient;
        // }

     
    }
}