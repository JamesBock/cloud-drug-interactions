using Microsoft.VisualStudio.TestTools.UnitTesting;
using UWPLockStep.Domain.Entities.Policies;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using UWPLockStep.Domain.Common;
using UWPLockStep.Domain.Common.State.PatientType;
using UWPLockStep.Domain.Entities.People;
using UWPLockStep.Domain.Entities.Orders;
using UWPLockStep.Domain.Common.Units;
using UWPLockStep.Domain.Entities.Ingredients;
using UWPLockStep.Domain.Entities.Factors;
using UWPLockStep.Domain.Entities.Intermediates;
using System.Linq;
using UWPLockStep.Domain.Entities.Orders.States;

namespace UWPLockStep.Domain.Entities.Policies.Tests
{
    [TestClass()]
    public class PatientBasisDurationPolicyTests
    {
        [TestMethod()]
        public void EvaluatePolicyWithExpiredOrderTest()
        {

            //Assemble
            var routeList = new Collection<AdministrationRouteEnumeration>();
            routeList.Add(InjectionRoute.IVCentral);
            var patientTypeLIst = new Collection<PatientType>();
            patientTypeLIst.Add(new EarlyNeonate());

            var patient = new LockStepPatient(new DateTime(2001, 5, 5), new DateTime(2000, 10, 20), new List<string>() { "John" }, "Doe") { Weight= new MonoUnitValue(new MonoUnit(Mass.Kilogram), 1.390m) };
            var ord = new FluidOrder(new MonoUnit(Volume.Milliliter)) { Id = Guid.NewGuid() };
            var ord2 = new FluidOrder(new MonoUnit(Volume.Liter)) { Id = Guid.NewGuid() };
            var lCystieneHCL = new Ingredient(new MonoUnit(Volume.Milliliter)) { Name = "L-Cystiene HCl", Id = Guid.NewGuid() };
            var cysteine = new Factor(new MonoUnit(Mass.Milligram)) { Name = "Cystiene", Id = Guid.NewGuid() };

            var aa = new Factor(new MonoUnit(Mass.Gram)) { Name = "Amino Acids", Id = Guid.NewGuid() };
            var trophamine = new Ingredient(new MonoUnit(Volume.Milliliter)) { Name = "Trophamine", Id = Guid.NewGuid() };
            var renalAmine = new Ingredient(new MonoUnit(Volume.Milliliter)) { Name = "Renal AAs", Id = Guid.NewGuid() };

            var facItem = new FactorItem(cysteine, lCystieneHCL, 50m);//Defining the amount of Factor in the Ingredient
            var facItem2 = new FactorItem(aa, trophamine, .1m);
            //var facItem3 = new FactorItem(aa, renalAmine, .1m);

            lCystieneHCL.FactorItems.Add(facItem);
            trophamine.FactorItems.Add(facItem2);
            //renalAmine.FactorItems.Add(facItem3);

            var ingItem = new IngredientItem(lCystieneHCL, ord, .01798507m, true);//Simulating the User entering the amount of the Ingredient in the Order. Unclear what the EditibleValue Property does.
            var trophAA = new IngredientItem(trophamine, ord, .3m, true);
            //var renAmine = new IngredientItem(renalAmine, ord, .3m, true);
            ord.IngredientCollection.Add(ingItem);
            ord.IngredientCollection.Add(trophAA);
            //ord.IngredientCollection.Add(renAmine);

            //var ingItem2 = new IngredientItem(lCystieneHCL, ord2,  17.98507m, true);//The FactorItem on the Ingredient is setup so there is 50mg per 1mL. Since ordItem2 is in Liters and the decimal Amount on the ingItem2 does not have a Unit it does not scale as was expected but it is unclear how the User will set this value. If the User knows the scale is Liters and not mL when ord2 was ordered, the number would be different. IngredientItem's Amount is still a bit arbitrary, it may need a Unit attached to it?
            //var trophAA2 = new IngredientItem(trophamine, ord2, 300m, true);
            //var renAmine2 = new IngredientItem(renalAmine, ord2, 300m, true);
            //ord2.IngredientCollection.Add(ingItem2);
            //ord2.IngredientCollection.Add(trophAA2);
            //ord2.IngredientCollection.Add(renAmine2);


            var ordItem = new FluidOrderItem(patient, ord, 168m, new DurationValue(Duration.Day, 1), 1);//Defining the Amount of the Order that is dosed to the Patient
            //var ordItem2 = new OrderItem(patient, ord2, .268m);//Defining the Amount of the Order
            var currentOrderItem = new FluidOrderItem(patient, ord, 0, new DurationValue(Duration.Day, 1), 1);
            patient.Orders.Add(ordItem);
            //patient.Orders.Add(ordItem2);

            //ordItem.OrderDuration = new DurationValue(Duration.Hour, 24);
            ordItem.TimeExecuted = new DateTime(2020, 1, 1, 5, 0, 0);
            ordItem.TransitionToState(new ActiveState());

            //ordItem2.OrderDuration = new DurationValue(Duration.Hour, 24);
            //ordItem2.TimeExecuted = new DateTime(2020, 1, 1, 5, 0, 0);

            //currentOrderItem.OrderDuration = new DurationValue(Duration.Hour, 24);
            //currentOrderItem.TimeExecuted = DateTime.Now;
            currentOrderItem.TimeOrdered = new DateTime(2020, 1, 1, 17, 0, 0);

            //how will the User communicate the Basis as Weight?
            var policy = new PatientBasisDurationPolicy(5m, aa,  new DurationValue(Duration.Day, 1) , routeList, patientTypeLIst);

            var policyCheck = policy.EvaluatePolicy(patient, currentOrderItem);//this should return half of the amount from the key substracted from the Policy limit

            var expectedPolicyAmount =6.95m /*- (8.04m * (168m/268m)*/ /*this is the percentage of the amount in FluidOrder and what Patient gets)/2m*/;

            Assert.AreEqual(expectedPolicyAmount, policyCheck.Amount);
            
        }

