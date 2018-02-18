using System;
using FluentAssertions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertionsTests
{
    public sealed class MustNotBeFalseTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustNotBeFalse must throw an ArgumentException when the specified boolean is false.")]
        public void BooleanFalse()
        {
            const bool myValue = false;

            Action act = () => myValue.MustNotBeFalse(nameof(myValue));

            act.Should().Throw<ArgumentException>()
               .And.Message.Should().Contain($"{nameof(myValue)} must be true, but it actually is false.");
        }

        [Fact(DisplayName = "MustNotBeFalse must not throw an exception when the specified boolean is true.")]
        public void BooleanTrue()
        {
            var result = true.MustNotBeFalse();

            result.Should().BeTrue();
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.AddExceptionTest(exception => false.MustNotBeFalse(exception))
                    .AddMessageTest<ArgumentException>(message => false.MustNotBeFalse(message: message));
        }
    }
}