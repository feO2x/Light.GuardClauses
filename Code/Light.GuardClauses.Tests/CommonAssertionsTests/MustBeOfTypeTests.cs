using System;
using System.IO;
using System.Text;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class MustBeOfTypeTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustBeOfType must throw an exception when object cannot be downcasted.")]
        public void TypeMismatch()
        {
            object @object = "Hey";

            Action act = () => @object.MustBeOfType<Array>(nameof(@object));

            act.ShouldThrow<TypeMismatchException>()
               .And.Message.Should().Contain($"{nameof(@object)} is of type {typeof(string).FullName} and cannot be downcasted to {typeof(Array).FullName}.");
        }

        [Fact(DisplayName = "MustBeOfType must return the downcasted object if cast succeeds.")]
        public void TypeDowncasted()
        {
            const string @string = "Hey";
            object @object = @string;

            var downcastedValue = @object.MustBeOfType<string>(nameof(@object));
            downcastedValue.Should().BeSameAs(@string);
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => "Wow!".MustBeOfType<Stream>(exception: exception)));

            testData.Add(new CustomMessageTest<TypeMismatchException>(message => "Hello".MustBeOfType<StringBuilder>(message: message)));
        }
    }
}