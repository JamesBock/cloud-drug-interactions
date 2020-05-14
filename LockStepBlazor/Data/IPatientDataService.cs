using Hl7.Fhir.Model;
using LockStepBlazor.Data.Models;
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
        Task<GetPatient.Model> GetPatientAsync(string id);
        //Task<PatientData> GetPatientDataAsync(string id);
        Task<GetRxCuiListAPI.Model> GetRxCuisAsync(List<MedicationConceptDTO> requests);
    }
}