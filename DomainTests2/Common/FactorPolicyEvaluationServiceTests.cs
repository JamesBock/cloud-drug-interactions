using Microsoft.VisualStudio.TestTools.UnitTesting;
using UWPLockStep.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;
using UWPLockStep.Persistance;
using UWPLockStep.Domain.Entities.Factors;
using UWPLockStep.Domain.Common.Units;

namespace UWPLockStep.Domain.Common.Tests
{
    [TestClass()]
    public class FactorPolicyEvaluationServiceTests
    {    
        [TestMethod()]
        public void ToStringTest()
        {
            var database = new DemoData();
            var aa = new Factor(new MonoUnit(Mass.Gram)) { Name = "Amino Acids" };
            var eval = new FactorPolicyEvaluationService(aa, database.Policies, database.DemoPatient, database.DemoFluidOrderItemOne);

            Assert.AreEqual(eval.ToString(), "Amino Acids: 6.5g");
        }
    }
}