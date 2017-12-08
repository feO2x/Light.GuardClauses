using System;
using System.Collections.ObjectModel;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.TypeAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class MustDeriveFromTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustDeriveFrom must throw a TypeException when the specified type does not derive from baseClass.")]
        public void DoesNotDeriveFrom()
        {
            var first = typeof(string);
            var second = typeof(double);

            Action act = () => first.MustDeriveFrom(second, parameterName: nameof(first));

            act.ShouldThrow<TypeException>()
               .And.Message.Should().Contain($"{nameof(first)} \"{first}\" must derive from \"{second}\", but it does not.");
        }

        [Fact(DisplayName = "MustDeriveFrom must not throw an exception when the specified type derives from baseClass.")]
        public void DerivesFrom()
        {
            var type = typeof(ObservableCollection<string>);
            
            var result = type.MustDeriveFrom(typeof(Collection<>));

            result.Should().BeSameAs(type);
        }

        [Fact(DisplayName = "MustDeriveFrom must throw an ArgumentNullException when either parameter or baseClass is null.")]
        public void ParameterNull()
        {
            new Action(() => ((Type) null).MustDeriveFrom(typeof(object))).ShouldThrow<ArgumentNullException>();
            new Action(() => typeof(string).MustDeriveFrom(null)).ShouldThrow<ArgumentNullException>();
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.AddExceptionTest(exception => typeof(int).MustDeriveFrom(typeof(double), exception: exception))
                    .AddMessageTest<TypeException>(message => typeof(Action<string>).MustDeriveFrom(typeof(ValueType), message: message));
        }
    }
}