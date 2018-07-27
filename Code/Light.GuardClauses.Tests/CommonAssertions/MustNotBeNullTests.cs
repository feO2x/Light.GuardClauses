using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertions
{
    public static class MustNotBeNullTests
    {
        [Fact]
        public static void ReferenceIsNull()
        {
            const string parameterName = Metasyntactic.Foo;
            Action act = () => ((object) null).MustNotBeNull(parameterName);

            var exceptionAssertion = act.Should().Throw<ArgumentNullException>().And;
            exceptionAssertion.Message.Should().Contain($"{parameterName} must not be null.");
            exceptionAssertion.ParamName.Should().BeSameAs(parameterName);
        }

        [Fact]
        public static void ReferenceIsNotNull() => string.Empty.MustNotBeNull().Should().BeSameAs(string.Empty);

        [Fact]
        public static void CustomException() =>
            Test.CustomException(exceptionFactory => ((string) null).MustNotBeNull(exceptionFactory));

        [Fact]
        public static void CustomExceptionParamterNotNull() => 
            Metasyntactic.Foo.MustNotBeNull(() => null).Should().BeSameAs(Metasyntactic.Foo);

        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<ArgumentNullException>(message => ((object) null).MustNotBeNull(message: message));
    }
}