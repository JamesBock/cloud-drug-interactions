using System;
using System.Collections.Generic;
using System.Text;
using UWPLockStep.Domain.Common.Units;

namespace UWPLockStep.Domain.Common.Exceptions
{
    public class UnitConversionException : Exception
    {
        public override string Message { get; }
        //public override string HelpLink { get => base.HelpLink; set => base.HelpLink = value; }//very cool concept


        public UnitConversionException(UnitEnumeration fromUnit, UnitEnumeration toUnit)
        {
            Message = $"{fromUnit.Name} cannot convert to {toUnit.Name} in this context.";
        }
    }
}
