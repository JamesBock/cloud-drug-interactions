using Microsoft.VisualStudio.TestTools.UnitTesting;
//using NUnit.Framework;
using System;
using UWPLockStep.Domain.Common;
using UWPLockStep.Domain.Common.Exceptions;
using UWPLockStep.Domain.Common.Units;
using UWPLockStep.Domain.Entities.Policies;

namespace EnumerationTests
{
    [TestClass]
    public class UnitEnumerationTests
    {
        [DataRow("6", "0.202884")]
        [DataTestMethod]
        public void PolicyValueConvertMililitersToOunces(string amount, string expected)
        {
            //Arrange
            //amount = 12m;
            //expected = 0.405768m;
            var expecteValue = new MonoUnit(Volume.Ounce);
            var policyValue = new PolyUnitValue(new MonoUnit(Volume.Milliliter), decimal.Parse(amount));

            //ACT
            policyValue.Convert(Volume.Ounce);

            //ASSERT
            Assert.AreEqual(decimal.Parse(expected), Math.Round(policyValue.Amount, 6));
            Assert.AreEqual(expecteValue.UnitEnumerator, policyValue.Unit.UnitEnumerator);
        }

        [TestMethod]
        public void PolicyValueConvertPoundsToMiligrams()
        {
            //Arrange

            decimal expected = 453597.024404m;
            var policyValue = new PolyUnitValue(new MonoUnit(Mass.Pound), 1);
            var expectedUint = new MonoUnit(Mass.Milligram);
            //ACT
            policyValue.Convert(Mass.Milligram);

            //ASSERT
            Assert.AreEqual(expected, Math.Round(policyValue.Amount, 6));
            Assert.AreEqual(expectedUint.UnitEnumerator, policyValue.Unit.UnitEnumerator);//Have to specify Unit.Unit beacause of IUnit uase in PolyUnitValue
        }

        [TestMethod]
        public void PolicyValueConvertMicrogramsToMilliliterExceptionThrow()
        {
            //Arrange
            var policyValue = new PolyUnitValue(new MonoUnit(Mass.Microgram), 8000000);
            //ACT
            try
            {
                policyValue.Convert(Volume.Milliliter);
                Assert.Fail();
            }
            catch (UnitConversionException ex)

            {
                
                Console.WriteLine(ex.Message);
            }


            //ASSERT


        }




    }
}
