using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.TypeAssertionsTests
{
    public sealed class MustDeriveFromOrImplementTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustDeriveFromOrImplement must throw a TypeException when the specified type does not derive from or implement baseClassOrInterfaceType.")]
        public void DoesNotDeriveFromOrImplement()
        {
            var first = typeof(ObservableCollection<object>);
            var second = typeof(IFormattable);

            Action act = () => first.MustDeriveFromOrImplement(second, parameterName: nameof(first));

            act.ShouldThrow<TypeException>()
               .And.Message.Should().Contain($"{nameof(first)} \"{first}\" must derive from or implement \"{second}\", but it does not.");
        }

        [Fact(DisplayName = "MustDeriveFromOrImplement must not throw an exception when the specified type derives from or implements baseClassOrInterfaceType.")]
        public void DerivesFromOrImplements()
        {
            var first = typeof(Dictionary<string, object>);
            var second = typeof(IDictionary<,>);

            var result = first.MustDeriveFromOrImplement(second);

            result.Should().BeSameAs(first);
        }

        [Fact(DisplayName = "MustDeriveFromOrImplement must throw an ArgumentNullException when parameter or baseClassOrInterfaceType is null.")]
        public void ParametersNull()
        {
            new Action(() => ((Type) null).MustDeriveFromOrImplement(typeof(object))).ShouldThrow<ArgumentNullException>();
            new Action(() => typeof(decimal).MustDeriveFromOrImplement(null)).ShouldThrow<ArgumentNullException>();
        }


        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.AddExceptionTest(exception => typeof(string).MustDeriveFromOrImplement(typeof(double), exception: exception))
                    .AddMessageTest<TypeException>(message => typeof(decimal).MustDeriveFromOrImplement(typeof(ICollection<>), message: message));
        }
    }
}