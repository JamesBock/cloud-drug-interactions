using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stateless;

namespace UWPLockStep.Domain.Common.State.PatientType
{
    public class PatientStatelessMachine
    {
        //This approach may not be viable
        enum Trigger { Create, StatusCheck};
        enum State { 
            ExtremelyPretermEarlyNeonate,
            VeryPretermEarlyNeonate,
            ModeratelyPretermEarlyNeonate,
            LatePretermEarlyNeonate,
            EarlyTermEarlyNeonate,
            TermOrUnknownEarlyNeonate,

            ExtremelyPretermNeonate,
            VeryPretermNeonate,
            ModeratelyPretermNeonate,
            LatePretermNeonate,
            EarlyTermNeonate,
            TermOrUnknownNeonate,


        };
    }
}
