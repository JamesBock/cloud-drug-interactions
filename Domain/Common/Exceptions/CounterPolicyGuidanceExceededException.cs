using System;
using System.Runtime.Serialization;

namespace UWPLockStep.Domain.Common.Exceptions
{
    [Serializable]
    internal class CounterPolicyGuidanceExceededException : Exception
    {
        public CounterPolicyGuidanceExceededException()
        {
        }

        public CounterPolicyGuidanceExceededException(string message) : base(message)
        {
        }

        public CounterPolicyGuidanceExceededException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CounterPolicyGuidanceExceededException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}