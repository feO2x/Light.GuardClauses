using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertionsTests
{
    public sealed class MustNotBeEmptyTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustNotBeEmpty must throw an exception when the specified GUID is empty.")]
        public void GuidEmpty()
        {
            var emptyGuid = Guid.Empty;

            Action act = () => emptyGuid.MustNotBeEmpty(nameof(emptyGuid));

            act.ShouldThrow<EmptyGuidException>()
               .And.ParamName.Should().Be(nameof(emptyGuid));
        }

        [Fact(DisplayName = "MustNotBeEmpty must not throw an exception when the specified GUID is not empty.")]
        public void GuidNotEmpty()
        {
            var validGuid = Guid.NewGuid();

            var result = validGuid.MustNotBeEmpty();

            result.Should().Be(validGuid);
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => Guid.Empty.MustNotBeEmpty(exception: exception)))
                    .Add(new CustomMessageTest<EmptyGuidException>(message => Guid.Empty.MustNotBeEmpty(message: message)));
        }
    }
}