using System;
using System.Collections.Generic;

namespace Light.GuardClauses.Tests.CustomMessagesAndExceptions
{
    public interface ICustomMessageAndExceptionTestData
    {
        Type TestClassType { get; }

        IReadOnlyList<IRunnableTest> CustomExceptionTests { get; }
        IReadOnlyList<IRunnableTest> CustomMessageTests { get; }

        bool IsPopulatedCorrectly { get; }
    }
}