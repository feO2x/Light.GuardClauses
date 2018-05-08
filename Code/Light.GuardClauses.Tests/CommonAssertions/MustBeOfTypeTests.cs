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
            object reference = MetasyntacticVariables.Foo;

            Action act = () => reference.MustBeOfType<Array>(nameof(reference));

            act.Should().Throw<TypeCastException>()
               .And.Message.Should().Contain($"{nameof(reference)} \"{reference}\" cannot be casted to \"{typeof(Array)}\".");
        }

        [Fact]
        public static void Downcast() =>
            MetasyntacticVariables.Bar.MustBeOfType<string>().Should().BeSameAs(MetasyntacticVariables.Bar);

        [Fact]
        public static void Cast() =>
            MetasyntacticVariables.Baz.MustBeOfType<IConvertible>().Should().BeSameAs(MetasyntacticVariables.Baz);
        

        [Fact]
        public static void ReferenceIsNull()
        {
            Action act = () => ((object) null).MustBeOfType<string>(MetasyntacticVariables.Foo);

            act.Should().Throw<ArgumentNullException>()
               .And.ParamName.Should().Be(MetasyntacticVariables.Foo);
        }

        [Fact]
        public static void CustomException() =>
            CustomExceptions.TestCustomException(exceptionFactory => 42.MustBeOfType<IDisposable>(exceptionFactory));

        [Fact]
        public static void CustomExceptionWithParameter() =>
            CustomExceptions.TestCustomException<object>(MetasyntacticVariables.Foo,
                                                         (value, exceptionFactory) => value.MustBeOfType<Encoding>(exceptionFactory));


        [Fact]
        public static void CustomExceptionReferenceIsNull()
        {
            Action act = () => ((object) null).MustBeOfType<StreamReader>(() => new Exception(), MetasyntacticVariables.Bar);

            act.Should().Throw<ArgumentNullException>()
               .And.ParamName.Should().Be(MetasyntacticVariables.Bar);
        }

        [Fact]
        public static void CustomExceptionWithParameterReferenceIsNull()
        {
            Action act = () => ((object)null).MustBeOfType<StreamReader>(_ => new Exception(), MetasyntacticVariables.Baz);

            act.Should().Throw<ArgumentNullException>()
               .And.ParamName.Should().Be(MetasyntacticVariables.Baz);
        }
    }
}