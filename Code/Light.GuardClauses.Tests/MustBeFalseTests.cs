using System;
using FluentAssertions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    public sealed class MustBeFalseTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustBeFalse must throw an ArgumentException when the specified value is true.")]
        public void ParameterTrue()
        {
            Action act = () => true.MustBeFalse("This boolean");

            act.ShouldThrow<ArgumentException>()
               .And.Message.Should().Contain("This boolean must be false, but you specified true.");
        }

        [Fact(DisplayName = "MustBeFalse must not throw an exception when the specified value is false.")]
        public void ParameterFalse()
        {
            Action act = () => false.MustBeFalse();

            act.ShouldNotThrow();
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => true.MustBeFalse(exception: exception)))
                    .Add(new CustomMessageTest<ArgumentException>(message => true.MustBeFalse(message: message)));
        }
    }
}