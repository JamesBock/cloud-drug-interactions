using Microsoft.VisualStudio.TestTools.UnitTesting;
using UWPLockStep.Domain.Entities.Intermediates;
using System;
using System.Collections.Generic;
using System.Text;
using UWPLockStep.Domain.Common.Units;
using UWPLockStep.Domain.Entities.People;
using UWPLockStep.Domain.Entities.Orders;

namespace UWPLockStep.Domain.Entities.Intermediates.Tests
{
    [TestClass()]
    public class FluidOrderItemTests
    {
        [TestMethod()]
        public void FluidOrderItemTest()
        {
            var patient = new LockStepPatient(new DateTime(2001, 5, 5), new DateTime(2000, 10, 20), new List<string>() { "John" }, "Doe") { Weight = new MonoUnitValue(new MonoUnit(Mass.Kilogram), 1.390m) };

            var ord = new FluidOrder(new MonoUnit(Volume.Milliliter)) { Id = Guid.NewGuid() };

            var ordItem = new FluidOrderItem(patient, ord, 169m, new DurationValue(Duration.Hour, 24), 1);//Defining the Amount of the Order that is dosed to the Patient

            

            Assert.AreEqual(168m, ordItem.OrderUnitValue.Amount);
        }
    }
}