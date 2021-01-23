using System;
using System.Collections.Generic;
using System.Text;
using LockStepBlazor.Data.Models;

namespace LockStepBlazor.Application
{
    public abstract class PatientStatus
    {
        public abstract void EnterStatus(LockStepPatient patient);

        public abstract void DischargePatient(LockStepPatient patient);

        public abstract void TransferPatient(LockStepPatient patient, int roomId);
    }
}
