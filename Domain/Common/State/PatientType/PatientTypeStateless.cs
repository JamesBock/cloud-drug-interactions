using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stateless;
using UWPLockStep.Domain.Common.State.Prematurity;

namespace UWPLockStep.Domain.Common.State.PatientType
{
    public class PatientTypeStateless
    {
        private readonly StateMachine<PatientTypeState, Trigger> _patientTypeState;
        private enum Trigger { Create, StatusCheck };
        public enum PatientTypeState
        {
            NewPatient,

            EarlyNeonate,
            Neonate,

            Infant,
            Child,
            PreschoolChild,
            Adolescent,

            YoungAdult,
            Adult,
            MiddleAged,

            SeniorAged,
            Octogenarian
        };

        public PatientTypeState PatientType => _patientTypeState.State;
        public PatientTypeStateless(PatientTypeState state, DateTimeOffset? birthday, DateTimeOffset? conception)
        {
            DateOfBirth = (DateTimeOffset)birthday;
            DateOfConception = (DateTimeOffset)conception;
            SetPrematurity();
            _patientTypeState = new StateMachine<PatientTypeState, Trigger>(state);

            #region Configuration
            //This Configuration is strictly on Age not GestationalAge which is appropriate in some cases but is captured in PrematuriyStatus?
            _patientTypeState.Configure(PatientTypeState.NewPatient)
                .PermitIf(Trigger.Create, PatientTypeState.EarlyNeonate, () => Age <= 2)
                .PermitIf(Trigger.Create, PatientTypeState.Neonate, () => Age > 2 && Age <= 28)
                .PermitIf(Trigger.Create, PatientTypeState.Infant, () => Age > 28 && Age <= 730)
                .PermitIf(Trigger.Create, PatientTypeState.PreschoolChild, () => Age > 730 && Age <= 1825)
                .PermitIf(Trigger.Create, PatientTypeState.Child, () => Age > 1825 && Age <= 4380)
                .PermitIf(Trigger.Create, PatientTypeState.Adolescent, () => Age > 4380 && Age <= 6935)
                .PermitIf(Trigger.Create, PatientTypeState.YoungAdult, () => Age > 6935 && Age <= 8760)
                .PermitIf(Trigger.Create, PatientTypeState.Adult, () => Age > 8760 && Age <= 16060)
                .PermitIf(Trigger.Create, PatientTypeState.MiddleAged, () => Age > 16060 && Age <= 23725)
                .PermitIf(Trigger.Create, PatientTypeState.SeniorAged, () => Age > 23725 && Age <= 29200)
                .PermitIf(Trigger.Create, PatientTypeState.Octogenarian, () => Age > 29200);

            _patientTypeState.Configure(PatientTypeState.Neonate)
                .Permit(Trigger.StatusCheck, PatientTypeState.Infant);

            _patientTypeState.Configure(PatientTypeState.EarlyNeonate)
                .SubstateOf(PatientTypeState.Neonate)
                .Permit(Trigger.StatusCheck, PatientTypeState.Neonate);

            _patientTypeState.Configure(PatientTypeState.Infant)
               .Permit(Trigger.StatusCheck, PatientTypeState.PreschoolChild);

            _patientTypeState.Configure(PatientTypeState.PreschoolChild)
              .Permit(Trigger.StatusCheck, PatientTypeState.Child);

            _patientTypeState.Configure(PatientTypeState.Child)
               .Permit(Trigger.StatusCheck, PatientTypeState.Adolescent);

            _patientTypeState.Configure(PatientTypeState.Adolescent)
              .Permit(Trigger.StatusCheck, PatientTypeState.YoungAdult);

            _patientTypeState.Configure(PatientTypeState.YoungAdult)
                .SubstateOf(PatientTypeState.Adult)
              .Permit(Trigger.StatusCheck, PatientTypeState.Adult);

            _patientTypeState.Configure(PatientTypeState.Adult)
              .Permit(Trigger.StatusCheck, PatientTypeState.MiddleAged);

            _patientTypeState.Configure(PatientTypeState.MiddleAged)
                .SubstateOf(PatientTypeState.Adult)
              .Permit(Trigger.StatusCheck, PatientTypeState.SeniorAged);

            _patientTypeState.Configure(PatientTypeState.SeniorAged)
              .Permit(Trigger.StatusCheck, PatientTypeState.Octogenarian);
            #endregion

            _patientTypeState.Fire(Trigger.Create);
        }

        public void CheckState()
        {
            //TODO: Each day at the time of birth, check for State change
        }


        public virtual double Age => (DateTime.Now - DateOfBirth).Days;
        public DateTimeOffset DateOfBirth { get; }
        public virtual double GestationalAge => ((DateTime.Now - DateOfConception).TotalDays)/7;
        public virtual DateTimeOffset DateOfConception { get; }
        public PrematurityEnumeration Prematurity { get; set; } //Prematuiry at birth. status is fixed
        public virtual string TypeDescription { get; }
        /// <summary>
        /// this should run in the ctor of the concreteclass that implements it
        /// </summary>
        public void SetPrematurity()//TODO: Check math on dates!
        {

            switch ((DateOfBirth - DateOfConception).Days)
            {
                case int i when i <= 196:  //28*7
                    Prematurity = PrematurityEnumeration.ExtremelyPreterm;
                    break;

                case int i when i > 196 && i <= 224:  //32*7 to 
                    Prematurity = PrematurityEnumeration.VeryPreterm;
                    break;

                case int i when i > 224 && i < 238:  //34*7
                    Prematurity = PrematurityEnumeration.ModeratelyPreterm;
                    break;

                case int i when i >= 238 && i < 259:  //37*7
                    Prematurity = PrematurityEnumeration.LatePreterm;
                    break;

                case int i when i >= 259 && i <= 272:  //*7
                    Prematurity = PrematurityEnumeration.EarlyTerm;
                    break;

                default: //What if there is no DateOfConception?
                    Prematurity = PrematurityEnumeration.TermOrUnknown;
                    break;
            }
        }
    }
}
