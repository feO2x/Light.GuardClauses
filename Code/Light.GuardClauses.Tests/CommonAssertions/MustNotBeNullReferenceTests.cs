using System;
using System.Collections;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertions
{
    public static class MustNotBeNullReferenceTests
    {
        [Fact]
        public static void StringIsNull() => CheckArgumentNullExceptionIsThrown(default(string));

        [Fact]
        public static void DelegateIsNull() => CheckArgumentNullExceptionIsThrown(default(Action));

        [Fact]
        public static void PolymorphicReferenceIsNull() => CheckArgumentNullExceptionIsThrown(default(IEnumerable));

        private static void CheckArgumentNullExceptionIsThrown<T>(T nullReference) where T : class
        {
            Action act = () => nullReference.MustNotBeNullReference(nameof(nullReference));

            act.Should().Throw<ArgumentNullException>()
               .And.Message.Should().Contain($"{nameof(nullReference)} must not be null.");
        }

        [Theory]
        [InlineData(42)]
        [InlineData(true)]
        [InlineData(false)]
        [InlineData(ConsoleColor.Cyan)]
        public static void ValueType<T>(T value) where T : struct =>
            value.MustNotBeNullReference().Should().Be(value);

        [Theory]
        [MetasyntacticVariablesData]
        [InlineData(new int[] { })]
        public static void ReferenceNotNull<T>(T reference) where T : class =>
            reference.MustNotBeNullReference().Should().BeSameAs(reference);

        [Fact]
        public static void CustomException() =>
            CustomExceptions.TestCustomException(exceptionFactory => ((object) null).MustNotBeNullReference(exceptionFactory));

        [Fact]
        public static void CustomMessage() =>
            CustomMessages.TestCustomMessage<ArgumentNullException>(message => ((object) null).MustNotBeNullReference(message: message));
    }
}