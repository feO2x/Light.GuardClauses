using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertionsTests
{
    public sealed class MustBeNullTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustBeNull throws an exception when the specified value is not null.")]
        public void ArgumentNotNull()
        {
            var @string = "Hey";

            Action act = () => @string.MustBeNull(nameof(@string));

            act.ShouldThrow<ArgumentNotNullException>()
               .And.ParamName.Should().Be(nameof(@string));
        }

        [Fact(DisplayName = "MustBeNull must not throw an exception when the specified value is null.")]
        public void ArgumentNull()
        {
            object @object = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            var result = @object.MustBeNull(nameof(@object));

            result.Should().BeNull();
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => new object().MustBeNull(exception: exception)))
                    .Add(new CustomMessageTest<ArgumentNotNullException>(message => new object().MustBeNull(message: message)));
        }
    }
}