using System;
using System.Collections.Generic;

namespace Light.GuardClauses.Tests.CustomMessagesAndExceptions
{
    public sealed class CustomMessageAndExceptionTestData : ICustomMessageAndExceptionTestData
    {
        private readonly List<IRunnableTest> _customExceptionTests = new List<IRunnableTest>();
        private readonly List<IRunnableTest> _customMessageTests = new List<IRunnableTest>();
        private readonly Type _testClassType;


        public CustomMessageAndExceptionTestData(Type testClassType)
        {
            testClassType.MustNotBeNull();

            _testClassType = testClassType;
        }

        Type ICustomMessageAndExceptionTestData.TestClassType => _testClassType;
        IReadOnlyList<IRunnableTest> ICustomMessageAndExceptionTestData.CustomExceptionTests => _customExceptionTests;
        IReadOnlyList<IRunnableTest> ICustomMessageAndExceptionTestData.CustomMessageTests => _customMessageTests;

        bool ICustomMessageAndExceptionTestData.IsPopulatedCorrectly => _customExceptionTests.Count >= 1 && _customMessageTests.Count >= 1;

        public CustomMessageAndExceptionTestData Add(CustomExceptionTest test)
        {
            test.MustNotBeNull(nameof(test));

            _customExceptionTests.Add(test);
            return this;
        }

        public CustomMessageAndExceptionTestData Add<TException>(CustomMessageTest<TException> test) where TException : Exception
        {
            test.MustNotBeNull(nameof(test));

            _customMessageTests.Add(test);
            return this;
        }

        public CustomMessageAndExceptionTestData AddExceptionTest(Action<Func<Exception>> raiseException)
        {
            _customExceptionTests.Add(new CustomExceptionTest(raiseException));
            return this;
        }

        public CustomMessageAndExceptionTestData AddMessageTest<TException>(Action<string> raiseException) where TException : Exception
        {
            _customMessageTests.Add(new CustomMessageTest<TException>(raiseException));
            return this;
        }
    }
}