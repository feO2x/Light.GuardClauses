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
            const string paramterName = Metasyntactic.Foo;
            Action act = () => ((string) null).MustNotBeDefault(paramterName);

            var exceptionAssertion = act.Should().Throw<ArgumentNullException>().Which;
            exceptionAssertion.Message.Should().Contain($"{paramterName} must not be null.");
            exceptionAssertion.ParamName.Should().BeSameAs(paramterName);
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

            var exceptionAssertion = act.Should().Throw<ArgumentDefaultException>().Which;
            exceptionAssertion.Message.Should().Contain($"{nameof(defaultValue)} must not be the default value.");
            exceptionAssertion.ParamName.Should().BeSameAs(nameof(defaultValue));
        }

        [Fact]
        public static void NullableIsNull()
        {
            Action act = () => ((int?) null).MustNotBeDefault();

            act.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData(Metasyntactic.Foo)]
        [InlineData(Metasyntactic.Bar)]
        public static void ReferenceIsNotNull(string reference) => reference.MustNotBeDefault().Should().BeSameAs(reference);

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
        public static void CustomExceptionNotNull() =>
            Metasyntactic.Foo.MustNotBeDefault(() => null).Should().BeSameAs(Metasyntactic.Foo);

        [Fact]
        public static void CustomExceptionNotDefault() =>
            true.MustNotBeDefault(() => null).Should().BeTrue();
        
        [Fact]
        public static void CustomMessageForReferenceType() =>
            Test.CustomMessage<ArgumentNullException>(message => ((string) null).MustNotBeDefault(message: message));

        [Fact]
        public static void CustomMessageForValueType() =>
            Test.CustomMessage<ArgumentDefaultException>(message => default(TypeCode).MustNotBeDefault(message: message));

        [Fact]
        public static void CallerArgumentExpression()
        {
            var someParameter = 0;

            Action act = () => someParameter.MustNotBeDefault();

            act.Should().Throw<ArgumentDefaultException>()
               .And.ParamName.Should().Be(nameof(someParameter));
        }

    }
}