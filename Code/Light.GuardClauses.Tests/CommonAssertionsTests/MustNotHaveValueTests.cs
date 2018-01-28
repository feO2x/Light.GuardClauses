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

        [Fact(DisplayName = "MustNotHaveValue must throw the custom parameterized exception when the specified Nullable<T> has a value.")]
        public void HasValueWithCustumParameterizedException()
        {
            DateTime? nullable = DateTime.UtcNow;
            var observedValue = default(DateTime);
            var exception = new Exception();

            Action act = () => nullable.MustNotHaveValue(v =>
                                                         {
                                                             observedValue = v;
                                                             return exception;
                                                         });

            act.ShouldThrow<Exception>().Which.Should().BeSameAs(exception);
            observedValue.Should().Be(nullable.Value);
        }

        [Fact(DisplayName = "MustNotHaveValue must not throw the custom parameterized exception when the specified Nullable<T> is null.")]
        public void HasNoValueWithCustomParameterizedException()
        {
            var result = ((int?) null).MustNotHaveValue(v => new Exception());
            result.Should().BeNull();
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            // ReSharper disable RedundantExplicitNullableCreation
            testData.Add(new CustomExceptionTest(exception => new int?(42).MustNotHaveValue(exception)))
                    .Add(new CustomMessageTest<NullableHasValueException>(message => new double?(42.0).MustNotHaveValue(message: message)));
            // ReSharper restore RedundantExplicitNullableCreation
        }
    }
}