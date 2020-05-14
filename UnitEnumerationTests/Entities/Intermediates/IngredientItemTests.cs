using Microsoft.VisualStudio.TestTools.UnitTesting;
using UWPLockStep.Domain.Entities.Intermediates;
using System;
using System.Collections.Generic;
using System.Text;
using UWPLockStep.Domain.Entities.Orders;
using UWPLockStep.Domain.Entities.Ingredients;
using UWPLockStep.Domain.Common.Units;
using UWPLockStep.Domain.Entities.Factors;
using UWPLockStep.Domain.Entities.People;
using System.Linq;

namespace UWPLockStep.Domain.Entities.Intermediates.Tests
{
    [TestClass()]
    public class IngredientItemTests
    {/// <summary>
    /// this simulates the selected patient having a FluidOrder ordered for them.
    /// </summary>
        [TestMethod()]
        public void IngredientItemTest()
        {
            //Asseble
            var patient = new LockStepPatient(new DateTime(2001, 5, 5), new DateTime(2000, 10, 20), new List<string>() { "John" }, "Doe");
            var ord = new FluidOrder(new MonoUnit(Volume.Milliliter));//When this is changed to Mass
            var ordItem = new OrderItem(patient, ord, 50m);
            var naCl = new Ingredient(new MonoUnit(Volume.Milliliter)) { Id = Guid.NewGuid() };
            var ele = new ElementFactor("Sodium") { Id = Guid.NewGuid() };
            var facItem = new FactorItem(ele, naCl, .5m);
            naCl.FactorItems.Add(facItem);
            var ingItem = new IngredientItem(naCl, ord, 10m, true);

            //Act
            var expected = 250m;

            //ingItem.IngredientAmount.Convert(Volume.Liter);//If this is strictly fixed to and Order, there is no way for the User to modify this unit after it is set. The User can potentially change the unit of the Ingredient or the Order that gets passed into the IngredientItem
            
            ordItem.OrderUnitValue.Convert(Volume.Ounce);//This is inteneded to trip up the GetTotal method but should not change the actual answer
            
            var actual = facItem.GetTotal(ingItem.GetTotal(ordItem.OrderUnitValue)).Amount;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void IngredientItemMultipleFactorsTest()
        {
            //Assemble
            var patient = new LockStepPatient(new DateTime(2001,5,5),new DateTime(2000,10,20), new List<string>() { "John" }, "Doe") ; 
            var ord = new FluidOrder(new MonoUnit(Volume.Milliliter)) { Id = Guid.NewGuid() }; 
            var naCl = new Ingredient(new MonoUnit(Volume.Milliliter)) { Id = Guid.NewGuid() };
            var naCa = new Ingredient(new MonoUnit(Volume.Milliliter)) { Id = Guid.NewGuid() };
            var sodium = new ElementFactor("Sodium") { Id = Guid.NewGuid() };
            var calcium = new ElementFactor("Calcium") { Id = Guid.NewGuid() };
            
            var facItem = new FactorItem(sodium, naCl, .5m);//Defining the amount of Factor in the Ingredient
            var facItem2 = new FactorItem(sodium, naCa, .8m);
            var facItem3 = new FactorItem(calcium, naCa, .5m);

            naCl.FactorItems.Add(facItem);
            naCa.FactorItems.Add(facItem2);
            naCa.FactorItems.Add(facItem3);

            var ingItem = new IngredientItem(naCl, ord, 10m, true);//Defining the amount of the Ingredient in the Order
            var ingItem2 = new IngredientItem(naCa, ord, 20m, true);

            ord.IngredientCollection.Add(ingItem);
            ord.IngredientCollection.Add(ingItem2);
            
            var ordItem = new OrderItem(patient, ord, 50m);//Defining the Amount of the Order
            
            patient.Orders.Add(ordItem);
            
            //Act
            var expectedSum = 1050m;

            //ingItem.IngredientAmount.Convert(Volume.Liter);//If this is strictly fixed to an Order, there is no way for the User to modify this unit after it is set. The User can potentially change the unit of the Ingredient or the Order that gets passed into the IngredientItem

            ordItem.OrderUnitValue.Convert(Volume.Ounce);//This is inteneded to trip up the GetTotal method but should not change the actual answer
            //need to calculate FactorItem total from the IngredientItem because it can integrate the OrderAmount.
            
            //this returns an IEnumerable of IngredientItems that contain the specified Id.
            var ingredientList = patient.Orders.SelectMany(i => i.IngredientItems).Where(f => f.Ingredient.FactorItems.Any(f => f.FactorId == sodium.Id));

            //this returns an IEnumerable of factorItems that contain the specified Id.
            var factorList = patient.Orders.SelectMany(i => i.IngredientItems.SelectMany(f => f.Ingredient.FactorItems).Where(f => f.FactorId == sodium.Id));

            //this returns the Sum of the GetTotal methods for the FactorItem and the IngredientItem matching IngredientId. This has the only Property that needs an actualy EntityObject and not just an Id (OrderUnitValue). This could be changed but not needed right now.        
            var actualAgg = factorList.Sum(f => f.GetTotal(ingredientList.Single(i => i.Ingredient.Id == f.IngredientId).GetTotal(ordItem.OrderUnitValue)).Amount);

            var actualAgg2 = factorList.Aggregate(0m, (f, next) => f + next.GetTotal(ingredientList.Single(i => i.Ingredient.Id == next.IngredientId).GetTotal(ordItem.OrderUnitValue)).Amount);

            //Assert

            Assert.AreEqual(expectedSum, actualAgg);
            Assert.AreEqual(expectedSum, actualAgg2);
        }
    }
}