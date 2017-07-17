using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class MustNotHaveValueTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustNotHaveValue must throw an exception when the specified Nullable<T> has a value.")]
        public void HasValue()
        {
            DateTime? value = DateTime.Today;

            Action act = () => value.MustNotHaveValue(nameof(value));

            act.ShouldThrow<NullableHasValueException>()
               .And.ParamName.Should().Be(nameof(value));
        }

        [Fact(DisplayName = "MustNotHaveValue must not throw an exception when the specified Nullable<T> is null.")]
        public void HasNoValue()
        {
            double? value = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => value.MustNotHaveValue(nameof(value));

            act.ShouldNotThrow();
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => new int?(42).MustNotHaveValue(exception: exception)));

            testData.Add(new CustomMessageTest<NullableHasValueException>(message => new double?(42.0).MustNotHaveValue(message: message)));
        }
    }
}