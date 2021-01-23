using LockStepBlazor.Data.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPLockStep.Domain.Common;

namespace LockStepBlazor.Data
{
    public interface IDrugInteractionParser : IObservable<MedicationInteractionPair>
    {
        public void ParseDrugInteractions(string jstring);
    }
}
