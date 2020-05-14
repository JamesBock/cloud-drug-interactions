using System;
using System.Collections.Generic;
using System.Text;

namespace UWPLockStep.Domain.Common.Exceptions
{
    public class NonUniqueDimensionException : Exception
    {
        public override string Message { get; }



        public NonUniqueDimensionException()
        {
            Message = "Not all units belong to different dimensions. This could lead to inequivalent conversions between units.";
        }
    }
}
