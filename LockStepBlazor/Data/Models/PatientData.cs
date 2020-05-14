using System;

namespace LockStepBlazor.Data.Models
{
    public class PatientData
    {
        public string FullName { get; set; }

        public DateTimeOffset? BirthDay { get; set; }

        public DateTimeOffset? ConceptionDay { get; set; }
    }
}
