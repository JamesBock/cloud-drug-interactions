using Hl7.Fhir.ElementModel;
using Hl7.Fhir.FhirPath;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using UWPLockStep.ApplicationLayer.Interfaces.MediatR;
using UWPLockStep.Domain.Common;
using Task = System.Threading.Tasks.Task;

namespace LockStepBlazor.Handlers
{
    public abstract class GetFhirMedicationsHandler : IGetFhirMedications.IHandler
    {
        protected readonly IFhirClient _client;

        protected readonly Channel<MedicationConceptDTO> channel = Channel.CreateUnbounded<MedicationConceptDTO>();

        public GetFhirMedicationsHandler(IFhirClient client)
        {
            _client = client;
        }

        public abstract Task<IGetFhirMedications.Model> Handle(IGetFhirMedications.Query request, CancellationToken cancellationToken);

        protected Task ParseMedicationsAsync(Task<Bundle> result)
        {
            return result.ContinueWith(bund =>
            {
                Task.Run(() => //This is not necessary for such a small dataset but if this was very large it could help because this is a CPU bound process Task.Run is appropriate.
                {
                    MedsToChannel(bund.GetAwaiter().GetResult().Entry
                                        .Select(e => e.Resource as Bundle)
                                        .SelectMany(b => b.Entry
                                        .Select(e => e.Resource))
                                        .ToList());

                });

            });
        }

        #region Code from seperating Resources at start

        //var bund = await result;

        //  var t1 = Task.Run(() => //This is not necessary for such a small dataset but if this was very large it could help because this is a CPU bound process Task.Run is appropriate.
        //   {
        //       var tempReqs = bund.Entry[0].Resource as Bundle;
        //       return tempReqs.Entry.Select(m => m.Resource as MedicationRequest).ToList(); //Could this be Parallel or ConcurentCollection to help performance
        //   }).ContinueWith(cw => DtoReturnReqs(cw.GetAwaiter().GetResult()));


        //   var t2 = Task.Run(() => //if these arent awaited...the Task will not complete be for the overriden method in the inherited class tries to read the channel and nothing is there
        //   {
        //       var tempState = bund.Entry[1].Resource as Bundle;
        //       return tempState.Entry.Select(m => m.Resource as MedicationStatement).ToList();
        //   }).ContinueWith(cw => DtoReturnStates(cw.GetAwaiter().GetResult()));

        //Task.WhenAll(t1, t2).ConfigureAwait(false);

        //{
        //    switch (m.Resource.ResourceType)
        //    {
        //        case ResourceType.Medication:
        //           medList.Add(m.Resource.ToTypedElement() as Medication);
        //            break;
        //        case ResourceType.MedicationRequest:
        //           medReqList.Add(m.Resource as MedicationRequest);//a MedicationRequest can still have a contained Medication or a reference to another Medication resource
        //            break;

        //    }


        //    Task.Run(() => //if these arent awaited...the Task will not complete be for the overriden method in the inherited class tries to read the channel and nothing is there
        //    {
        //        var tempState = tempBund.Entry[1].Resource as Bundle;
        //        return tempState.Entry.Select(m => m.Resource as MedicationStatement).ToList();
        //    }).ContinueWith(cw => DtoReturnStates(cw.GetAwaiter().GetResult())).ConfigureAwait(false);
        //});

        //var t1 = Task.Run(() => //This is not necessary for such a small dataset but if this was very large it could help because this is a CPU bound process Task.Run is appropriate.
        //{
        //    var tempReqs = bund.Entry[0].Resource as Bundle;
        //    return tempReqs.Entry.Select(m => m.Resource as MedicationRequest).ToList(); //Could this be Parallel or ConcurentCollection to help performance
        //}).ContinueWith(cw => DtoReturnReqs(cw.GetAwaiter().GetResult()));


        //var t2 = Task.Run(() => //if these arent awaited...the Task will not complete be for the overriden method in the inherited class tries to read the channel and nothing is there
        //{
        //    var tempState = bund.Entry[1].Resource as Bundle;
        //    return tempState.Entry.Select(m => m.Resource as MedicationStatement).ToList();
        //}).ContinueWith(cw => DtoReturnStates(cw.GetAwaiter().GetResult()));

        //  Task.WhenAll(t1, t2); //this doesn't allow the method to continue until thus result is in

        //.ContinueWith(cw => channel.Writer.Complete());//When this is not awaited, the channel closes and an exception is thrown

        //channel.Writer.Complete();//App would not proceed w/o this. 
        #endregion


