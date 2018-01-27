using System;
using System.IO;
using System.Text;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertionsTests
{
    public sealed class MustBeOfTypeTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustBeOfType must throw an exception when object cannot be downcasted.")]
        public static void TypeMismatch()
        {
            object @object = "Hey";

            Action act = () => @object.MustBeOfType<Array>(nameof(@object));

            act.ShouldThrow<TypeMismatchException>()
               .And.Message.Should().Contain($"{nameof(@object)} \"{@object}\" cannot be downcasted to \"{typeof(Array)}\".");
        }

        [Fact(DisplayName = "MustBeOfType must return the downcasted object if cast succeeds.")]
        public static void TypeDowncasted()
        {
            const string @string = "Hey";
            object @object = @string;

            var downcastedValue = @object.MustBeOfType<string>(nameof(@object));
            downcastedValue.Should().BeSameAs(@string);
        }

        [Fact(DisplayName = "MustBeOfType must throw an ArgumentNullException when the specified reference is null.")]
        public static void NullReference()
        {
            Action act = () => ((object) null).MustBeOfType<string>("parameter");

            act.ShouldThrow<ArgumentNullException>()
               .And.ParamName.Should().Be("parameter");
        }

        [Fact(DisplayName = "MustBeOfType with custom exception must throw an ArgumentNullException when the specified reference is null.")]
        public static void NullReferenceWithCustomException()
        {
            Action act = () => ((object) null).MustBeOfType<StreamReader>(() => new Exception(), "Foo");

            act.ShouldThrow<ArgumentNullException>()
               .And.ParamName.Should().Be("Foo");
        }

        [Fact(DisplayName = "MustBeOfType must throw the custom exception and pass in the parameter if a downcast is not possible.")]
        public static void CustomParameterizedException()
        {
            object reference = "Foo";
            var observerdReference = default(object);
            var exception = new Exception();

            Action act = () => reference.MustBeOfType<Array>(r =>
                                                             {
                                                                 observerdReference = r;
                                                                 return exception;
                                                             });

            act.ShouldThrow<Exception>().Which.Should().BeSameAs(exception);
            observerdReference.Should().BeSameAs(reference);
        }

        [Fact(DisplayName = "MustBeOfType with custom parameterized exception must throw an ArgumentNullException when the specified reference is null.")]
        public static void NullReferenceWithCustomParameterizedException()
        {
            Action act = () => ((object)null).MustBeOfType<StreamWriter>(parameter => new Exception(), "Bar");

            act.ShouldThrow<ArgumentNullException>()
               .And.ParamName.Should().Be("Bar");
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => "Wow!".MustBeOfType<Stream>(exception)))
                    .Add(new CustomMessageTest<TypeMismatchException>(message => "Hello".MustBeOfType<StringBuilder>(message: message)));
        }
    }
}