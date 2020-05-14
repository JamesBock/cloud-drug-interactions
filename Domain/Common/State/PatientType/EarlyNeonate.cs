using System;
using System.Collections.Generic;
using System.Text;

namespace UWPLockStep.Domain.Common.State.PatientType
{
    public class EarlyNeonate : PatientType
    {
        public override double GestationalAge => (DateTime.Now - DateOfConception).TotalDays;
        public string GestationalAgeString => (GestationalAge / 7).ToString() + "weeks";
        public override string TypeDescription => "0 to 2 days of life"; //IToolTip?
    }
}
