using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPLockStep.Domain.Common;
using UWPLockStep.Domain.Common.State.PatientType;
using UWPLockStep.Domain.Common.Units;
using UWPLockStep.Domain.Entities.Factors;
using UWPLockStep.Domain.Entities.Ingredients;
using UWPLockStep.Domain.Entities.Intermediates;
using UWPLockStep.Domain.Entities.Orders;
using UWPLockStep.Domain.Entities.Orders.States;
using UWPLockStep.Domain.Entities.People;
using UWPLockStep.Domain.Entities.Policies;
using UWPLockStep.Domain.Interfaces;

namespace UWPLockStep.Persistance
{
    public class DemoData
    {
        public ICollection<LockStepPatient> Patients { get; set; } = new List<LockStepPatient>();
        public LockStepPatient DemoPatient { get; set; }
        public FluidOrder DemoFluidOrderOne { get; set; }
        public FluidOrder DemoFluidOrderTwo { get; set; }
        public CompoundedFluidOrderItem DemoFluidOrderItemOne { get; set; }
        public FluidOrderItem DemoFluidOrderItemTwo { get; set; }
        public ICollection<ICombinablePolicy<FactorBase>> Policies { get; set; }


        public DemoData()
        {
            var routeList = new Collection<AdministrationRouteEnumeration>
            {
                InjectionRoute.IVCentral
            };
            var patientTypeLIst = new Collection<PatientTypeStateless.PatientTypeState>
            {
                PatientTypeStateless.PatientTypeState.Neonate,
                PatientTypeStateless.PatientTypeState.EarlyNeonate,
            };

            #region Patient

            //Patient aged out of Neonate when turned 29 days old. Changing birthdate to dynamic age.
            DemoPatient = new LockStepPatient(DateTime.Now-TimeSpan.FromDays(15), new DateTime(2019, 3, 29), new List<string>() { "James" }, "Bock") { Weight = new MonoUnitValue(new MonoUnit(Mass.Kilogram), 1.390m) };
            #endregion
            Patients.Add(DemoPatient);

            #region Factors
            var cysteine = new Factor(new MonoUnit(Mass.Milligram)) { Name = "Cystiene" };
            var aa = new Factor(new MonoUnit(Mass.Gram)) { Name = "Amino Acids" };
            var glucose = new Factor(new MonoUnit(Mass.Gram)) { Name = "Glucose" };
            var sodium = new ElementFactor("Sodium") { Name = "Sodium" };
            var calcium = new ElementFactor("Calcium") { Name = "Calcium" };
            var heparin = new Factor(new MonoUnit(Units.Unit)) { Name = "Heparin" };
            #endregion

            #region Orders

            DemoFluidOrderOne = new FluidOrder(new MonoUnit(Volume.Milliliter)) { Id = Guid.NewGuid(), AdministrationRoute = routeList };
            DemoFluidOrderTwo = new FluidOrder(new MonoUnit(Volume.Milliliter)) { Id = Guid.NewGuid(), AdministrationRoute = routeList };
            #endregion


            #region Ingredients

            var lCystieneHCL = new Ingredient(new MonoUnit(Volume.Milliliter)) { Name = "L-Cystiene HCl", Id = Guid.NewGuid() };
            var trophamine = new Ingredient(new MonoUnit(Volume.Milliliter)) { Name = "Trophamine", Id = Guid.NewGuid() };
            var renalAmine = new Ingredient(new MonoUnit(Volume.Milliliter)) { Name = "Renal AAs", Id = Guid.NewGuid() };
            var glucoseSolution = new Ingredient(new MonoUnit(Volume.Milliliter)) { Name = "Dextrose 70% solution", Id = Guid.NewGuid() };
            var calciumInjection = new Ingredient(new MonoUnit(Volume.Milliliter)) { Name = "Calcium Injection", Id = Guid.NewGuid() };
            var SodiumInjection = new Ingredient(new MonoUnit(Volume.Milliliter)) { Name = "Sodium Injection", Id = Guid.NewGuid() };
            var heparinInjection = new Ingredient(new MonoUnit(Volume.Milliliter)) { Name = "Heparin Injection", Id = Guid.NewGuid() };
            #endregion

            #region Intermediates

            var cystieneInCycstieneHCL = new FactorItem(cysteine, lCystieneHCL, 50m);//Defining the amount of Factor in the Ingredient
            var aaInTrophamine = new FactorItem(aa, trophamine, .1m);

            var aaInRenalAA = new FactorItem(aa, renalAmine, .1m);

            var sodiumInCystiene = new FactorItem(sodium, lCystieneHCL, .02m);
            var sodiumInTrophamine = new FactorItem(sodium, trophamine, .01m);
            var sodiumInD5 = new FactorItem(sodium, SodiumInjection, .05m);
            var calciumInInjection = new FactorItem(calcium, calciumInjection, .12m);
            var glucosInD70 = new FactorItem(glucose, glucoseSolution, .7m);
            var heparinInInjection = new FactorItem(heparin, heparinInjection, .1m);

            //FactorItem Assignments
            lCystieneHCL.FactorItems.Add(cystieneInCycstieneHCL);
            lCystieneHCL.FactorItems.Add(sodiumInCystiene);

            trophamine.FactorItems.Add(aaInTrophamine);
            trophamine.FactorItems.Add(sodiumInTrophamine);

            renalAmine.FactorItems.Add(aaInRenalAA);

            glucoseSolution.FactorItems.Add(glucosInD70);

            calciumInjection.FactorItems.Add(calciumInInjection);

            SodiumInjection.FactorItems.Add(sodiumInD5);

            heparinInjection.FactorItems.Add(heparinInInjection);


            var lCysteineInjectionItem = new IngredientItem(lCystieneHCL, DemoFluidOrderOne, 3m, true);
            var trophAAItem = new IngredientItem(trophamine, DemoFluidOrderOne, 1m, true);
            var renAmineItem = new IngredientItem(renalAmine, DemoFluidOrderOne, 5m, true);
            var calciumInjectionItem = new IngredientItem(calciumInjection, DemoFluidOrderOne, .09m, true);
            var glucoseInjectionItem = new IngredientItem(glucoseSolution, DemoFluidOrderOne, .23m, true);
            var heparinInjectionItem = new IngredientItem(heparinInjection, DemoFluidOrderOne, .1m, true);

            DemoFluidOrderOne.IngredientCollection.Add(lCysteineInjectionItem);
            DemoFluidOrderOne.IngredientCollection.Add(trophAAItem);
            DemoFluidOrderOne.IngredientCollection.Add(renAmineItem);
            DemoFluidOrderOne.IngredientCollection.Add(calciumInjectionItem);
            DemoFluidOrderOne.IngredientCollection.Add(glucoseInjectionItem);
            DemoFluidOrderOne.IngredientCollection.Add(heparinInjectionItem);


            DemoFluidOrderTwo.IngredientCollection.Add(glucoseInjectionItem);


            DemoFluidOrderItemOne = new CompoundedFluidOrderItem(DemoPatient, DemoFluidOrderOne, 168m, new DurationValue(Duration.Hour, 24), 1, UnitValueFactory.Create(Volume.Milliliter, 100m)) { TimeExecuted = DateTime.Now - TimeSpan.FromHours(12d), AdministrationRoute = InjectionRoute.IVCentral };//Defining the Amount of the Order that is dosed to the Patient
            DemoFluidOrderItemOne.TransitionToState(new ActiveState());//If order should be Expired, this will automatically change to ExpiredState

            DemoFluidOrderItemTwo = new FluidOrderItem(DemoPatient, DemoFluidOrderTwo, 20m, new DurationValue(Duration.Hour, 24), 1) { TimeExecuted = DateTime.Now - TimeSpan.FromHours(12d), AdministrationRoute = InjectionRoute.IVCentral };
            DemoFluidOrderItemTwo.TransitionToState(new ActiveState());
            #endregion






            #region Policies

            Policies = new Collection<ICombinablePolicy<FactorBase>>();

            var aaPolicy = new FactorWeightDurationPolicy(aa, new DurationValue(Duration.Day, 1), routeList, patientTypeLIst, Mass.Kilogram);
            aaPolicy.SetMinimumValue(0m);
            aaPolicy.SetMaximumValue(5m);
            aaPolicy.SetRecommendationLowValue(2m);
            aaPolicy.SetRecommendationHighValue(4m);

            var aaPercentagePolicy = new OrderFactorInstantPolicy(aa, DemoFluidOrderOne, 100m, routeList, patientTypeLIst);
            //aaPercentagePolicy.SetMinimumValue(0.25m);
            aaPercentagePolicy.SetMaximumValue(10m);
            aaPercentagePolicy.SetRecommendationLowValue(2m);
            aaPercentagePolicy.SetRecommendationHighValue(4m);

            var sodiumPolicy = new FactorWeightDurationPolicy(sodium, new DurationValue(Duration.Day, 1), routeList, patientTypeLIst, Mass.Kilogram);
            sodiumPolicy.SetMinimumValue(0m);
            sodiumPolicy.SetMaximumValue(4m);
            sodiumPolicy.SetRecommendationLowValue(0m);
            sodiumPolicy.SetRecommendationHighValue(1m);

            var sodiumDailyPolicy = new FactorDurationPolicy(sodium, new DurationValue(Duration.Day, 1), routeList, patientTypeLIst);
            sodiumDailyPolicy.SetMinimumValue(0m);
            sodiumDailyPolicy.SetMaximumValue(8m);
            sodiumDailyPolicy.SetRecommendationLowValue(0m);
            sodiumDailyPolicy.SetRecommendationHighValue(2m);

            var glucoseDailyRecommendationPolicy = new FactorDurationPolicy(glucose, new DurationValue(Duration.Day, 1), routeList, patientTypeLIst);
            //glucoseDailyRecommendationPolicy.SetMinimumValue(0m);
            // glucoseDailyRecommendationPolicy.SetMaximumValue(20m);
            glucoseDailyRecommendationPolicy.SetRecommendationLowValue(0m);
            glucoseDailyRecommendationPolicy.SetRecommendationHighValue(2m);

            var heparinPolicy = new OrderFactorInstantPolicy(heparin, DemoFluidOrderOne, 1m, routeList, patientTypeLIst);
            heparinPolicy.SetMinimumValue(0.25m);
            //heparinPolicy.SetMaximumValue(2m);
            //heparinPolicy.SetRecommendationLowValue(0.5m);
            //heparinPolicy.SetRecommendationHighValue(1m);

            var cystienePolicy = new FactorFactorInstantPolicy(cysteine, aa, 1m, routeList, patientTypeLIst);
            cystienePolicy.SetMaximumValue(100m);
            cystienePolicy.SetRecommendationHighValue(40m);
            cystienePolicy.SetRecommendationLowValue(20m);


            Policies.Add(aaPolicy);
            Policies.Add(aaPercentagePolicy);
            Policies.Add(sodiumPolicy);
            Policies.Add(sodiumDailyPolicy);
            Policies.Add(glucoseDailyRecommendationPolicy);
            Policies.Add(heparinPolicy);
            Policies.Add(cystienePolicy);

            #endregion


            DemoPatient.Orders.Add(DemoFluidOrderItemOne);
            DemoPatient.Orders.Add(DemoFluidOrderItemTwo);




        }
    }
}
