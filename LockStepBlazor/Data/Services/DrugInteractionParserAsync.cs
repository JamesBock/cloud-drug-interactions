using LockStepBlazor.Shared;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UWPLockStep.Domain.Common;

namespace LockStepBlazor.Data.Services
{
    public class DrugInteractionParserAsync : IDrugInteractionParserAsync
    {
        public async IAsyncEnumerable<MedicationInteractionPair> ParseDrugInteractionsAsync(string jstring)
        {

            //var interactionList = new List<MedicationInteractionPair>();
            JObject j = new JObject();
            int tokenCount = 0;
            int fullInteractiveTypeCount = 0;
            int interactionPairCount = 0;
            var minConceptTokenList = new List<JToken>();
            var urlTokenList = new List<JToken>();

            j = JObject.Parse(jstring);
            try
            {
                tokenCount = j["fullInteractionTypeGroup"].Children()["fullInteractionType"].Children()["minConcept"].ToList().Count();
                fullInteractiveTypeCount = j["fullInteractionTypeGroup"].Children()["fullInteractionType"].ToList().Count();
            }

            catch (NullReferenceException)
            {
                tokenCount = 0;
                fullInteractiveTypeCount = 0;
                interactionPairCount = 0;
            }


            for (int f = 0; f < fullInteractiveTypeCount; f++)
            {
                interactionPairCount = j["fullInteractionTypeGroup"][f]["fullInteractionType"].Children()["interactionPair"].ToList().Count();

                for (int i = 0; i < interactionPairCount; i++)
                {
                    var interaction = new MedicationInteractionPair() { InteractionId = Guid.NewGuid() };

                    interaction.Comment = j["fullInteractionTypeGroup"][f]["fullInteractionType"][i]["comment"].ToString();
                    interaction.MedicationPair = (new MedicationInteractionPair.MedicationInteractionViewModel() { DisplayName = j["fullInteractionTypeGroup"][f]["fullInteractionType"][i]["minConcept"][0]["name"].ToString(), RxCui = j["fullInteractionTypeGroup"][f]["fullInteractionType"][i]["minConcept"][0]["rxcui"].ToString() }, new MedicationInteractionPair.MedicationInteractionViewModel() { DisplayName = j["fullInteractionTypeGroup"][f]["fullInteractionType"][i]["minConcept"][1]["name"].ToString(), RxCui = j["fullInteractionTypeGroup"][f]["fullInteractionType"][i]["minConcept"][1]["rxcui"].ToString() });

                    //MinConcept also contains the RxCuis, which can be used to .Join these with the MedDTO object thing  

                    //interactionList.Add(interaction);

                    var interactionConceptCount = j["fullInteractionTypeGroup"][f]["fullInteractionType"][i]["interactionPair"].Children()["interactionConcept"].ToList().Count();


                    string PATTERN = @"interaction.(?:(?!\.).)*";
                    var m = Regex.Matches(interaction.Comment, PATTERN);


                    // for (int p = 0; p < interactionConceptCount; p++)
                    // {

                    //     var detail = new MedicationInteractionPair.InteractionDetail();

                    //     detail.InteractionAssertion = char.ToUpper(m[p].Groups[0].Value[0]) + m[p].Groups[0].Value.Substring(1);

                    //     detail.Description = j["fullInteractionTypeGroup"][f]["fullInteractionType"][i]["interactionPair"][p]["description"].ToString();

                    //     detail.Severity = j["fullInteractionTypeGroup"][f]["fullInteractionType"][i]["interactionPair"][p]["severity"].ToString();

                    //     detail.LinkTupList = new List<(string, Uri)>(j["fullInteractionTypeGroup"][f]["fullInteractionType"][i]["interactionPair"][p]["interactionConcept"].Children()["minConceptItem"]["name"].ToList()
                    //                                 .Select(x => x.ToString().ToUpper())
                    //                                 .Zip(j["fullInteractionTypeGroup"][f]["fullInteractionType"][i]["interactionPair"][p]["interactionConcept"].Children()["sourceConceptItem"]["url"].ToList()
                    //                                 .Select(v => new Uri(v.ToString().Equals("NA")
                    //                                 ? "https://www.ncbi.nlm.nih.gov/pmc/articles/PMC3422823/"
                    //                                 : v.ToString()))));


                    //     interaction.DrugInteractionDetails.Add(detail);
                    // }
                    var option = new ParallelOptions();
                   
                    Parallel.For(0, interactionConceptCount, (p, state) =>
                    {

                        var detail = new MedicationInteractionPair.InteractionDetail();

                            detail.InteractionAssertion = char.ToUpper(m[p].Groups[0].Value[0]) + m[p].Groups[0].Value.Substring(1);

                            detail.Description = j["fullInteractionTypeGroup"][f]["fullInteractionType"][i]["interactionPair"][p]["description"].ToString();

                            detail.Severity = j["fullInteractionTypeGroup"][f]["fullInteractionType"][i]["interactionPair"][p]["severity"].ToString();

                            detail.LinkTupList = new List<(string, Uri)>(j["fullInteractionTypeGroup"][f]["fullInteractionType"][i]["interactionPair"][p]["interactionConcept"].Children()["minConceptItem"]["name"].ToList()
                                                        .Select(x => x.ToString().ToUpper())
                                                        .Zip(j["fullInteractionTypeGroup"][f]["fullInteractionType"][i]["interactionPair"][p]["interactionConcept"].Children()["sourceConceptItem"]["url"].ToList()
                                                        .Select(v => new Uri(v.ToString().Equals("NA")
                                                        ? "https://www.ncbi.nlm.nih.gov/pmc/articles/PMC3422823/"
                                                        : v.ToString()))));


                        interaction.DrugInteractionDetails.Add(detail);

                    });

                    yield return interaction;

                }

                if (interactionPairCount == 0)
                {
                    var emptyDrug = new MedicationInteractionPair();
                    emptyDrug.DrugInteractionDetails.Add(

                                        new MedicationInteractionPair.InteractionDetail()
                                        { Description = "No Drug-Drug Interactions Found", Severity = "N/A", LinkTupList = new List<(string, Uri)>() { ("NIH", new Uri(Constants.NLM_INTERACTION_API_URI)) } });
                    yield return emptyDrug;

                    //return new GetDrugInteractions.Model() { Meds = interactionList };
                }
            }

            //return interactionList;
        }
    }
}
