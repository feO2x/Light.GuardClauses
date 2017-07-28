using System;
using FluentAssertions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class MustNotBeFalseTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustNotBeFalse must throw an ArgumentException when the specified boolean is false.")]
        public void BooleanFalse()
        {
            const bool myValue = false;

            Action act = () => myValue.MustNotBeFalse(nameof(myValue));

            act.ShouldThrow<ArgumentException>()
               .And.Message.Should().Contain($"{nameof(myValue)} must not be false, but you specified false.");
        }

        [Fact(DisplayName = "MustNotBeFalse must not throw an exception when the specified boolean is true.")]
        public void BooleanTrue()
        {
            Action act = () => true.MustNotBeFalse();

            act.ShouldNotThrow();
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.AddExceptionTest(exception => false.MustNotBeFalse(exception: exception))
                    .AddMessageTest<ArgumentException>(message => false.MustNotBeFalse(message: message));
        }
    }
}