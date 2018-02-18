using System;
using FluentAssertions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertionsTests
{
    public sealed class MustBeFalseTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustBeFalse must throw an ArgumentException when the specified value is true.")]
        public void ParameterTrue()
        {
            Action act = () => true.MustBeFalse("This boolean");

            act.Should().Throw<ArgumentException>()
               .And.Message.Should().Contain("This boolean must be false, but it actually is true.");
        }

        [Fact(DisplayName = "MustBeFalse must not throw an exception when the specified value is false.")]
        public void ParameterFalse()
        {
            var result = false.MustBeFalse();

            result.Should().BeFalse();
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => true.MustBeFalse(exception)))
                    .Add(new CustomMessageTest<ArgumentException>(message => true.MustBeFalse(message: message)));
        }
    }
}