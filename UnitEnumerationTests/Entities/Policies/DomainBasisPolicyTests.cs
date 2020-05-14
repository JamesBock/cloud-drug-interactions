using Microsoft.VisualStudio.TestTools.UnitTesting;
using UWPLockStep.Domain.Entities.Policies;
using System;
using System.Collections.Generic;
using System.Text;
using UWPLockStep.Domain.Entities.People;
using UWPLockStep.Domain.Common.Units;
using UWPLockStep.Domain.Entities.Orders;
using UWPLockStep.Domain.Entities.Ingredients;
using UWPLockStep.Domain.Entities.Factors;
using UWPLockStep.Domain.Entities.Intermediates;
using System.Linq;
using UWPLockStep.Domain.Common;
using UWPLockStep.ApplicationLayer.Services;
using System.Collections.ObjectModel;
using UWPLockStep.Domain.Common.State.PatientType;

namespace UWPLockStep.Domain.Entities.Policies.Tests
{
    [TestClass()]
    public class DomainBasisPolicyTests
    {
        [TestMethod()]
        public void GetPolicyValueTest()
        {
            //Assemble
            var routeList = new Collection<AdministrationRouteEnumeration>
            {
                InjectionRoute.IVCentral
            };
            var patientTypeLIst = new Collection<PatientType>
            {
                new EarlyNeonate()
            };

            var patient = new LockStepPatient(new DateTime(2001, 5, 5), new DateTime(2000, 10, 20), new List<string>() { "John" }, "Doe");
            var ord = new FluidOrder(new MonoUnit(Volume.Milliliter)) { Id = Guid.NewGuid() };
            var naCl = new Ingredient(new MonoUnit(Volume.Milliliter)) { Id = Guid.NewGuid() };
            //var naCa = new Ingredient(new MonoUnit(Volume.Milliliter)) { Id = Guid.NewGuid() };
            var sodium = new ElementFactor("Sodium") { Id = Guid.NewGuid() };
            var calcium = new ElementFactor("Calcium") { Id = Guid.NewGuid() };

            var facItem = new FactorItem(sodium, naCl, .70m);//Defining the amount of Factor in the Ingredient
            //var facItem2 = new FactorItem(sodium, naCa, .8m);
            //var facItem3 = new FactorItem(calcium, naCa, .5m);

            naCl.FactorItems.Add(facItem);
           // naCa.FactorItems.Add(facItem2);
            //naCa.FactorItems.Add(facItem3);

            var ingItem = new IngredientItem(naCl, ord, .17858209m, true);//Simulating the User entering the amount of the Ingredient in the Order. Unclear what the EditibleValue Property does.
            //var ingItem2 = new IngredientItem(naCa, ord, 20m, true);

            ord.IngredientCollection.Add(ingItem);
            //ord.IngredientCollection.Add(ingItem2);

            var ordItem = new OrderItem(patient, ord, 268.00m);//Defining the Amount of the Order

            patient.Orders.Add(ordItem);

            var policy = new HeavyFactorTargetPolicy(.125m, sodium, ord, routeList, patientTypeLIst);
            var policy2 = new HeavyFactorTargetPolicy(.125m, sodium, ord, routeList, patientTypeLIst);
            var policyEval = new PolicyEvaluator(policy);

            var policyCheck = policyEval.EvaluatePolicy(ordItem);
          //  var policyCheck2 = policy.GetPolicyValue(ordItem);

            var ingredientList = patient.Orders.SelectMany(i => i.IngredientItems).Where(f => f.Ingredient.FactorItems.Any(f => f.FactorId == sodium.Id));
            
            //var intAggPercent = ingredientList.Sum(i => i.GetTotalPercent(ordItem.OrderUnitValue).Amount);

            //this is returning the Amount of the Ingredient in terms of its Unit  defined in the IngredientItem
            var intAgg = ingredientList.Sum(i => i.GetTotal(ordItem.OrderUnitValue).Amount);

            //this returns an IEnumerable of factorItems that contain the specified Id.
            var factorList = patient.Orders.SelectMany(i => i.IngredientItems.SelectMany(f => f.Ingredient.FactorItems).Where(f => f.FactorId == sodium.Id));

            
            //var actualAggPercent = factorList.Sum(f => f.GetTotal(ingredientList.Single(i => i.Ingredient.Id == f.IngredientId).GetTotalPercent(ordItem.OrderUnitValue)).Amount);
            
            //this returns the Sum of the GetTotal methods for the FactorItem and the IngredientItem matching IngredientId. This has the only Property that needs an actualy EntityObject and not just an Id (OrderUnitValue). This could be changed but not needed right now.          
            var actualAgg = factorList.Sum(f => f.GetTotal(ingredientList.Single(i => i.Ingredient.Id == f.IngredientId).GetTotal(ordItem.OrderUnitValue)).Amount);


          
        }