        [TestMethod()]//this test uses old POlicies
        public void EvaluatePolicyWithActiveOrderTest()
        {

            //Assemble
            var routeList = new Collection<AdministrationRouteEnumeration>();
            routeList.Add(InjectionRoute.IVCentral);
            var patientTypeLIst = new Collection<PatientType>();
            patientTypeLIst.Add(new EarlyNeonate());

            var patient = new LockStepPatient(new DateTime(2001, 5, 5), new DateTime(2000, 10, 20), new List<string>() { "John" }, "Doe") { Weight = new MonoUnitValue(new MonoUnit(Mass.Kilogram), 1.390m) };
            var ord = new FluidOrder(new MonoUnit(Volume.Milliliter)) { Id = Guid.NewGuid() };
            var ord2 = new FluidOrder(new MonoUnit(Volume.Liter)) { Id = Guid.NewGuid() };
            var lCystieneHCL = new Ingredient(new MonoUnit(Volume.Milliliter)) { Name = "L-Cystiene HCl", Id = Guid.NewGuid() };
            var cysteine = new Factor(new MonoUnit(Mass.Milligram)) { Name = "Cystiene", Id = Guid.NewGuid() };

            var aa = new Factor(new MonoUnit(Mass.Gram)) { Name = "Amino Acids", Id = Guid.NewGuid() };
            var trophamine = new Ingredient(new MonoUnit(Volume.Milliliter)) { Name = "Trophamine", Id = Guid.NewGuid() };
            var renalAmine = new Ingredient(new MonoUnit(Volume.Milliliter)) { Name = "Renal AAs", Id = Guid.NewGuid() };

            var facItem = new FactorItem(cysteine, lCystieneHCL, 50m);//Defining the amount of Factor in the Ingredient
            var facItem2 = new FactorItem(aa, trophamine, .1m);
            //var facItem3 = new FactorItem(aa, renalAmine, .1m);

            lCystieneHCL.FactorItems.Add(facItem);
            trophamine.FactorItems.Add(facItem2);
            //renalAmine.FactorItems.Add(facItem3);

            var ingItem = new IngredientItem(lCystieneHCL, ord, .01798507m, true);//Simulating the User entering the amount of the Ingredient in the Order. Unclear what the EditibleValue Property does.
            var trophAA = new IngredientItem(trophamine, ord, .3m, true);
            //var renAmine = new IngredientItem(renalAmine, ord, .3m, true);
            ord.IngredientCollection.Add(ingItem);
            ord.IngredientCollection.Add(trophAA);
            //ord.IngredientCollection.Add(renAmine);

            //var ingItem2 = new IngredientItem(lCystieneHCL, ord2,  17.98507m, true);//The FactorItem on the Ingredient is setup so there is 50mg per 1mL. Since ordItem2 is in Liters and the decimal Amount on the ingItem2 does not have a Unit it does not scale as was expected but it is unclear how the User will set this value. If the User knows the scale is Liters and not mL when ord2 was ordered, the number would be different. IngredientItem's Amount is still a bit arbitrary, it may need a Unit attached to it?
            //var trophAA2 = new IngredientItem(trophamine, ord2, 300m, true);
            //var renAmine2 = new IngredientItem(renalAmine, ord2, 300m, true);
            //ord2.IngredientCollection.Add(ingItem2);
            //ord2.IngredientCollection.Add(trophAA2);
            //ord2.IngredientCollection.Add(renAmine2);


            var ordItem = new FluidOrderItem(patient, ord, 168m, new DurationValue(Duration.Day, 1), 1);//Defining the Amount of the Order that is dosed to the Patient
            //var ordItem2 = new OrderItem(patient, ord2, .268m);//Defining the Amount of the Order
            var currentOrderItem = new FluidOrderItem(patient, ord, 0, new DurationValue(Duration.Day, 1), 1);
            patient.Orders.Add(ordItem);
            //patient.Orders.Add(ordItem2);

            ordItem.OrderDuration = new DurationValue(Duration.Hour, 24);
            //ordItem.FlowRate.GetVolume(ordItem);
            ordItem.ChangeRate(9m);
            ordItem.ChangeRate(7m);
            ordItem.TimeExecuted = DateTime.Now.Subtract(TimeSpan.FromHours(12));
            ordItem.TransitionToState(new ActiveState());

            //ordItem2.OrderDuration = new DurationValue(Duration.Hour, 24);
            //ordItem2.TimeExecuted = new DateTime(2020, 1, 1, 5, 0, 0);

            currentOrderItem.OrderDuration = new DurationValue(Duration.Hour, 24);
            currentOrderItem.TimeExecuted = DateTime.Now;
            currentOrderItem.TimeOrdered = DateTime.Now;

            //how will the User communicate the Basis as Weight?
            var policy = new PatientBasisDurationPolicy(5m, aa, new DurationValue(Duration.Day, 1), routeList, patientTypeLIst);

            var policyCheck = policy.EvaluatePolicy(patient, currentOrderItem);//this should return half of the amount from the key substracted from the Policy limit

            var expectedPolicyAmount = 6.95m - (8.04m * (168m / 268m) /*this is the percentage of the amount in FluidOrder and what Patient gets*/ )/2m /*dividing by 2 because the Order in the ActiveState is half over */;

            Assert.AreEqual(expectedPolicyAmount, Math.Round(policyCheck.Amount,4));

        }
    }
}