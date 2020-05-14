using System;
using System.Collections.Generic;
using System.Text;

namespace UWPLockStep.Domain.Common.Units
{
    public class Duration : TimeEnumeration
    {
        public static readonly Duration Tick
         = new Duration(0, "Tick", "tick", 1);
        public static readonly Duration Hour
         = new Duration(1, "Hour", "hr", TimeSpan.TicksPerHour);
        public static readonly Duration Minute
          = new Duration(2, "Minute", "min", TimeSpan.TicksPerMinute);
        public static readonly Duration Day
          = new Duration(3, "Day", "d", TimeSpan.TicksPerDay);//is this a thing

        private Duration()
        {

        }
        private Duration(int value, string name, string abbreviation, long time) : base(value, name, abbreviation, time) { }


        
        //public override decimal FromBase(decimal amount)//this functionality is built into TimeSpan but Enumerations are not and they need to be added
        //{
        //    return base.FromBase(amount);
        //}

        //public override decimal ToBase(decimal amount)
        //{
        //    return base.ToBase(amount);
        //}
    }
}
