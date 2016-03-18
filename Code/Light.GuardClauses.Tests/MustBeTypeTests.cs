using System;
using System.IO;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    public sealed class MustBeTypeTests
    {
        [Fact(DisplayName = "MustBeType must throw an exception when object cannot be downcasted.")]
        public void TypeMismatch()
        {
            object @object = "Hey";

            Action act = () => @object.MustBeType<Array>(nameof(@object));

            act.ShouldThrow<TypeMismatchException>()
               .And.Message.Should().Contain($"{nameof(@object)} is of type {typeof (string).FullName} and cannot be downcasted to {typeof (Array).FullName}.");
        }

        [Fact(DisplayName = "MustBeType must return the downcasted object if cast succeeds.")]
        public void TypeDowncasted()
        {
            const string @string = "Hey";
            object @object = @string;

            var downcastedValue = @object.MustBeType<string>(nameof(@object));
            downcastedValue.Should().BeSameAs(@string);
        }

        [Fact(DisplayName = "The caller can specify a custom message that MustBeType must inject instead of the default one.")]
        public void CustomMessage()
        {
            object @object = "Hello";
            const string message = "Thou shall be in the same inheritance line!";

            Action act = () => @object.MustBeType<Activator>(message: message);

            act.ShouldThrow<TypeMismatchException>()
               .And.Message.Should().Be(message);
        }

        [Fact(DisplayName = "The caller can specify a custom exception that MustBeType must raise instead of the default one.")]
        public void CustomException()
        {
            object @object = "Wow!";
            var exception = new Exception();

            Action act = () => @object.MustBeType<Stream>(exception: exception);

            act.ShouldThrow<Exception>().Which.Should().BeSameAs(exception);
        }
    }
}