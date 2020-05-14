using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using UWPLockStep.Persistance;
using Xunit;

namespace ApplicationLayerXUnitTest
{
    public class CheckExpirationShould
    {
        public DemoData Database { get; set; }
        [Fact] //This test is bad. 
        public void ExpireWhenDatePassed()
        {

            //Assemble

            var routeList = new Collection<AdministrationRouteEnumeration>
            {
                InjectionRoute.IVCentral
            };
            var patientTypeLIst = new Collection<PatientTypeStateless.PatientTypeState>
            {
                PatientTypeStateless.PatientTypeState.Neonate,
                PatientTypeStateless.PatientTypeState.EarlyNeonate,
            };

            Database = new DemoData();

            var patient = new LockStepPatient(new DateTime(2001, 5, 5), new DateTime(2000, 10, 20), new List<string>() { "John" }, "Doe") { Weight = new MonoUnitValue(new MonoUnit(Mass.Kilogram), 1.390m) };

            var ord = new FluidOrder(new MonoUnit(Volume.Milliliter)) { Id = Guid.NewGuid() };
            var ord2 = new FluidOrder(new MonoUnit(Volume.Milliliter)) { Id = Guid.NewGuid() };
            var lCystieneHCL = new Ingredient(new MonoUnit(Volume.Milliliter)) { Name = "L-Cystiene HCl", Id = Guid.NewGuid() };
            var cysteine = new Factor(new MonoUnit(Mass.Milligram)) { Name = "Cystiene", Id = Guid.NewGuid() };

            var aa = new Factor(new MonoUnit(Mass.Gram)) { Name = "Amino Acids", Id = Guid.NewGuid() };
            
            var trophamine = new Ingredient(new MonoUnit(Volume.Milliliter)) { Name = "Trophamine", Id = Guid.NewGuid() };
            var renalAmine = new Ingredient(new MonoUnit(Volume.Milliliter)) { Name = "Renal AAs", Id = Guid.NewGuid() };

            var facItem = new FactorItem(cysteine, lCystieneHCL, 50m);//Defining the amount of Factor in the Ingredient
            FactorItem facItem2 = new FactorItem(aa, trophamine, .1m);
            FactorItem facItem3 = new FactorItem(aa, renalAmine, .1m);

            lCystieneHCL.FactorItems.Add(facItem);
            trophamine.FactorItems.Add(facItem2);
            //renalAmine.FactorItems.Add(facItem3);

            var ingItem = new IngredientItem(lCystieneHCL, ord, .01798507m, true);//Simulating the User entering the amount of the Ingredient in the Order. Unclear what the EditibleValue Property does.
            var trophAA = new IngredientItem(trophamine, ord, .30m, true);
            var renAmine = new IngredientItem(renalAmine, ord, .3m, true);
            ord.IngredientCollection.Add(ingItem);
            ord.IngredientCollection.Add(trophAA);
            ord.IngredientCollection.Add(renAmine);

            var ingItem2 = new IngredientItem(lCystieneHCL, ord2,  .01798507m, true);//The FactorItem on the Ingredient is setup so there is 50mg per 1mL. Since ordItem2 is in Liters and the decimal Amount on the ingItem2 does not have a Unit it does not scale as was expected but it is unclear how the User will set this value. If the User knows the scale is Liters and not mL when ord2 was ordered, the number would be different. IngredientItem's Amount is still a bit arbitrary, it may need a Unit attached to it?
            var trophAA2 = new IngredientItem(trophamine, ord2, .300m, true);
            var renAmine2 = new IngredientItem(renalAmine, ord2, .300m, true);
            ord2.IngredientCollection.Add(ingItem2);
            ord2.IngredientCollection.Add(trophAA2);
            ord2.IngredientCollection.Add(renAmine2);


            var ordItem = new CompoundedFluidOrderItem(patient, ord, 168m, new DurationValue(Duration.Day, 1), 1, UnitValueFactory.Create(Volume.Milliliter, 100m));//Defining the Amount of the Order that is dosed to the Patient
            var ordItem2 = new CompoundedFluidOrderItem(patient, ord, 168m, new DurationValue(Duration.Day, 1), 1, UnitValueFactory.Create(Volume.Milliliter, 100m));

            var currentOrderItem = new OrderItem(patient, ord, 0);


            ordItem.OrderDuration = new DurationValue(Duration.Hour, 24);
            ordItem.TimeExecuted = new DateTime(2020, 1, 14, 5, 1, 1);

            ordItem2.OrderDuration = new DurationValue(Duration.Hour, 24);
            ordItem2.TimeExecuted = DateTime.Now.Subtract(TimeSpan.FromHours(12));
            ordItem.TransitionToState(new ActiveState());
            ordItem2.TransitionToState(new ActiveState());

            currentOrderItem.OrderDuration = new DurationValue(Duration.Hour, 24);
            currentOrderItem.TimeExecuted = DateTime.Now;
            currentOrderItem.TimeOrdered =  DateTime.Now;

            Database.DemoPatient.Orders.Add(ordItem);
            Database.DemoPatient.Orders.Add(ordItem2);
            Database.DemoPatient.Orders.Add(currentOrderItem);
            var decoyUnit = new MonoUnit(Mass.Gram);
            decoyUnit.Convert(2m, Mass.Pound);
            //aa.Unit.Convert(1, Mass.Pound);//this showed that the Unit object on the Factor that is passed around remained the same reference in all places it was passed. This is accessing the Convert method on the Unit which is only for use in the UnitValue class and does not adjust a value because it has none. 
            ordItem2.OrderUnitValue.Convert(Volume.Milliliter);
            var policy = new FactorWeightDurationPolicy(/*5m,*/ aa, new DurationValue(Duration.Day, 1), routeList, patientTypeLIst, Mass.Kilogram);
            policy.SetMaximumValue(5m);
            facItem3.FactorPerIngredientUnit.UnitPairs[aa.Id].Convert(Mass.Pound);//this dose not cause the test to fail...in fact when its uncommented, the calculation works even though its not added to the IngredientItem
            facItem2.FactorPerIngredientUnit.ConvertNumerator(Mass.Pound); //I believe this is causing the conversion of the policy because the Unit that is passed into the policy is the same one that eventually gets converted. When the decoyUnit is created and converted, it does not cause the PolicyUnit to change because it is a new Unit. Since the Policy is created with the aa Factor passed to it, the reference to that factor is the same where ever its passed so if the Unit changes in one place it changes everywhere.
            facItem2.FactorPerIngredientUnit.UnitPairs[aa.Id].Convert(Mass.Gram);
            var evalService = new FactorPolicyEvaluationService(aa, Database.Policies, Database.DemoPatient, currentOrderItem);
            
            var policyCheck = policy.EvaluateAgainstMaximum(evalService);
            var expectedPolicy = UnitValueFactory.Create(Mass.Gram, 6.95m)/* - (8.04m * (168m / 268m)*/ /*this is the percentage of the amount in FluidOrder and what Patient gets*//*) / 2m)*/; //Which is not what the test is written for. 

            Assert.Equal(expectedPolicy.Amount, policyCheck.Amount, 4);
            Assert.Equal(expectedPolicy.Unit.GetType(), policyCheck.Unit.GetType());
           
        }

      
    }
}
