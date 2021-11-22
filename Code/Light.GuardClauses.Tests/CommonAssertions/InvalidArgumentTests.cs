using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertions
{
    public static class InvalidArgumentTests
    {
        [Theory]
        [MetasyntacticVariablesData]
        public static void ConditionTrue(string parameterName)
        {
            var act = () => Check.InvalidArgument(true, parameterName);

            var exceptionAssertion = act.Should().Throw<ArgumentException>().And;
            exceptionAssertion.ParamName.Should().BeSameAs(parameterName);
            exceptionAssertion.Message.Should().Contain($"{parameterName} is invalid.");
        }

        [Fact]
        public static void ConditionFalse()
        {
            Check.InvalidArgument(false);
        }

        [Fact]
        public static void ConditionFalseCustomException()
        {
            Check.InvalidArgument(false, () => new Exception());
        }

        [Fact]
        public static void ConditionFalseCustomExceptionWithParameter()
        {
            Check.InvalidArgument(false, new object(), _ => new Exception());
        }

        [Fact]
        public static void CustomException() =>
            Test.CustomException(exceptionFactory => Check.InvalidArgument(true, exceptionFactory));

        [Fact]
        public static void CustomExceptionWithParameter() =>
            Test.CustomException(new object(),
                                 (parameter, exceptionFactory) => Check.InvalidArgument(true, parameter, exceptionFactory));

        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<ArgumentException>(message => Check.InvalidArgument(true, message: message));

        [Fact]
        public static void DefaultMessage()
        {
            var act = () => Check.InvalidArgument(true);

            act.Should().Throw<ArgumentException>()
               .And.Message.Should().Be("The value is invalid.");
        }
    }
}