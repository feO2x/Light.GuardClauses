using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertionsTests
{
    public sealed class MustBeNullTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustBeNull must throw an ArgumentNotNullException when the specified value is not null.")]
        public static void ArgumentNotNull()
        {
            var @string = "Hey";

            Action act = () => @string.MustBeNull(nameof(@string));

            act.ShouldThrow<ArgumentNotNullException>()
               .And.ParamName.Should().Be(nameof(@string));
        }

        [Fact(DisplayName = "MustBeNull must not throw an exception when the specified value is null.")]
        public static void ArgumentNull()
        {
            object @object = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            var result = @object.MustBeNull(nameof(@object));

            result.Should().BeNull();
        }

        [Fact(DisplayName = "MustBeNull must throw the custom exception and pass in the parameter if it is not null.")]
        public static void CustomParamterizedException()
        {
            var value = new object();
            var observedValue = default(object);
            var myException = new Exception();

            Action act = () => value.MustBeNull(v =>
                                                {
                                                    observedValue = v;
                                                    return myException;
                                                });

            act.ShouldThrow<Exception>().Which.Should().BeSameAs(myException);
            observedValue.Should().BeSameAs(value);
        }

        [Fact(DisplayName = "MustBeNull with custom parameterized exception must not throw an exception when the specified parameter is null.")]
        public void CustomParameterizedExceptionNotThrown()
        {
            ((string) null).MustBeNull(_ => new Exception()).Should().BeNull();
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => new object().MustBeNull(exception)))
                    .Add(new CustomMessageTest<ArgumentNotNullException>(message => new object().MustBeNull(message: message)));
        }
    }
}