using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.TypeAssertionsTests
{
    public sealed class MustNotBeEquivalentToTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustNotBeEquivalentTo must throw a TypeException when the two types are equivalent.")]
        public void TypesEquivalent()
        {
            var first = typeof(decimal);
            var second = typeof(decimal);

            Action act = () => first.MustNotBeEquivalentTo(second, nameof(first));

            act.ShouldThrow<TypeException>()
               .And.Message.Should().Contain($"first \"{first}\" must not be equivalent to \"{second}\", but it is.");
        }

        [Fact(DisplayName = "MustNotBeEquivalentTo must not throw an exception when the two types are not equivalent.")]
        public void TypeEquivalent()
        {
            var first = typeof(Expression);
            var second = typeof(IComparer<>);

            var result = first.MustNotBeEquivalentTo(second);

            result.Should().BeSameAs(first);
        }

        [Fact(DisplayName = "MustNotBeEquivalentTo must throw an ArgumentNullException when parameter is null.")]
        public void ParameterNull()
        {
            Action act = () => ((Type) null).MustNotBeEquivalentTo(typeof(string), "foo");

            act.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("foo");
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => typeof(string).MustNotBeEquivalentTo(typeof(string), exception: exception)))
                    .Add(new CustomMessageTest<TypeException>(message => typeof(string).MustNotBeEquivalentTo(typeof(string), message: message)));
        }
    }
}