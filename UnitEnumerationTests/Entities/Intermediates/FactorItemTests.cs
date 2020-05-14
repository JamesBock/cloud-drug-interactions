using Microsoft.VisualStudio.TestTools.UnitTesting;
using UWPLockStep.Domain.Entities.Intermediates;
using System;
using System.Collections.Generic;
using System.Text;
using UWPLockStep.Domain.Entities.Orders;
using UWPLockStep.Domain.Entities.Ingredients;
using UWPLockStep.Domain.Common.Units;
using UWPLockStep.Domain.Entities.Factors;

namespace UWPLockStep.Domain.Entities.Intermediates.Tests
{
    [TestClass()]
    public class FactorItemTests
    {
    //    [TestMethod()]
    //    public void GetFactorTotalIntegrationTest()//this will not execute on its own. Its functionality dependeds on the Unit above it and as they changed, this had to change and ended up having to be the same as the IngredientItemTest
    //    {
    //        ////Asseble
    //        //var ord = new FluidOrder(new MonoUnit(Volume.Liter));
    //        //var naCl = new Ingredient(new MonoUnit(Volume.Milliliter)) { Id = Guid.NewGuid() };
    //        //var ele = new ElementFactor("Sodium") { Id = Guid.NewGuid() };
    //        //var fac = new FactorItem(ele, naCl, .5m);
    //        //naCl.FactorItems.Add(fac);
    //        //var ingItem = new IngredientItem(naCl, ord, 10m);

    //        ////Act
    //        //var expected = 5m;
    //        //ingItem.IngredientAmount.Convert(Volume.Liter);//this converts the amount of the Ingredientitem 
            
    //        ////ingItem.IngredientAmount.Unit.Convert(ingItem.IngredientAmount.Amount, Volume.Liter);//this does not change the Amount Prop of the ingItem but changes the UNit...the math is right but the units are wrong
    //        //var actual = fac.GetFactorTotal(ingItem.CreateIngredientTotal(ord.).Amount;
    //        ////var actual = ingItem.GetFactorTotal(ele);
            
    //        ////Assert
    //        //Assert.AreEqual(expected, actual);
    //    }
    }
}