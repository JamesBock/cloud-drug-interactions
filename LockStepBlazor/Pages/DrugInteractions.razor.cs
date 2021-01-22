using LockStepBlazor.Data;
using LockStepBlazor.Data.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UWPLockStep.Domain.Common;
using System.Diagnostics;

namespace LockStepBlazor.Pages

{
    public partial class DrugInteractions : ComponentBase, IObserver<MedicationInteractionPair>
    {
        [Inject]
        protected IDrugInteractionParserAsync DrugInteractionParser { get; set; }
        [Inject]
        protected IPatientDataService PatientService { get; set; }

        protected List<MedicationConceptDTO> MedicationConcepts = new List<MedicationConceptDTO>();

        protected List<MedicationInteractionPair> Interactions = new List<MedicationInteractionPair>();

        protected Dictionary<Guid, bool> CollapseDrugInteraction = new Dictionary<Guid, bool>();

        [Parameter]
        public string PatientId { get; set; }

        /// <summary>
        /// When render type is set to ServerPrerendered, this whole method will load before the screen is loaded so all of the interaction will be shown at once. Then the app will rerender and "stream" the interactions. When in server mode the interactions are stream right away. Streaming effect is caused by the DrugInteractionParser 
        /// </summary>
        /// <returns></returns>
        //protected override async Task OnInitializedAsync()
        //{
        //    var stopwatch = new Stopwatch();
        //    stopwatch.Start();

        //    #region Live APIs

        //    var requestResult = await PatientService.GetMedicationRequestsAsync("921330");
        //    medicationConcepts = requestResult.Requests;
        //    this.StateHasChanged();
        //    var rxcuisResult = await PatientService.GetRxCuisAsync(requestResult.Requests);
        //    var drugResult = await PatientService.GetDrugInteractionListAsync(rxcuisResult.MedDtos);

        //        await foreach (var drug in DrugInteractionParser.ParseDrugInteractionsAsync(drugResult.Meds))
        //    {
        //        collapseDrugInteraction.Add(drug.InteractionId, true);

        //        requestResult.Requests.Where(r => r.RxCui == drug.MedicationPair.Item1.RxCui)
        //                                .Select(z => (drug.MedicationPair.Item1.DisplayName = z.Text,
        //                               drug.MedicationPair.Item1.TimeOrdered = z.TimeOrdered,
        //                               drug.MedicationPair.Item1.Prescriber = z.Prescriber,
        //                               drug.MedicationPair.Item1.FhirType = z.FhirType,
        //                               drug.MedicationPair.Item1.ResourceId = z.ResourceId)).ToList();
        //        requestResult.Requests.Where(r => r.RxCui == drug.MedicationPair.Item2.RxCui)
        //                                .Select(z => (drug.MedicationPair.Item2.DisplayName = z.Text,
        //                               drug.MedicationPair.Item2.TimeOrdered = z.TimeOrdered,
        //                               drug.MedicationPair.Item2.Prescriber = z.Prescriber,
        //                               drug.MedicationPair.Item2.FhirType = z.FhirType,
        //                               drug.MedicationPair.Item2.ResourceId = z.ResourceId)).ToList();
        //        interactions.Add(drug);
        //        this.StateHasChanged();
        //    };
        //    #endregion
        protected async override void OnParametersSet()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            //DrugInteractionParser.Subscribe(this);
            #region Live APIs

            var requestResult = await PatientService.GetMedicationRequestsAsync(PatientId)
                .ContinueWith(c => MedicationConcepts = c.Result.Requests);
            if (requestResult.Count > 0)
            {
                var rxcuisResult = await PatientService.GetRxCuisAsync((requestResult));
                StateHasChanged();
                //medicationConcepts = requestResult.Result.Requests;
                var drugResult = await PatientService.GetDrugInteractionListAsync((rxcuisResult).MedDtos);
                //DrugInteractionParser.ParseDrugInteractions(await drugResult.Meds);
                await foreach (var drug in DrugInteractionParser.ParseDrugInteractionsAsync(await drugResult.Meds))
                {
                    CollapseDrugInteraction.Add(drug.InteractionId, true);

                    MedicationConcepts.Where(r => r.RxCui == drug.MedicationPair.Item1.RxCui)
                                            .Select(z => (drug.MedicationPair.Item1.DisplayName = z.Text,
                                           drug.MedicationPair.Item1.TimeOrdered = z.TimeOrdered,
                                           drug.MedicationPair.Item1.Prescriber = z.Prescriber,
                                           drug.MedicationPair.Item1.FhirType = z.FhirType,
                                           drug.MedicationPair.Item1.ResourceId = z.ResourceId)).ToList();
                    MedicationConcepts.Where(r => r.RxCui == drug.MedicationPair.Item2.RxCui)
                                            .Select(z => (drug.MedicationPair.Item2.DisplayName = z.Text,
                                           drug.MedicationPair.Item2.TimeOrdered = z.TimeOrdered,
                                           drug.MedicationPair.Item2.Prescriber = z.Prescriber,
                                           drug.MedicationPair.Item2.FhirType = z.FhirType,
                                           drug.MedicationPair.Item2.ResourceId = z.ResourceId)).ToList();
                    Interactions.Add(drug);
                    hideInteractions = false;
                    StateHasChanged();
                }

            };
            #endregion

            Debug.WriteLine($"Drug interations loaded by {stopwatch.ElapsedMilliseconds}");
         
            #region Old code using joins to unify interaction data and Medication data

