using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPLockStep.Domain.Common.Units
{
    public class Units : UnitEnumeration
    {
        public static readonly Units Unit 
          = new Units(0, "Unit", "unit", 1m);




        protected Units() { }
        protected Units(int value, string name, string abbreviation, decimal baseConversion) : base(value, name, abbreviation, baseConversion) { }
    }
}
