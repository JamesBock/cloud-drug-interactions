using System;
using System.Collections.Generic;
using System.Text;
using UWPLockStep.Domain.Entities.People;

namespace UWPLockStep.Domain.Common.State
{
    public class PreadmissionStatus : PatientStatus
    {
        public override void DischargePatient(LockStepPatient patient)
        {
           
        }

        public override void EnterStatus(LockStepPatient patient)
        {
            
        }

        public override void TransferPatient(LockStepPatient patient, int roomId)
        {
            
        }
    }
}