        void MedsToChannel(List<Resource> resources)
        {
            foreach (var item in resources)
            {
                switch (item.ResourceType)
                {
                    case ResourceType.Medication://will never have a contained medication
                        var med = item as Medication;
                        //var code = med.Contained.Select(x => x.ResourceType == ResourceType.Medication ? x as Medication : null);
                        channel.Writer.WriteAsync(med.Code.Coding.Select(s =>
                                                  new MedicationConceptDTO()
                                                  {//if a Medication Resource is here, it should be from the Include clause and another Resource should be able to be matched by Id.

                                                      //If a Medication is included, it is its own Resource and does not have a MedRequest that can be referenced in this way.
                                                      //if this is a MedicationStatement, it defaults to Prescriber unknown.
                                                      //throws nullException at r... this is trying to find the Prescriber of the Medication by comparing the Medication resources of the MedReqs but not all of them have Medications Resources and it is throwing null
                                                      Prescriber = resources.Select(p => p as MedicationRequest)
                                                            .Where(r => r.Medication !=null)
                                                            .Where(m=> (m.Medication as ResourceReference).Reference == $"Medication/{med.Id}")
                                                            .First().Requester == null
                                                                ? "Prescriber Unknown"
                                                                : resources.Select(p => p as MedicationRequest).Where(r => (r.Medication as ResourceReference).     Reference == $"Medication/{med.Id}").First().Requester.ToString(),

                                                      //TimeOrdered = resources.Select(p => p as MedicationRequest).Where(r => (r.Medication as ResourceReference).Reference == $"Medication/{med.Id}").First().AuthoredOnElement == null
                                                      //    ? resources.Select(p => p as MedicationRequest).Where(r => (r.Medication as ResourceReference).Reference == $"Medication/{med.Id}").First().AuthoredOnElement.ToDateTimeOffset(TimeZoneInfo.Local.BaseUtcOffset)
                                                      //    : resources.Select(p => p as MedicationStatement).Where(r => (r.Medication as ResourceReference).Reference == $"Medication/{med.Id}").First().DateAssertedElement.ToDateTimeOffset(TimeZoneInfo.Local.BaseUtcOffset),

                                                      ResourceId = resources.Select(p => p as MedicationRequest)
                                                      .Where(r => r.Medication !=null)
                                                      .Where(m => (m.Medication as ResourceReference).Reference == $"Medication/{med.Id}")
                                                      .FirstOrDefault().Id,
                                                      FhirType = med.GetType()
                                                         .ToString(),
                                                      Sys = s.System
                                                         .ToLower(),
                                                      CodeString = s.Code,
                                                      Text = med.Code.Coding.FirstOrDefault().Display
                                                  }).FirstOrDefault());



                        break;
                    //if the Medication element is a Reference to another Medication Resource, the MedicationRequest is not kept but is referenced in the Medication that is included. This is the desired behavior but im not certain why its happening
                    case ResourceType.MedicationRequest:
                        var medReq = item as MedicationRequest;
                        var medReqMed = medReq.Medication as CodeableConcept;//this is null when medication is contained
                        var codeReq = medReq.Contained.Select(x => x.ResourceType == ResourceType.Medication ? x as Medication : null);
                        if (codeReq.Any(m => m.ResourceType == ResourceType.Medication))//this should pick out the contained resource if it exists
                        {
                            var q = codeReq.Select(c => c.Code.Coding.Select(s => new MedicationConceptDTO()
                            {
                                Prescriber = medReq.Requester == null
                                                          ? "Prescriber Unknown"
                                                          : medReq.Requester.Reference,
                                TimeOrdered = medReq.AuthoredOnElement == null
                                                          ? DateTimeOffset.UtcNow
                                                          : medReq.AuthoredOnElement.ToDateTimeOffset(TimeZoneInfo.Local.BaseUtcOffset),
                                ResourceId = medReq.Id,
                                FhirType = medReq.GetType()
                                                          .ToString(),
                                Sys = s.System
                                                          .ToLower(),
                                CodeString = s.Code,
                                Text = c.Code.Text
                            }).FirstOrDefault());

                            q.Select(c => channel.Writer.WriteAsync(c)).ToList();//if this isn't sent ToList, it doesnt add to the channel

                            break;
                        }
                        else
                        {
                            if (medReqMed == null)
                            {
                                Debug.WriteLineIf(medReqMed == null, $"MedicationRequest {item.Id} contains no Medication");
                                break;
                            }
                            else
                            {


                                channel.Writer.WriteAsync(medReqMed.Coding.Select(s =>
                                                     new MedicationConceptDTO()
                                                     {
                                                         Prescriber = medReq.Requester == null
                                                            ? "Prescriber Unknown"
                                                            : medReq.Requester.Reference,
                                                         TimeOrdered = medReq.AuthoredOnElement == null
                                                            ? DateTimeOffset.UtcNow
                                                            : medReq.AuthoredOnElement.ToDateTimeOffset(TimeZoneInfo.Local.BaseUtcOffset),
                                                         ResourceId = medReq.Id,
                                                         FhirType = medReq.GetType()
                                                            .ToString(),
                                                         Sys = s.System
                                                            .ToLower(),
                                                         CodeString = s.Code,
                                                         Text = medReqMed.Text
                                                     }).FirstOrDefault());
                                break;
                            }

                        }
                    case ResourceType.MedicationStatement:
                        var medState = item as MedicationStatement;
                        var medStateMed = medState.Medication as CodeableConcept;
                        var codeState = medState.Contained.Select(x => x.ResourceType == ResourceType.Medication ? x as Medication : null);
                        if (codeState.Any(m => m.ResourceType == ResourceType.Medication))
                        {
                            codeState.Select(x => channel.Writer.WriteAsync(x.Code.Coding.Select(s =>
                                                 new MedicationConceptDTO()
                                                 {
                                                     Prescriber = medState.InformationSource == null
                                                      ? ""
                                                      : medState.InformationSource.Reference,
                                                     TimeOrdered = medState.DateAssertedElement == null
                                                      ? DateTimeOffset.UtcNow
                                                      : medState.DateAssertedElement.ToDateTimeOffset(TimeZoneInfo.Local.BaseUtcOffset),
                                                     ResourceId = item.Id,
                                                     FhirType = item.GetType()
                                                      .ToString(),
                                                     Sys = s.System
                                                      .ToLower(),
                                                     CodeString = s.Code,
                                                     Text = medStateMed.Text
                                                 }).FirstOrDefault()
                        )); break;
                        }
                        else
                        {
                            if (medStateMed == null)
                            {
                                Debug.WriteLineIf(medStateMed == null, $"MedicationStatement {item.Id} contains no Medication");
                                break;
                            }
                            else
                            {
                                channel.Writer.WriteAsync(medStateMed.Coding.Select(s =>
                                                    new MedicationConceptDTO()
                                                    {
                                                        Prescriber = medState.InformationSource == null
                                                         ? ""
                                                         : medState.InformationSource.Reference,
                                                        TimeOrdered = medState.DateAssertedElement == null
                                                         ? DateTimeOffset.UtcNow
                                                         : medState.DateAssertedElement.ToDateTimeOffset(TimeZoneInfo.Local.BaseUtcOffset),
                                                        ResourceId = item.Id,
                                                        FhirType = item.GetType()
                                                         .ToString(),
                                                        Sys = s.System
                                                         .ToLower(),
                                                        CodeString = s.Code,
                                                        Text = medStateMed.Text
                                                    }).FirstOrDefault()
                            );
                            }

                            break;

                        }


                }
            }
        }
        //void DtoReturnStates(List<MedicationStatement> statements)
        //{
        //    foreach (var item in statements)
        //    {
        //        if (item.Medication == null)
        //        {
        //            Debug.WriteLineIf(item.Medication == null, $"MedicationStatement {item.Id} contains no Medication");
        //        }
        //        else
        //        {
        //            //TODO: This will need to handle Medication resource references as well.
        //            var code = item.Medication as CodeableConcept;

