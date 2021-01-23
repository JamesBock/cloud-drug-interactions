using Hl7.Fhir.ElementModel;
using Hl7.Fhir.FhirPath;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Serialization;
using LockStepBlazor.Application.Interfaces;
using LockStepBlazor.Data.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace LockStepBlazor.Handlers
{
    public abstract class GetFhirMedicationsHandler : IGetFhirMedications.IHandler
    {
        protected readonly FhirClient client;

        //protected readonly Channel<MedicationConceptDTO> channel = Channel.CreateUnbounded<MedicationConceptDTO>();
        //protected readonly List<MedicationConceptDTO> meds = new List<MedicationConceptDTO>();

        public GetFhirMedicationsHandler(FhirClient client)
        {
            this.client = client;
        }

        public abstract Task<IGetFhirMedications.Model> Handle(IGetFhirMedications.Query request, CancellationToken cancellationToken);

        protected async Task<IEnumerable<MedicationConceptDTO>> ParseMedicationsAsync(List<Task<Bundle>> result)
        {
            var medos = new List<MedicationConceptDTO>();
            while (result.Count > 0)
            {

                var completedTask = await Task.WhenAny(result);
                //TODO: The Requester is being included here now...need to handle this..may need to happen a level above

                //new PrescriberDTO to with name etc...make part of FhirMedications.Model?
                medos.AddRange(
                             MedsToChannel(completedTask
                                 .GetAwaiter().GetResult().Entry
                                 .Select(e => e.Resource)
                                 .ToList())
                );
                result.Remove(completedTask);
            }
            return medos;

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


        IEnumerable<MedicationConceptDTO> MedsToChannel(List<Resource> resources)
        {
            foreach (var item in resources)
            {
                switch (item.TypeName)
                {
                    case "Medication"://will never have a contained medication
                        var med = item as Medication;

                        yield return (med.Code.Coding.Select(s =>
                                                  new MedicationConceptDTO()
                                                  {//if a Medication Resource is here, it should be from the Include clause and another Resource should be able to be matched by Id.

                                                      //If a Medication is included, it is its own Resource and does not have a MedRequest that can be referenced in this way.
                                                      //if this is a MedicationStatement, it defaults to Prescriber unknown.
                                                      Prescriber = resources.Select(p => p as MedicationRequest)
                                                            .Where(r => r.Medication != null)
                                                            .Where(m => (m.Medication as ResourceReference).Reference == $"Medication/{med.Id}")
                                                            .First().Requester == null
                                                                ? "Prescriber Unknown"
                                                                : resources.Select(p => p as MedicationRequest).Where(r => (r.Medication as ResourceReference).Reference == $"Medication/{med.Id}").First().Requester.Display.ToString(),

                                                      //TimeOrdered = resources.Select(p => p as MedicationRequest).Where(r => (r.Medication as ResourceReference).Reference == $"Medication/{med.Id}").First().AuthoredOnElement == null
                                                      //    ? resources.Select(p => p as MedicationRequest).Where(r => (r.Medication as ResourceReference).Reference == $"Medication/{med.Id}").First().AuthoredOnElement.ToDateTimeOffset(TimeZoneInfo.Local.BaseUtcOffset)
                                                      //    : resources.Select(p => p as MedicationStatement).Where(r => (r.Medication as ResourceReference).Reference == $"Medication/{med.Id}").First().DateAssertedElement.ToDateTimeOffset(TimeZoneInfo.Local.BaseUtcOffset),

                                                      ResourceId = resources.Select(p => p as MedicationRequest)
                                                      .Where(r => r.Medication != null)
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
                    case "MedicationRequest":
                        var medReq = item as MedicationRequest;
                        var medReqMed = medReq.Medication as CodeableConcept;//this is null when medication is contained
                        var codeReq = medReq.Contained.Select(x => x.TypeName == "Medication" ? x as Medication : null);
                        if (codeReq.Any(m => m.TypeName == "Medication"))//this should pick out the contained resource if it exists
                        {
                            yield return codeReq.Select(s => new MedicationConceptDTO()
                            {
                                Prescriber = medReq.Requester == null
                                                          ? "Prescriber Unknown"
                                                          : medReq.Requester.Display,
                                TimeOrdered = medReq.AuthoredOnElement == null
                                                          ? DateTimeOffset.UtcNow
                                                          : medReq.AuthoredOnElement.ToDateTimeOffset(TimeZoneInfo.Local.BaseUtcOffset),
                                ResourceId = medReq.Id,
                                FhirType = medReq.GetType()
                                                          .ToString(),
                                Sys = s.Code.Coding.FirstOrDefault().System
                                                          .ToLower(),
                                CodeString = s.Code.Coding.FirstOrDefault().Code,
                                Text = s.Code.Coding.FirstOrDefault().Display
                            }).FirstOrDefault()/*.ToList()*/;

                            // q.ForEach(c => meds.Add(c));//if this isn't sent ToList, it doesnt add to the channel

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


                                yield return (medReqMed.Coding.Select(s =>
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
                    //TODO: need null checks here
                    case "MedicationStatement":
                        var medState = item as MedicationStatement;
                        var medStateMed = medState.Medication as CodeableConcept;
                        var codeState = medState.Contained
                            .Select(x => x.TypeName == "Medication" ? x as Medication : null)
                            .ToList();


                        if (codeState.Any(m => m.TypeName == "Medication"))
                        {
                            yield return codeState.SelectMany(p => p.Code.Coding)

                                     .Select(s =>
                                                 new MedicationConceptDTO()
                                                 {
                                                     //  Prescriber = medState.InformationSource == null
                                                     //   ? ""
                                                     //   : medState.InformationSource.Reference,
                                                     //  TimeOrdered = medState.DateAssertedElement == null
                                                     //   ? DateTimeOffset.UtcNow
                                                     //   : medState.DateAssertedElement.ToDateTimeOffset(TimeZoneInfo.Local.BaseUtcOffset),
                                                     //  ResourceId = item.Id,
                                                     //  FhirType = item.GetType()
                                                     //   .ToString(),
                                                     //  Sys = s.System
                                                     //   .ToLower(),
                                                     //  CodeString = s.Code,
                                                     //Text = medStateMed.Text
                                                     Text = s.Display
                                                 }
                                                ).FirstOrDefault();

                            break;
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
                                yield return (medStateMed.Coding.Select(s =>
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


