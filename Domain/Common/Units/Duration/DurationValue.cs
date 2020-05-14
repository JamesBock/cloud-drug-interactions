using System;
using System.Collections.Generic;
using System.Text;

namespace UWPLockStep.Domain.Common.Units
{
    public class DurationValue : IComparable<DurationValue>
    {

        public TimeEnumeration DurationUnit { get; private set; }

        public decimal Quantity { get; private set; }//this is just the multiplyer. Only whole numbers should be allowed but decimal may be required for calculations
        public TimeSpan TimeSpanDuration { get; private set; } //time in Ticks?


        public DurationValue(TimeEnumeration durationUnit, long quantity)
        {
            DurationUnit = durationUnit;
            Quantity = quantity;
            TimeSpanDuration = TimeSpan.FromTicks(quantity * durationUnit.Time);
        }

        public int CompareTo(DurationValue other)
        {
            if (ReferenceEquals(other, null) )return 1;
                
            return DurationUnit.ToBase(Quantity).CompareTo(other.DurationUnit.ToBase(other.Quantity));
        }

        public static bool operator > (DurationValue left, DurationValue right)
        {
            return left.DurationUnit.ToBase(left.Quantity).CompareTo(right.DurationUnit.ToBase(right.Quantity)) == 1;
        }

        public static bool operator < (DurationValue left, DurationValue right)
        {
            return left.DurationUnit.ToBase(left.Quantity).CompareTo(right.DurationUnit.ToBase(right.Quantity)) == -1;

        }

        public string GetDurationString()
        {

            if (Quantity == 1)
            {
                return $"{DurationUnit.Name}";
            }

            else return $"{Quantity.ToString()} {DurationUnit.Name}s";
        }
        //Two types of conversion here, one to TimeSpan, one to a differnt display Enumeration, since all of the values are the same in terms of Ticks
        /// <summary>
        /// Fully converts the Amounts and Units to the TimeEnumerations parameter
        /// If this is a true ValueObject, immutability means "changing" the value requires creating a new object to replace the current one. This could be done by the implementing class? or could have in this class. Will the old ValueObject be garbge collected
        /// </summary>
        /// <param name="finish"></param>
        public void Convert(TimeEnumeration finish)
        {
            Quantity = (DurationUnit.Time* Quantity) / finish.Time ;
            DurationUnit = finish;
            
        }

    }
}
