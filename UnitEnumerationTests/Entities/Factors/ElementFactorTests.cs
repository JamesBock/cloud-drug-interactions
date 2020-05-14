using Microsoft.VisualStudio.TestTools.UnitTesting;
using UWPLockStep.Domain.Entities.Factors;
using System;
using System.Collections.Generic;
using System.Text;
using UWPLockStep.Domain.Common.Units;

namespace UWPLockStep.Domain.Entities.Factors.Tests
{
    [TestClass()]
    public class ElementFactorTests
    {
        [TestMethod()]
        public void ElementFactorTestSodium()
        {
            
            var ele = new ElementFactor("Sodium");
                       
            Assert.AreEqual(22.989769m, ele.ElementObject.AtomicMass);
        }

        [TestMethod()]
        public void ElementFactorTestCalcium()
        {

            var ele = new ElementFactor("Calcium");
            Assert.AreEqual(40.078m, ele.ElementObject.AtomicMass);

            //ele.MultiDimensionalUnit.C
        }
    }
}