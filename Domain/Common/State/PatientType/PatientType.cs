using System;
using System.Collections.Generic;
using System.Text;
using UWPLockStep.Domain.Common.State.Prematurity;

namespace UWPLockStep.Domain.Common.State.PatientType
{
    public abstract class PatientType
    {   //Hack: this will slow performace
        public virtual double  Age => (DateTime.Now - DateOfBirth).Days; 
        public DateTime DateOfBirth { get; }
        public virtual double GestationalAge { get; }//=> ((DateTime.Now - DateOfConception).TotalDays)/7;
        public virtual DateTime DateOfConception { get; }
        public PrematurityEnumeration Prematurity { get; set; } //Prematuiry at birth. status is fixed
        public virtual string TypeDescription { get; }
        /// <summary>
        /// this should run in the ctor of the concreteclass that implements it
        /// </summary>
        public void SetPrematurity()//TODO: Check math on dates!
        {
            
            switch (GestationalAge)
            {
                case double d when d <= 196:  //28*7
                    Prematurity = PrematurityEnumeration.ExtremelyPreterm;
                    break;

                case double d when d > 196 && d <= 224:  //32*7 to 
                    Prematurity = PrematurityEnumeration.VeryPreterm;
                    break;

                case double d when d > 224 && d < 238:  //34*7
                    Prematurity = PrematurityEnumeration.ModeratelyPreterm;
                    break;

                case double d when d >= 238 && d < 259:  //37*7
                    Prematurity = PrematurityEnumeration.LatePreterm;
                    break;

                case double d when d >= 259 && d <= 272:  //*7
                    Prematurity = PrematurityEnumeration.EarlyTerm;
                    break;
                    
                default: //What if there is no DateOfConception?
                    Prematurity = PrematurityEnumeration.TermOrUnknown;
                    break;
            }
        }
    }
}