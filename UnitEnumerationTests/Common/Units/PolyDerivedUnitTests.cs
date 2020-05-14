using Microsoft.VisualStudio.TestTools.UnitTesting;
using UWPLockStep.Domain.Common.Units;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using UWPLockStep.Domain.Common.Exceptions;

namespace UWPLockStep.Domain.Common.Units.Tests
{
    [TestClass()]
    public class PolyDerivedUnitTests
    {
        [TestMethod()]
        public void PolyDerivedUnitDictionaryConstructorTest()
        {

            //Assemble 
            var miligramTest = new MonoUnitValue(new MonoUnit(Mass.Milligram), 12);
            var molTest = new MonoUnitValue(new MonoUnit(AmountOfElement.Mole), 5);
            var mLTest = new MonoUnitValue(new MonoUnit(Volume.Liter), 10);



            //Act
            var polyTest = new PolyUnit(miligramTest, molTest, mLTest);
            var listOfUnits = new Dictionary<UnitEnumeration, decimal>();
            listOfUnits.Add(miligramTest.Unit.UnitEnumerator, miligramTest.Amount);
            listOfUnits.Add(molTest.Unit.UnitEnumerator, molTest.Amount);
            listOfUnits.Add(mLTest.Unit.UnitEnumerator, mLTest.Amount);

            //Asssert
            //Assert.AreEqual(polyTest.TranslationDefinitions, listOfUnits);
        }

        [TestMethod()]
        public void ProjectUnitTestNonUniqueExpecption()
        {
            //Assemble 
            var miligramTest = UnitValueFactory.Create(Mass.Milligram, 10);
            var molTest = new MonoUnitValue(new MonoUnit(AmountOfElement.Mole), 5);
            var mLTest = new MonoUnitValue(new MonoUnit(Mass.Gram), .2m);


            try
            {

                var testPolyUnit = new PolyUnit(miligramTest, molTest, mLTest);
                Assert.Fail();
            }
            catch (NonUniqueDimensionException)
            {


            }


        }

        [TestMethod()]
        public void ProjectUnitTestConvertUnitMgToMl()
        {
            //Assemble 
            var miligramTest = new MonoUnitValue(new MonoUnit(Mass.Milligram), 10);
            var molTest = new MonoUnitValue(new MonoUnit(AmountOfElement.Mole), 5);
            var mLTest = new MonoUnitValue(new MonoUnit(Volume.Liter), .2m);

            var testPolyUnit = new PolyUnit(miligramTest, molTest, mLTest);
            var testPolyValue = new PolyUnitValue(testPolyUnit, 5m);


            //Act

            testPolyValue.Convert(Volume.Milliliter);

            //Assert
            Assert.AreEqual(100m, testPolyValue.Amount);
        }

        [TestMethod()]
        public void ProjectUnitTestConvertUnitMgToMmol()
        {
            //Assemble 
            var miligramTest = new MonoUnitValue(new MonoUnit(Mass.Milligram), 10);
            var molTest = new MonoUnitValue(new MonoUnit(AmountOfElement.Mole), 5);
            var mLTest = new MonoUnitValue(new MonoUnit(Volume.Liter), .2m);

            var testPolyUnit = new PolyUnit(miligramTest, molTest, mLTest);
            var testPolyValue = new PolyUnitValue(testPolyUnit, 50m);


            //Act

            testPolyValue.Convert(AmountOfElement.Millimole);

            //Assert
            Assert.AreEqual(25000m, testPolyValue.Amount);
        }

        [TestMethod()]
        public void ProjectUnitTestUnitConversionException()//should fail due to Unit not being in the Dictionary
        {
            //Assemble 
            var miligramTest = new MonoUnitValue(new MonoUnit(Mass.Milligram), 10);
            var molTest = new MonoUnitValue(new MonoUnit(AmountOfElement.Mole), 5);
            var mLTest = new MonoUnitValue(new MonoUnit(Volume.Milliliter), .2m);

            var testPolyUnit = new PolyUnit(miligramTest, molTest, mLTest);
            var testPolyValue = new PolyUnitValue(testPolyUnit, 5m);


            //Act
            try
            {
                testPolyValue.Convert(Equivalents.Milliequivalent);
                Assert.Fail();
            }
            catch (UnitConversionException)
            {


            }

        }

    }
}