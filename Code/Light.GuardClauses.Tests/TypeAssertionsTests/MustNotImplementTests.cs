using System;
using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.TypeAssertionsTests
{
    public sealed class MustNotImplementTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustNotImplement must throw a TypeException when the specified type implements the other type.")]
        public void Implements()
        {
            var first = typeof(double);
            var second = typeof(IComparable<double>);

            Action act = () => first.MustNotImplement(second, parameterName: nameof(first));

            act.Should().Throw<TypeException>()
               .And.Message.Should().Contain($"{nameof(first)} \"{first}\" must not implement \"{second}\", but it does.");
        }

        [Fact(DisplayName = "MustNotImplement must not throw an exception when the specified type does not implement the other type.")]
        public void DoesNotImplement()
        {
            var first = typeof(string);
            var second = typeof(IDictionary);

            var result = first.MustNotImplement(second);

            result.Should().BeSameAs(first);
        }

        [Fact(DisplayName = "MustNotImplement must throw an ArgumentNullException when either parameter or other is null.")]
        public void ParametersNull()
        {
            new Action(() => ((Type) null).MustNotImplement(typeof(IEnumerator))).Should().Throw<ArgumentNullException>();
            new Action(() => typeof(double).MustNotImplement(null)).Should().Throw<ArgumentNullException>();
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.AddExceptionTest(exception => typeof(string).MustNotImplement(typeof(IEnumerable<>), exception: exception))
                    .AddMessageTest<TypeException>(message => typeof(string).MustNotImplement(typeof(IEnumerable<char>), message: message));
        }
    }
}