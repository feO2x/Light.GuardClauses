using System;
using System.Collections.ObjectModel;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.TypeAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class MustNotDeriveFromTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustNotDeriveFrom must throw a TypeException when the specified type does derive from the other type.")]
        public void DerivesFrom()
        {
            var first = typeof(Action<string>);
            var second = typeof(Delegate);

            Action act = () => first.MustNotDeriveFrom(second, parameterName: nameof(first));

            act.ShouldThrow<TypeException>()
               .And.Message.Should().Contain($"{nameof(first)} \"{first}\" must not derive from \"{second}\", but it does.");
        }

        [Fact(DisplayName = "MustNotDeriveFrom must not throw an exception when the specified type does not derive from the other type.")]
        public void DoesNotDeriveFrom()
        {
            var type = typeof(ObservableCollection<string>);

            var result = type.MustNotDeriveFrom(typeof(Activator));

            result.Should().BeSameAs(type);
        }

        [Fact(DisplayName = "MustNotDeriveFrom must throw an ArgumentNullException when either parameter or other is null.")]
        public void ParameterNull()
        {
            new Action(() => ((Type) null).MustNotDeriveFrom(typeof(object))).ShouldThrow<ArgumentNullException>();
            new Action(() => typeof(string).MustNotDeriveFrom(null)).ShouldThrow<ArgumentNullException>();
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.AddExceptionTest(exception => typeof(int).MustNotDeriveFrom(typeof(ValueType), exception: exception))
                    .AddMessageTest<TypeException>(message => typeof(ArgumentException).MustNotDeriveFrom(typeof(Exception), message: message));
        }
    }
}