using System;

namespace Light.GuardClauses.Exceptions
{
    public class InvalidDateTimeException : ArgumentException
    {
        public readonly DateTime InvalidDateTime;

        public InvalidDateTimeException(DateTime invalidDateTime, string message, string parameterName, Exception innerException = null)
            : base(message, parameterName, innerException)
        {
            InvalidDateTime = invalidDateTime;
        }
    }
}