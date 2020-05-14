using Microsoft.VisualStudio.TestTools.UnitTesting;
using UWPLockStep.Domain.Common.Units;
using System;
using System.Collections.Generic;
using System.Text;

namespace UWPLockStep.Domain.Common.Units.Tests
{
    [TestClass()]
    public class DerivedRatioValueTests
    {
        [TestMethod()]
        public void ShowDenominatorDimensionTest()
        {
            //Assemble
            var derivedUnit = new DerivedRatioUnit(Mass.Gram, AmountOfElement.Mole, 22m);
            var derivedValue = new DerivedRatioValue(derivedUnit,44m);

            //Act
            derivedValue.ShowDenominatorDimension();
            derivedValue.ShowNumeratorDimension();
            //Assert
            Assert.AreEqual(derivedValue.Amount,44m);
            Assert.AreEqual(derivedValue.Unit.Unit, Mass.Gram);
            
        }
    }
}