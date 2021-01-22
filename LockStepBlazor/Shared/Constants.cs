using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LockStepBlazor.Shared
{
    public static class Constants
    {
        public static string GOOGLE_PROJECT = "mystic-sound-280300";
        public static string GOOGLE_lOCATION = "us-central1";
        public static string GOOGLE_DATASET = "lockstep-test-fhir";
        public static string GOOGLE_FHIRSTORE = "lockstep-test";
        public static string GOOGLE_FHIR_BASE_URI = "https://healthcare.googleapis.com/v1";
        public static string GOOGLE_CONSTANT = $"{GOOGLE_FHIR_BASE_URI}/projects/{GOOGLE_PROJECT}/locations/{GOOGLE_lOCATION}/datasets/{GOOGLE_DATASET}/fhirStores/{GOOGLE_FHIRSTORE}/fhir/";
        public static Uri GOOGLE_FHIR_STICHED_URI = new Uri(GOOGLE_CONSTANT);


        public const string FHIR_URI = "http://hapi.fhir.org/baseR4";
        public const string RXCUI_API_URI = "https://rxnav.nlm.nih.gov/REST/rxcui.json";
        public const string NLM_INTERACTION_API_URI = "https://rxnav.nlm.nih.gov/REST/interaction/";
    }
}
