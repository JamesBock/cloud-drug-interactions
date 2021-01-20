using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using LockStepBlazor.Data.Models;
using LockStepBlazor.Data.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using UWPLockStep.ApplicationLayer.DrugInteractions;
using UWPLockStep.ApplicationLayer.FHIR.Queries;
using UWPLockStep.ApplicationLayer.Interfaces.MediatR;
using UWPLockStep.ApplicationLayer.Patients.Queries;
using UWPLockStep.Domain.Common;

namespace LockStepBlazor.Data
{
    public interface IPatientDataService
    {
        Task<IGetDrugInteractions.Model> GetDrugInteractionListAsync(List<string> rxcuis);
        Task<IGetFhirMedications.Model> GetMedicationRequestsAsync(string id);
        Task<GetPatientList.Model> GetPatientListAsync(string firstName = null, string lastName = null);
        Task<GetPatient.Model> GetPatientAsync(string id);
        Task<NavigateBundle.Model> NavigateBundleAsync(Bundle bundle, PageDirection nav);
        Task<GetRxCuiListAPI.Model> GetRxCuisAsync(List<MedicationConceptDTO> requests);

    }
}