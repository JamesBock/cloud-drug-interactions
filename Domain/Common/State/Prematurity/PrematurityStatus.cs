using System;
using System.Collections.Generic;
using System.Text;

namespace UWPLockStep.Domain.Common.State.Prematurity
{
    public abstract class PrematurityStatus // this could just be an Enum or enumeration. Is only 
    {
        //public double LowerLimt { get; }
        //public double UpperLimit { get; }
        public double Gestation { get; set; }
        private void SetPrematurityStatus(double gestation)
        {
            Gestation = gestation;

        } 

    }
}
