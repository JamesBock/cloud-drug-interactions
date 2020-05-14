using LockStepBlazor.Data;
using LockStepBlazor.Data.Models;
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
        protected LockStepPatient patientdata;

        protected override async Task OnInitializedAsync()
        {
            var pat = await PatientService.GetPatientAsync("921330");
            patientdata = pat.QueriedPatient;
        }
    }
}