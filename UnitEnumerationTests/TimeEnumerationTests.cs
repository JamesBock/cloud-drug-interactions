using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using UWPLockStep.Domain.Common;
using UWPLockStep.Domain.Common.Units;
using UWPLockStep.Domain.Entities.Policies;

namespace EnumerationTests
{
    [TestClass]
    public class TimeEnumerationTests
    {
        [TestMethod]
        public void DurationValueConvertMinutesToHours()
        {
            //Arrange

            decimal expected = 2;
            var durationValue = new DurationValue(Duration.Minute, 120);

            //ACT
            durationValue.Convert(Duration.Hour);

            //ASSERT
            Assert.AreEqual(expected, durationValue.Quantity);
        }

        [TestMethod]
        public void DurationValueCheckTimeSpanAssignment()
        {
            //Arrange

            TimeSpan expected = new TimeSpan(2, 0, 0);
            var durationValue = new DurationValue(Duration.Minute, 120);

            //ACT

            //assignment in ctor

            //ASSERT
            Assert.AreEqual(expected, durationValue.TimeSpanDuration);
        }

        [TestMethod]
        public void DurationValueGreaterThanOverrideMinutesHours()
        {
            //Arrange

            var left = new DurationValue(Duration.Minute, 190);
            var right = new DurationValue(Duration.Hour, 1);
            var expected = true;

            //ACT
            var test = left > right;


            //ASSERT
            Assert.AreEqual(expected, test);
        }
        [TestMethod]
        public void DurationValueLessThanOverrideHoursTicks()
        {
            //Arrange

            var left = new DurationValue(Duration.Hour, 190);
            var right = new DurationValue(Duration.Tick, 1);
            var expected = false;

            //ACT
            var test = left < right;


            //ASSERT
            Assert.AreEqual(expected, test);
        }
    }
}
