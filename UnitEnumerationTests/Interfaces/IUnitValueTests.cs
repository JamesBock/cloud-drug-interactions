using Microsoft.VisualStudio.TestTools.UnitTesting;
using UWPLockStep.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using UWPLockStep.Domain.Common;
using UWPLockStep.Domain.Common.Units;

namespace UWPLockStep.Domain.Interfaces.Tests
{
    [TestClass()]
    public class IUnitValueTests
    {
        UnitValueBase Left1 = UnitValueFactory.Create(UnitFactory.Create(UnitValueFactory.Create(Mass.Pound, 10), UnitValueFactory.Create(Volume.Milliliter, 100)), 2);
        
        UnitValueBase Right1 = UnitValueFactory.Create(UnitFactory.Create(UnitValueFactory.Create(Volume.Milliliter, 100), UnitValueFactory.Create(Mass.Pound, 10)), 19.9m);

        [TestMethod()]
        public void CompareToTest()//two PolyUnitValues with the same translation rules have differnet Units projected and slightly different Amounts
        {
            var expected = true;
            var result = Left1 > Right1;

            Assert.AreEqual(expected, result);
        }
    }
}