        //            channel.Writer.WriteAsync(code.Coding.Select(s =>
        //                                        new MedicationConceptDTO()
        //                                        {
        //                                            Prescriber = item.InformationSource == null
        //                                               ? ""
        //                                               : item.InformationSource.Reference,
        //                                            TimeOrdered = item.DateAssertedElement == null
        //                                               ? DateTimeOffset.UtcNow
        //                                               : item.DateAssertedElement.ToDateTimeOffset(),
        //                                            ResourceId = item.Id,
        //                                            FhirType = item.GetType()
        //                                               .ToString(),
        //                                            Sys = s.System
        //                                               .ToLower(),
        //                                            CodeString = s.Code,
        //                                            Text = code.Text
        //                                        }).FirstOrDefault()).ConfigureAwait(false);
        //        }
        //    }
        //}


        //async Task<MedicationConceptDTO> dtoReturnStatement(IAsyncEnumerable<MedicationStatement> resources)
        //{
        //    await foreach (var item in resources)//If the query object was of a different type, it would be easier to integrate into other systems. It should ba a list of StringConcept?
        //    {
        //        if (item.Medication == null)
        //        {
        //            Debug.WriteLineIf(item.Medication == null, $"MedicationStatement {item.Id} contains no Medication");
        //            return null;
        //        }
        //        else
        //        {

        //            //TODO: This will need to handle Medication resource references as well.
        //            var code = item.Medication as CodeableConcept;

        //            return (code.Coding.Select(s =>
        //                                        new MedicationConceptDTO()
        //                                        {
        //                                            Prescriber = item.InformationSource == null ? "" : item.InformationSource.Reference,
        //                                            TimeOrdered = item.DateAssertedElement == null ? DateTimeOffset.UtcNow : item.DateAssertedElement.ToDateTimeOffset(),
        //                                            ResourceId = item.Id,
        //                                            FhirType = item.GetType().ToString(),
        //                                            Sys = s.System.ToLower(),
        //                                            CodeString = s.Code,
        //                                            Text = code.Text
        //                                        }).FirstOrDefault());
        //        }
        //        //try //need a try statement incase there in no CodeableConcept
        //        //{
        //        //}
        //        //catch (NullReferenceException)
        //        //{

        //        //}
        //    }
        //    return null;
        //}
    }
}


