using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.TypeAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class MustImplementTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustImplement must throw a TypeException when the specified type does not implement interfaceType.")]
        public void DoesNotImplement()
        {
            var first = typeof(int);
            var second = typeof(IDisposable);

            Action act = () => first.MustImplement(second, parameterName: nameof(first));

            act.ShouldThrow<TypeException>()
               .And.Message.Should().Contain($"{nameof(first)} \"{first}\" must implement \"{second}\", but it does not.");
        }

        [Fact(DisplayName = "MustImplement must not throw an exception when the specified type implements interfaceType.")]
        public void Implements()
        {
            var first = typeof(ObservableCollection<string>);
            var second = typeof(IList<>);

            var result = first.MustImplement(second);

            result.Should().BeSameAs(first);
        }

        [Fact(DisplayName = "MustImplement must throw an ArgumentNullException when either parameter or interfaceType is null.")]
        public void ParametersNull()
        {
            new Action(() => ((Type) null).MustImplement(typeof(IEnumerator))).ShouldThrow<ArgumentNullException>();
            new Action(() => typeof(double).MustImplement(null)).ShouldThrow<ArgumentNullException>();
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.AddExceptionTest(exception => typeof(Action).MustImplement(typeof(IProgress<>), exception: exception))
                    .AddMessageTest<TypeException>(message => typeof(Exception).MustImplement(typeof(IComparable), message: message));
        }
    }
}