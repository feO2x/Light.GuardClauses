using System;
using FluentAssertions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertionsTests
{
    public sealed class MustNotBeDefaultTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustNotBeDefault must throw an ArgumentNullException when a null reference is passed in.")]
        public static void IsNull()
        {
            object reference = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => reference.MustNotBeDefault(nameof(reference));

            act.ShouldThrow<ArgumentNullException>()
               .And.Message.Should().Contain($"{nameof(reference)} must not be null.");
        }

        [Theory(DisplayName = "MustNotBeDefault must throw an ArgumentException when a default value of a value type is passed in.")]
        [InlineData(default(int))]
        [InlineData(default(char))]
        [InlineData(default(double))]
        [InlineData(default(short))]
        [InlineData(default(long))]
        public static void IsDefault<T>(T structValue)
        {
            Action act = () => structValue.MustNotBeDefault(nameof(structValue));

            act.ShouldThrow<ArgumentException>()
               .And.Message.Should().Contain($"{nameof(structValue)} must not be the default value.");
        }

        [Fact(DisplayName = "MustNotBeDefault must throw an ArgumentNullException when null of type Nullable<T> is passed in.")]
        public void Nullable()
        {
            int? nullable = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => nullable.MustNotBeDefault();

            act.ShouldThrow<ArgumentNullException>();
        }

        [Theory(DisplayName = "MustNotBeDefault must return the value when it is not a default value.")]
        [InlineData(42)]
        [InlineData(82567L)]
        [InlineData("Foo")]
        public void NotDefault<T>(T value)
        {
            var result = value.MustNotBeDefault();

            result.Should().Be(value);
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.AddExceptionTest(exception => 0.MustNotBeDefault(exceptionFactory: exception))
                    .AddMessageTest<ArgumentException>(message => default(char).MustNotBeDefault(message: message));

            testData.AddExceptionTest(exception => default(object).MustNotBeDefault(exceptionFactory: exception))
                    .AddMessageTest<ArgumentNullException>(message => default(string).MustNotBeDefault(message: message));
        }
    }
}