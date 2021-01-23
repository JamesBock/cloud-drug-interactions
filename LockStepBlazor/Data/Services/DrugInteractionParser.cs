using LockStepBlazor.Data.Models;
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
    public class DrugInteractionParser : IDrugInteractionParser, IObservable<MedicationInteractionPair>
    {
        public DrugInteractionParser()
        {
            observers = new List<IObserver<MedicationInteractionPair>>();
        }
        public void ParseDrugInteractions(string jstring)
        {

            var interactionList = new List<MedicationInteractionPair>();
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

                    interactionList.Add(interaction);

                    var interactionConceptCount = j["fullInteractionTypeGroup"][f]["fullInteractionType"][i]["interactionPair"].Children()["interactionConcept"].ToList().Count();


                    string PATTERN = @"interaction.(?:(?!\.).)*";
                    var m = Regex.Matches(interaction.Comment, PATTERN);
                    //int parseHelper = 5;
                    for (int p = 0; p < interactionConceptCount; p++)
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

                        //detail.InteractionAssertion = (interaction.Comment.Substring(m.Index + 4)[0])+ interaction.Comment.Substring(m.Index + 5)?? "No Details on Source available";

                        //string[] stringArray = { "Drug1", "Drug2" };
                        //var splitString = interaction.Comment.Split(stringArray, StringSplitOptions.RemoveEmptyEntries);
                        //string levelOne =  p == 0 ? splitString[3] : splitString[parseHelper];
                        //var levelTwo  = levelOne.Split("and ", 2)[1];
                        //detail.InteractionAssertion = char.ToUpper(levelTwo[0]) + levelTwo.Substring(1);
                        //while (p > 0)
                        //{

                        //    parseHelper = splitString.Count() - 1 > parseHelper ? parseHelper + 2 : 5;
                        //    break;
                        //}

                        interaction.DrugInteractionDetails.Add(detail);
                    }

                    foreach (var observer in observers)
                    {
                        observer.OnNext(interaction);
                    }
                }
                if (interactionPairCount == 0)
                {
                    var emptyDrug = new MedicationInteractionPair();
                    emptyDrug.DrugInteractionDetails.Add(

                                        new MedicationInteractionPair.InteractionDetail()
                                        { Description = "No Drug-Drug Interactions Found", Severity = "N/A", LinkTupList = new List<(string, Uri)>() { ("NIH", new Uri(Constants.NLM_INTERACTION_API_URI)) } });
                    //yield return emptyDrug;
                    foreach (var observer in observers)
                    {
                        observer.OnNext(emptyDrug);
                    }
                    //return new GetDrugInteractions.Model() { Meds = interactionList };
                }
            }


            foreach (var o in observers)
            {
                o.OnCompleted();
            }
            //return interactionList;
        }

        private List<IObserver<MedicationInteractionPair>> observers;

        public IDisposable Subscribe(IObserver<MedicationInteractionPair> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
            return new Unsubscriber(observers, observer);
        }

        private class Unsubscriber : IDisposable
        {
            private List<IObserver<MedicationInteractionPair>> _observers;
            private IObserver<MedicationInteractionPair> _observer;

            public Unsubscriber(List<IObserver<MedicationInteractionPair>> observers, IObserver<MedicationInteractionPair> observer)
            {
                this._observers = observers;
                this._observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }

    }
}
