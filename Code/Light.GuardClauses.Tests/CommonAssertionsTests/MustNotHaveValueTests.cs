using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertionsTests
{
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
            var result = value.MustNotHaveValue();

            result.Should().BeNull();
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            // ReSharper disable RedundantExplicitNullableCreation
            testData.Add(new CustomExceptionTest(exception => new int?(42).MustNotHaveValue(exception: exception)))
                    .Add(new CustomMessageTest<NullableHasValueException>(message => new double?(42.0).MustNotHaveValue(message: message)));
            // ReSharper restore RedundantExplicitNullableCreation
        }
    }
}