            // var piper1 = interactions.Join(requestResult.Requests,
            //                              a => a.MedicationPair.Item1.RxCui,
            //                              mdto => mdto.RxCui,
            //                             (a, mdto) => new MedicationInteractionPair.MedicationInteractionViewModel()
            //                             {
            //                                 InteractionId = a.InteractionId,
            //                                 RxCui = mdto.RxCui,
            //                                 TimeOrdered = mdto.TimeOrdered,
            //                                 Prescriber = mdto.Prescriber,
            //                                 FhirType = mdto.FhirType,
            //                                 ResourceId = mdto.ResourceId,
            //                                 DisplayName = mdto.Text
            //                             }
            //      );
            // var piper2 = interactions.Join(requestResult.Requests,
            //                             a => a.MedicationPair.Item2.RxCui,
            //                             mdto => mdto.RxCui,
            //                            (a, mdto) => new MedicationInteractionPair.MedicationInteractionViewModel()
            //                            {
            //                                InteractionId = a.InteractionId,
            //                                RxCui = mdto.RxCui,
            //                                TimeOrdered = mdto.TimeOrdered,
            //                                Prescriber = mdto.Prescriber,
            //                                FhirType = mdto.FhirType,
            //                                ResourceId = mdto.ResourceId,
            //                                DisplayName = mdto.Text//This means you are using the text from the MedicationRequest or MedicationReport, not the RxCui...showing the name as it is shown in the prescription is prefered i beleieve
            //                            }
            //     );

            // var zippy = piper1.Join(piper2,
            //                 one => one.InteractionId,
            //                 two => two.InteractionId,
            //                 (one, two) => (one, two  )


            //     );


            // //var test = interactions.Join(zippy,
            // //                            interaction => interaction.InteractionId,
            // //                            zip => zip.Item1.InteractionId,
            // //                            (p, q) => p
            // //    );

            ////this seems to work completelty at random...I think i needed the .ToList and the SelectMany to get it to trigger for them all...You nee to figure out how LINQ actually works
            //var tes = zippy.SelectMany(z => interactions
            //                   .Where(i => i.InteractionId == z.one.InteractionId).Select(s => (s.MedicationPair = z, s.InteractionId = z.one.InteractionId) )).ToList()
            //                   ;

            //var query = from interaction in interactionsb
            //           join vm in zippy on interaction equals vm.one.InteractionId into gj
            //           select interaction  { OwnerName = gj. Pets = gj };


            //interactions.Join(requestResult.Requests,
            //                            a => a.MedicationPair.Item2.RxCui,
            //                            mdto => mdto.RxCui,
            //                           (a, mdto) => a.InteractionId = a.InteractionId,
            //                               a.MedicationPair.Item2.TimeOrdered = mdto.TimeOrdered,
            //                               a.MedicationPair.Item2.Prescriber = mdto.Prescriber,
            //                               a.MedicationPair.Item2.FhirType = mdto.FhirType,
            //                               a.MedicationPair.Item2.ResourceId = mdto.ResourceId
            //    ));

            #endregion
            #region Uses ContinueWith

            //var stopwatch = new Stopwatch();
            //stopwatch.Start();

            //var requestResult = PatientService.GetMedicationRequests("921330");

            //drugs = await requestResult
            //    .ContinueWith(a => PatientService.GetRxCuis(a.Result.Requests)
            //    .ContinueWith(a => PatientService.GetDrugInteractionList(a.Result.RxCuis)
            //    .ContinueWith(a => a.Result.Meds).Result).Result);

            //Debug.WriteLine($"Drug interations loaded by {stopwatch.ElapsedMilliseconds}");
            #endregion
            #region Uses Task.WhenAll . These are about the same speed or this one is slightly slower... But why? The GetPatientDataAsync method isn't blocking the rest of the code? 

            //var waitingPatient = PatientService.GetPatientDataAsync("921330");
            //var requestResult = PatientService.GetMedicationRequests("921330");
            //await Task.WhenAll(waitingPatient, requestResult);


            //var rxcuisResult = await PatientService.GetRxCuis(requestResult.Result.Requests);
            //var drugResult = await PatientService.GetDrugInteractionList(rxcuisResult.RxCuis);


            //patientdata = waitingPatient.Result;
            //drugs = drugResult.Meds;
            //Debug.WriteLine($"Drug interations loaded by {stopwatch.ElapsedMilliseconds}");
            #endregion
        }

        public void OnCompleted()
        {

        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(MedicationInteractionPair value)
        {
            CollapseDrugInteraction.Add(value.InteractionId, true);

            MedicationConcepts.Where(r => r.RxCui == value.MedicationPair.Item1.RxCui)
                                    .Select(z => (value.MedicationPair.Item1.DisplayName = z.Text,
                                   value.MedicationPair.Item1.TimeOrdered = z.TimeOrdered,
                                   value.MedicationPair.Item1.Prescriber = z.Prescriber,
                                   value.MedicationPair.Item1.FhirType = z.FhirType,
                                   value.MedicationPair.Item1.ResourceId = z.ResourceId)).ToList();
            MedicationConcepts.Where(r => r.RxCui == value.MedicationPair.Item2.RxCui)
                                    .Select(z => (value.MedicationPair.Item2.DisplayName = z.Text,
                                   value.MedicationPair.Item2.TimeOrdered = z.TimeOrdered,
                                   value.MedicationPair.Item2.Prescriber = z.Prescriber,
                                   value.MedicationPair.Item2.FhirType = z.FhirType,
                                   value.MedicationPair.Item2.ResourceId = z.ResourceId)).ToList();
            Interactions.Add(value);
            InvokeAsync(() => StateHasChanged());
        }
    }
}
