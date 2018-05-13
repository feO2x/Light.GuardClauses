using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertions
{
    public static class MustNotBeDefaultTests
    {
        [Fact]
        public static void ReferenceIsNull()
        {
            Action act = () => ((string) null).MustNotBeDefault(Metasyntactic.Foo);

            act.Should().Throw<ArgumentNullException>()
               .And.Message.Should().Contain($"{Metasyntactic.Foo} must not be null.");
        }

        [Theory]
        [InlineData(default(int))]
        [InlineData(default(double))]
        [InlineData(default(short))]
        [InlineData(default(long))]
        [InlineData(default(char))]
        [InlineData(default(bool))]
        [InlineData(default(ConsoleColor))]
        public static void ValueIsDefault<T>(T defaultValue) where T : struct
        {
            Action act = () => defaultValue.MustNotBeDefault(nameof(defaultValue));

            act.Should().Throw<ArgumentDefaultException>()
               .And.Message.Should().Contain($"{nameof(defaultValue)} must not be the default value.");
        }

        [Fact]
        public static void NullableIsNull()
        {
            int? nullable = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => nullable.MustNotBeDefault();

            act.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData("Foo")]
        [InlineData(new int[] { })]
        public static void ReferenceIsNotNull<T>(T reference) where T : class => reference.MustNotBeDefault().Should().BeSameAs(reference);

        [Theory]
        [InlineData(42)]
        [InlineData(true)]
        [InlineData('a')]
        [InlineData(8256L)]
        public static void ValueIsNotDefault<T>(T value) where T : struct => value.MustNotBeDefault().Should().Be(value);

        [Fact]
        public static void NullableIsNotNull()
        {
            var nullable = new decimal?(42m);
            nullable.MustNotBeDefault().Should().Be(nullable);
        }

        [Fact]
        public static void CustomExceptionForReferenceType() =>
            Test.CustomException(exceptionFactory => ((object)null).MustNotBeDefault(exceptionFactory));

        [Fact]
        public static void CustomExceptionForValueType() =>
            Test.CustomException(exceptionFactory => default(int).MustNotBeDefault(exceptionFactory));

        [Fact]
        public static void CustomMessageForReferenceType() =>
            Test.CustomMessage<ArgumentNullException>(message => ((string) null).MustNotBeDefault(message: message));

        [Fact]
        public static void CustomMessageForValueType() =>
            Test.CustomMessage<ArgumentDefaultException>(message => default(TypeCode).MustNotBeDefault(message: message));

    }
}