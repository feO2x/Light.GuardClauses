using FluentAssertions;
using System;
using System.Collections.Generic;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    public sealed class MustBeTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustBe must throw an exception when the specified value is not the expected one.")]
        [InlineData(42, 0)]
        [InlineData(true, false)]
        [InlineData("Hello", "World!")]
        public void ValuesNotEqual<T>(T value, T other)
        {
            Action act = () => value.MustBe(other, nameof(value));

            act.ShouldThrow<ArgumentException>()
               .And.Message.Should().Contain($"{nameof(value)} must be {other}, but you specified {value}");
        }

        [Theory(DisplayName = "MustBe must not throw an exception when the specified value is the same as the expected one.")]
        [InlineData(42)]
        [InlineData(55.89)]
        [InlineData("Hey")]
        public void ValuesEqual<T>(T value)
        {
            Action act = () => value.MustBe(value, nameof(value));

            act.ShouldNotThrow();
        }

        [Fact(DisplayName = "MustBe must throw an exception when the specified value is not the expected one, using an equality comparer for comparison.")]
        public void ValuesNotEqualWithEqualityComparer()
        {
            Action act = () => 55.0.MustBe(55.1, EqualityComparer<double>.Default);

            act.ShouldThrow<ArgumentException>();
        }

        [Fact(DisplayName = "MustBeEqualToValue must throw an exception when the specified IEquatable<T> values (Value Type) are not equal.")]
        public void EquatableStructsComparison()
        {
            Action act = () => 42.MustBeEqualToValue(44);

            act.ShouldThrow<ArgumentException>();
        }

        [Fact(DisplayName = "MustBeEqualTo must throw an exception when the specified IEquatable<T> values (Reference Types) are not equal.")]
        public void EquatableComparison()
        {
            Action act = () => "Hello".MustBeEqualTo("There!");

            act.ShouldThrow<ArgumentException>();
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => "Hello".MustBe("World", exception: exception)));

            testData.Add(new CustomMessageTest<ArgumentException>(message => 42.MustBe(48, message: message)));
        }
    }
}