        [TestMethod()] // this uses old Policies
        public void GetPolicyValueCystieneTest()
        {
            //Assemble
            var routeList = new Collection<AdministrationRouteEnumeration>
            {
                InjectionRoute.IVCentral
            };
            var patientTypeLIst = new Collection<PatientType>
            {
                new EarlyNeonate()
            };

            var patient = new LockStepPatient(new DateTime(2001, 5, 5), new DateTime(2000, 10, 20), new List<string>() {"John"}, "Doe");
            var ord = new FluidOrder(new MonoUnit(Volume.Milliliter)) { Id = Guid.NewGuid() };
            var lCystieneHCL = new Ingredient(new MonoUnit(Volume.Milliliter)) {Name = "L-Cystiene HCl", Id = Guid.NewGuid() };
            var cysteine = new Factor(new MonoUnit(Mass.Milligram)) {Name = "Cystiene", Id = Guid.NewGuid() };

            var aa = new Factor(new MonoUnit(Mass.Gram)) {Name = "Amino Acids", Id = Guid.NewGuid() };
            var trophamine = new Ingredient(new MonoUnit(Volume.Milliliter)) {Name = "Trophamine", Id = Guid.NewGuid() };
            var renalAmine = new Ingredient(new MonoUnit(Volume.Milliliter)) { Name = "Renal AAs", Id = Guid.NewGuid() };
            
            var facItem = new FactorItem(cysteine, lCystieneHCL, 50m);//Defining the amount of Factor in the Ingredient
            var facItem2 = new FactorItem(aa, trophamine, .1m);
            var facItem3 = new FactorItem(aa, renalAmine, .1m);
            
            lCystieneHCL.FactorItems.Add(facItem);
            trophamine.FactorItems.Add(facItem2);
            renalAmine.FactorItems.Add(facItem3);

            var ingItem = new IngredientItem(lCystieneHCL, ord, .01798507m, true);//Simulating the User entering the amount of the Ingredient in the Order. Unclear what the EditibleValue Property does.
            var trophAA = new IngredientItem(trophamine, ord, .3m, true);
            var renAmine = new IngredientItem(renalAmine, ord, .3m, true);
            ord.IngredientCollection.Add(ingItem);
            ord.IngredientCollection.Add(trophAA);
            ord.IngredientCollection.Add(renAmine);

            var ordItem = new CompoundedFluidOrderItem(patient, ord, 168m, new DurationValue(Duration.Hour, 24), 1, UnitValueFactory.Create(Volume.Milliliter, 100m));//Defining the Amount of the Order

            patient.Orders.Add(ordItem);

            var policy = new HeavyFactorTargetPolicy(30m, cysteine, aa, routeList, patientTypeLIst);

            var policyCheck = policy.EvaluatePolicy(ordItem);
             
            var expectedPolicyAmount = 482.40m;//Double the cystiene because there are two ingredients that have cystiene

            var ingredientList = patient.Orders.SelectMany(i => i.IngredientItems).Where(f => f.Ingredient.FactorItems.Any(f => f.FactorId == aa.Id));


            //this is returning the Amount of the Ingredient in terms of its Unit  defined in the IngredientItem
            var intAgg = ingredientList.Sum(i => i.GetTotal(ordItem.OrderUnitValue).Amount);

            //this returns an IEnumerable of factorItems that contain the specified Id.
            var factorList = patient.Orders.SelectMany(i => i.IngredientItems.SelectMany(f => f.Ingredient.FactorItems).Where(f => f.FactorId == aa.Id));


            //this returns the Sum of the GetTotal methods for the FactorItem and the IngredientItem matching IngredientId. This has the only Property that needs an actualy EntityObject and not just an Id (OrderUnitValue). This could be changed but not needed right now.          
            var actualAgg = factorList.Sum(f => f.GetTotal(ingredientList.Single(i => i.Ingredient.Id == f.IngredientId).GetTotal(ordItem.OrderUnitValue)).Amount);


            Assert.AreEqual(expectedPolicyAmount, policyCheck.Amount);
        }


        [TestMethod()]
        public void GetPolicyValueConcentrationTest()
        {
            //Assemble
            var routeList = new Collection<AdministrationRouteEnumeration>();
            routeList.Add(InjectionRoute.IVCentral);
            var patientTypeLIst = new Collection<PatientType>();
            patientTypeLIst.Add(new EarlyNeonate());


            var patient = new LockStepPatient(new DateTime(2001, 5, 5), new DateTime(2000, 10, 20), new List<string>() { "John" }, "Doe");
            var ord = new FluidOrder(new MonoUnit(Volume.Milliliter)) { Id = Guid.NewGuid() };
            var d70 = new Ingredient(new MonoUnit(Volume.Milliliter)) { Id = Guid.NewGuid() };

            var dextrose = new Factor(new MonoUnit(Mass.Gram)) { Id = Guid.NewGuid() };


            var facItem = new FactorItem(dextrose, d70, .7m);//Defining the amount of Factor in the Ingredient
            
            d70.FactorItems.Add(facItem);
            

            var ingItem = new IngredientItem(d70, ord, .01798507m, true);//Simulating the User entering the amount of the Ingredient in the Order. Unclear what the EditibleValue Property does.
            

            ord.IngredientCollection.Add(ingItem);
           
            //TODO: this test was not written with overfill in mind. 
            var ordItem = new CompoundedFluidOrderItem(patient, ord, 168m, new DurationValue(Duration.Hour,24), 1 ,UnitValueFactory.Create(Volume.Milliliter, 100m));//Defining the Amount of the Order

            patient.Orders.Add(ordItem);

            var policy = new HeavyFactorTargetPolicy(.125m, dextrose, ord, routeList, patientTypeLIst);
           

            var policyCheck = policy.EvaluatePolicy(ordItem);
           
            var expectedPolicyAmount = 33.50m;

            Assert.AreEqual(expectedPolicyAmount, policyCheck.Amount);
        }
    }
}