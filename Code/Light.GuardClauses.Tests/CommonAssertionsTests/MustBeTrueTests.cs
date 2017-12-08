using System;
using FluentAssertions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertionsTests
{
    public sealed class MustBeTrueTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustBeTrue must throw an ArgumentException when the specified value is false.")]
        public void ParamterFalse()
        {
            Action act = () => false.MustBeTrue("This boolean");

            act.ShouldThrow<ArgumentException>()
               .And.Message.Should().Contain("This boolean must be true, but you specified false.");
        }

        [Fact(DisplayName = "MustBeTrue must not throw an exception when the specified value is true.")]
        public void ParameterTrue()
        {
            var result = true.MustBeTrue();

            result.Should().BeTrue();
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => false.MustBeTrue(exception: exception)))
                    .Add(new CustomMessageTest<ArgumentException>(message => false.MustBeTrue(message: message)));
        }
    }
}