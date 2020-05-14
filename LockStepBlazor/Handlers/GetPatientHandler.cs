using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UWPLockStep.ApplicationLayer.Patients.Queries;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Model;

namespace LockStepBlazor.Handlers
{
    public class GetPatientHandler : GetPatient.IHandler
    {
        private readonly IFhirClient _client;

        public GetPatientHandler(IFhirClient client)
        {
            _client = client;
            
        }

        public async Task<GetPatient.Model> Handle(GetPatient.Query request, CancellationToken cancellationToken)
        {
            #region how nested Bundle was handled
            //var transaction = new TransactionBuilder(_client.Endpoint);

            //var patientById = new SearchParams()
            // .Where($"_id={request.PatientId}");

            //transaction.Search(patientById, "Patient");// went away from the transaction builder because it returned nested Bundles (which were handled successfullly, but no need to add complexity) and added an additional search to the server. Unclear if the Include method is also a serach but I should be much more effiecient. Also, not all servers support TransactionsBuilder.
            //transaction.Search(conception, "Observation");
            //var qResult = await _client.TransactionAsync(transaction.ToBundle());

            //var patBundle = (Bundle)qResult.Entry.First().Resource;
            //var pat = (Patient)patBundle.Entry.First().Resource;

            //var concpetionBundle = (Bundle)qResult.Entry[1].Resource;
            #endregion

            var conception = new SearchParams()
                            .Where($"subject={request.PatientId}")
                            .Where("code=33067-0")//Fixed to ConceptionDate LOINC
                            .LimitTo(1)//Incase there is more than one, though there shouldnt be, but I added two to the FHIR server to see what happens 
                            .Include("Observation:subject");//TODO: this will get all Observation infor from the server on Patient?

            var qResult = await _client.SearchAsync<Observation>(conception);
            var conceptionObservation = (Observation)qResult.Entry[0].Resource;
            var conceptionFhirDateTime = (FhirDateTime)conceptionObservation.Value;
            var pat = (Patient)qResult.Entry[1].Resource;

            //Create map from FHIR Patient to your Patient.
            //Decided to see what it would be like if my Patient type inheirited FHIRs. 
            var modelPatient = new UWPLockStep.Domain.Entities.People.LockStepPatient()
            {
                FhirPatient = pat,
                LastName = pat.Name[0].Family,
            };
            modelPatient.GivenNames.AddRange(pat.Name.SelectMany(n => n.GivenElement.Select(nm => nm.Value)));
            modelPatient.SetBirthStatus(pat.BirthDateElement.ToDateTimeOffset().GetValueOrDefault(new DateTimeOffset(DateTime.MinValue)),
                conceptionFhirDateTime.ToDateTimeOffset());//TODO: this is depricated, learn how to use DateTimeOffset properly
            return new GetPatient.Model() { QueriedPatient = modelPatient };
        }

    }
}
