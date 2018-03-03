using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.TypeAssertionsTests
{
    public sealed class MustBeEquivalentToTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustBeEquivalentTo must throw a TypeException when the two types are not equivalent.")]
        public static void TypesNotEquivalent()
        {
            var first = typeof(double);
            var second = typeof(Dictionary<,>);

            Action act = () => first.MustBeEquivalentTo(second, nameof(first));

            act.Should().Throw<TypeException>()
               .And.Message.Should().Contain($"first \"{first}\" must be equivalent to \"{second}\", but it actually is not.");
        }

        [Fact(DisplayName = "MustBeEquivalentTo must not throw an exception when the two types are equivalent.")]
        public static void TypeEquivalent()
        {
            var first = typeof(IList<string>);
            var second = typeof(IList<>);

            var result = first.MustBeEquivalentTo(second);

            result.Should().BeSameAs(first);
        }

        [Fact(DisplayName = "MustBeEquivalentTo must throw an ArgumentNullException when parameter is null.")]
        public static void ParameterNull()
        {
            Action act = () => ((Type) null).MustBeEquivalentTo(typeof(string), "foo");

            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("foo");
        }

        [Fact(DisplayName = "MustBeEquivalentTo must throw the custom exception with one parameter when the specified types are not equivalent.")]
        public static void CustomExceptionWithParameter()
        {
            var first = typeof(string);
            var second = typeof(object);
            var recordedType = default(Type);
            var exception = new Exception();

            Action act = () => first.MustBeEquivalentTo(second, type =>
            {
                recordedType = type;
                return exception;
            });

            act.Should().Throw<Exception>().Which.Should().BeSameAs(exception);
            recordedType.Should().BeSameAs(first);
        }

        [Fact(DisplayName = "MustBeEquivalentTo must not throw the custom exception with one parameter when the specified types are equivalent.")]
        public static void NoCustomExceptionWithParameter()
        {
            var type = typeof(ObservableCollection<string>);

            var result = type.MustBeEquivalentTo(typeof(ObservableCollection<>), _ => null);

            result.Should().BeSameAs(type);
        }

        [Fact(DisplayName = "MustBeEquivalentTo must throw the custom exception with two parameters when the specified types are not equivalent.")]
        public static void CustomExceptionTwoParameters()
        {
            var first = typeof(int);
            var second = typeof(double);
            var recordedParameter = default(Type);
            var recordedOther = default(Type);
            var exception = new Exception();

            Action act = () => first.MustBeEquivalentTo(second, (t1, t2) =>
            {
                recordedParameter = t1;
                recordedOther = t2;
                return exception;
            });

            act.Should().Throw<Exception>().Which.Should().BeSameAs(exception);
            recordedParameter.Should().BeSameAs(first);
            recordedOther.Should().BeSameAs(second);
        }

        [Fact(DisplayName = "MustBeEquivalentTo must not throw the custom exception with two parameters when the specified types are equivalent.")]
        public static void NoCustomExceptionTwoParameters()
        {
            var type = typeof(double);

            var result = type.MustBeEquivalentTo(type, (t1, t2) => null);

            result.Should().BeSameAs(type);
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => typeof(string).MustBeEquivalentTo(typeof(int), exception)))
                    .Add(new CustomMessageTest<TypeException>(message => typeof(Delegate).MustBeEquivalentTo(typeof(Math), message: message)));
        }
    }
}