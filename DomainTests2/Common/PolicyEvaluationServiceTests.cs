using Microsoft.VisualStudio.TestTools.UnitTesting;
using UWPLockStep.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

using UWPLockStep.Domain.Entities.Factors;
using UWPLockStep.Domain.Entities.Ingredients;
using UWPLockStep.Domain.Entities.Intermediates;
using UWPLockStep.Domain.Entities.Orders;
using UWPLockStep.Domain.Entities.Orders.States;
using UWPLockStep.Domain.Entities.People;
using UWPLockStep.Domain.Interfaces;


using System.Linq;
using UWPLockStep.Domain.Common.Units;
using UWPLockStep.Domain.Entities.Policies;
using UWPLockStep.Persistance;

namespace UWPLockStep.Domain.Common.Tests
{
    [TestClass()]
    public class PolicyEvaluationServiceTests
    {

        [TestMethod()]
        public void PolicyEvaluationServiceTest()
        {
            var db = new DemoData();

            var currentOrder = new CompoundedFluidOrderItem(db.DemoPatient, db.DemoFluidOrderOne, 100m, new DurationValue(Duration.Hour, 24), 1, UnitValueFactory.Create(Volume.Milliliter, 50m)) {AdministrationRoute = InjectionRoute.IVCentral, TimeExecuted = DateTime.Now };
           // currentOrder.IngredientItems.Select(ii => { ii.EditableUnitValue.Amount = .5m; return ii; }).ToList() ;
            //Does DraftState matter here?
           

            //this was going to query all the Factors on the Patient but that seems unnecessary be cause the Practitioner should only need to know about the Factors in the currentOrder
            var evalService = currentOrder.IngredientItems
                .SelectMany(ii => ii.Ingredient.FactorItems).GroupBy(f => f.ItemFactor).Select(f => new FactorPolicyEvaluationService(f.Key, db.Policies, db.DemoPatient, currentOrder)).ToList();

            ; 
            //.Select(f => new PolicyEvaluationService<FactorBase>(f.Key, db.Policies, db.DemoPatient, currentOrder))).ToList();

            Assert.Fail();
        }
    }
}