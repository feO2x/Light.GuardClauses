using System;
using System.IO;
using System.Text;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertions
{
    public static class MustBeOfTypeTests
    {
        [Fact]
        public static void CastNotPossible()
        {
            object reference = Metasyntactic.Foo;

            Action act = () => reference.MustBeOfType<Array>(nameof(reference));

            var exceptionAssertions = act.Should().Throw<TypeCastException>().Which;
            exceptionAssertions.Message.Should().Contain($"{nameof(reference)} \"{reference}\" cannot be casted to \"{typeof(Array)}\".");
            exceptionAssertions.ParamName.Should().BeSameAs(nameof(reference));
        }

        [Fact]
        public static void Downcast() =>
            Metasyntactic.Bar.MustBeOfType<string>().Should().BeSameAs(Metasyntactic.Bar);

        [Fact]
        public static void Cast() =>
            Metasyntactic.Baz.MustBeOfType<IConvertible>().Should().BeSameAs(Metasyntactic.Baz);

        [Fact]
        public static void ReferenceIsNull()
        {
            Action act = () => ((object) null).MustBeOfType<string>(Metasyntactic.Foo, Metasyntactic.Bar);

            var exceptionAssertion = act.Should().Throw<ArgumentNullException>().Which;
            exceptionAssertion.ParamName.Should().Be(Metasyntactic.Foo);
            exceptionAssertion.Message.Should().Contain(Metasyntactic.Bar);
        }

        [Fact]
        public static void CustomException() =>
            Test.CustomException<object>(Metasyntactic.Foo,
                                         (value, exceptionFactory) => value.MustBeOfType<Encoding>(exceptionFactory));


        [Fact]
        public static void CustomExceptionArgumentNull() => 
            Test.CustomException<object>(null,
                                         (nullReference, exceptionFactory) => nullReference.MustBeOfType<string>(exceptionFactory));

        [Fact]
        public static void CustomExceptionDowncastValid()
        {
            var encoding = Encoding.UTF8;
            encoding.MustBeOfType<UTF8Encoding>(e => null).Should().BeSameAs(encoding);
        }
        
        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<TypeCastException>(message => "Foo".MustBeOfType<StreamReader>(message: message));
    }
}