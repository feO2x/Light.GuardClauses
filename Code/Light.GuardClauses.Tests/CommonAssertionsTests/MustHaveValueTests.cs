using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class MustHaveValueTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustHaveValue must throw an exception when the specified Nullable<T> has no value.")]
        public void HasNoValue()
        {
            DateTime? value = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => value.MustHaveValue(nameof(value));

            act.ShouldThrow<NullableHasNoValueException>()
               .And.ParamName.Should().Be(nameof(value));
        }

        [Fact(DisplayName = "MustHaveValue must not throw an exception when the specified Nullable<T> has a value.")]
        public void HasValue()
        {
            int? value = 42;

            Action act = () => value.MustHaveValue(nameof(value));

            act.ShouldNotThrow();
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception =>
                                                 {
                                                     double? value = null;
                                                     // ReSharper disable once ExpressionIsAlwaysNull
                                                     value.MustHaveValue(exception: exception);
                                                 }));

            testData.Add(new CustomMessageTest<NullableHasNoValueException>(message =>
                                                                            {
                                                                                double? value = null;
                                                                                // ReSharper disable once ExpressionIsAlwaysNull
                                                                                value.MustHaveValue(message: message);
                                                                            }));
        }
    }
}