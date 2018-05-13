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

            act.Should().Throw<TypeCastException>()
               .And.Message.Should().Contain($"{nameof(reference)} \"{reference}\" cannot be casted to \"{typeof(Array)}\".");
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
            Action act = () => ((object) null).MustBeOfType<string>(Metasyntactic.Foo);

            act.Should().Throw<ArgumentNullException>()
               .And.ParamName.Should().Be(Metasyntactic.Foo);
        }

        [Fact]
        public static void CustomException() =>
            Test.CustomException<object>(Metasyntactic.Foo,
                                         (value, exceptionFactory) => value.MustBeOfType<Encoding>(exceptionFactory));


        [Fact]
        public static void CustomExceptionReferenceIsNull()
        {
            Action act = () => ((object) null).MustBeOfType<StreamReader>(_ => new Exception(), Metasyntactic.Baz);

            act.Should().Throw<ArgumentNullException>()
               .And.ParamName.Should().Be(Metasyntactic.Baz);
        }

        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<TypeCastException>(message => "Foo".MustBeOfType<StreamReader>(message: message));
    }
}