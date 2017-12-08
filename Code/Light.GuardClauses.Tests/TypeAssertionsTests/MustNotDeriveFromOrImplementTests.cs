using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.TypeAssertionsTests
{
    public sealed class MustNotDeriveFromOrImplementTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustNotDeriveFromOrImplement must throw a TypeException when the specified type does derive from or implement the other type.")]
        public void DerivesFromOrImplements()
        {
            var first = typeof(ConsoleColor);
            var second = typeof(Enum);

            Action act = () => first.MustNotDeriveFromOrImplement(second, parameterName: nameof(first));

            act.ShouldThrow<TypeException>()
               .And.Message.Should().Contain($"{nameof(first)} \"{first}\" must not derive from or implement \"{second}\", but it does.");
        }

        [Fact(DisplayName = "MustNotDeriveFromOrImplement must not throw an exception when the specified type does not derive from or implement the other type.")]
        public void DoesNotDeriveFromOrImplement()
        {
            var first = typeof(int);
            var second = typeof(bool);

            var result = first.MustNotDeriveFromOrImplement(second);

            result.Should().BeSameAs(first);
        }

        [Fact(DisplayName = "MustNotDeriveFromOrImplement must throw an ArgumentNullException when parameter or other is null.")]
        public void ParametersNull()
        {
            new Action(() => ((Type) null).MustNotDeriveFromOrImplement(typeof(object))).ShouldThrow<ArgumentNullException>();
            new Action(() => typeof(decimal).MustNotDeriveFromOrImplement(null)).ShouldThrow<ArgumentNullException>();
        }


        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.AddExceptionTest(exception => typeof(string).MustNotDeriveFromOrImplement(typeof(object), exception: exception))
                    .AddMessageTest<TypeException>(message => typeof(decimal).MustNotDeriveFromOrImplement(typeof(ValueType), message: message));
        }
    }
}