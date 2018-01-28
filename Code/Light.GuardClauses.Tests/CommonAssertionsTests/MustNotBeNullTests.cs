using System;
using FluentAssertions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertionsTests
{
    public sealed class MustNotBeNullTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustNotBeNull must throw an ArgumentNullException when the specified value is null.")]
        public void NullMessage()
        {
            object @object = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => @object.MustNotBeNull(nameof(@object));

            act.ShouldThrow<ArgumentNullException>()
               .And.Message.Should().Contain($"{nameof(@object)} must not be null.");
        }

        [Fact(DisplayName = "MustNotBeNull must not throw an exception when the specified reference is not null.")]
        public void ValidObjectReferenceIsGiven()
        {
            var result = string.Empty.MustNotBeNull();

            result.Should().Be(string.Empty);
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => ((object) null).MustNotBeNull(exception)))
                    .Add(new CustomMessageTest<ArgumentNullException>(message => ((object) null).MustNotBeNull(message: message)));
        }
    }
}