using System;
using System.Collections.Generic;
using System.Text;


namespace LockStepBlazor.Data.Models
{
    public abstract class PatientStatus
    {
        public abstract void EnterStatus(LockStepPatient patient);

        public abstract void DischargePatient(LockStepPatient patient);

        public abstract void TransferPatient(LockStepPatient patient, int roomId);
    }
}
