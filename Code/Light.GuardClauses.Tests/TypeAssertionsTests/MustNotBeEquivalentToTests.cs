using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.TypeAssertionsTests
{
    public sealed class MustNotBeEquivalentToTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustNotBeEquivalentTo must throw a TypeException when the two types are equivalent.")]
        public static void TypesEquivalent()
        {
            var first = typeof(decimal);
            var second = typeof(decimal);

            Action act = () => first.MustNotBeEquivalentTo(second, nameof(first));

            act.Should().Throw<TypeException>()
               .And.Message.Should().Contain($"first \"{first}\" must not be equivalent to \"{second}\", but it actually is.");
        }

        [Fact(DisplayName = "MustNotBeEquivalentTo must not throw an exception when the two types are not equivalent.")]
        public static void TypeEquivalent()
        {
            var first = typeof(Expression);
            var second = typeof(IComparer<>);

            var result = first.MustNotBeEquivalentTo(second);

            result.Should().BeSameAs(first);
        }

        [Fact(DisplayName = "MustNotBeEquivalentTo must throw an ArgumentNullException when parameter is null.")]
        public static void ParameterNull()
        {
            Action act = () => ((Type) null).MustNotBeEquivalentTo(typeof(string), "foo");

            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("foo");
        }

        [Fact(DisplayName = "MustNotBeEquivalentTo must throw the custom exception with one parameter when the specified types are equivalent.")]
        public static void CustomExceptionOneParameter()
        {
            var first = typeof(List<object>);
            var second = typeof(List<>);
            var recordedType = default(Type);
            var exception = new Exception();

            Action act = () => first.MustNotBeEquivalentTo(second, type =>
            {
                recordedType = type;
                return exception;
            });

            act.Should().Throw<Exception>().Which.Should().BeSameAs(exception);
            recordedType.Should().BeSameAs(first);
        }

        [Fact(DisplayName = "MustNotBeEquivalentTo must not throw the custom exception with one parameter when the specified types are not equivalent.")]
        public static void NoCustomExceptionOneParameter()
        {
            var type = typeof(decimal);

            var result = type.MustNotBeEquivalentTo(typeof(int), t => null);

            result.Should().BeSameAs(type);
        }

        [Fact(DisplayName = "MustNotBeEquivalentTo must throw the custom exception with two parameters when the specified types are equivalent.")]
        public static void CustomExceptionTwoParameters()
        {
            var first = typeof(float);
            var second = typeof(float);
            var recordedParameter = default(Type);
            var recordedOther = default(Type);
            var exception = new Exception();

            Action act = () => first.MustNotBeEquivalentTo(second, (t1, t2) =>
            {
                recordedParameter = t1;
                recordedOther = t2;
                return exception;
            });

            act.Should().Throw<Exception>().Which.Should().BeSameAs(exception);
            recordedParameter.Should().BeSameAs(first);
            recordedOther.Should().BeSameAs(second);
        }

        [Fact(DisplayName = "MustNotBeEquivalentTo must not throw the custom exception with two parameters when the specified types are not equivalent.")]
        public static void NoCustomExceptionTwoParameters()
        {
            var type = typeof(bool);

            var result = type.MustNotBeEquivalentTo(typeof(object), (t1, t2) => null);

            result.Should().BeSameAs(type);
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => typeof(string).MustNotBeEquivalentTo(typeof(string), exception)))
                    .Add(new CustomMessageTest<TypeException>(message => typeof(string).MustNotBeEquivalentTo(typeof(string), message: message)));
        }
    }
}