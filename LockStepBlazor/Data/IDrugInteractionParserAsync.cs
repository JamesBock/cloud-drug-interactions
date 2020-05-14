using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPLockStep.Domain.Common;

namespace LockStepBlazor.Data
{
    public interface IDrugInteractionParserAsync
    {
        public IAsyncEnumerable<MedicationInteractionPair> ParseDrugInteractionsAsync(string jstring);
    }
}
