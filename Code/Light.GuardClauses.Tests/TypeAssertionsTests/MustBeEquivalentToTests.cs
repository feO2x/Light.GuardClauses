using System;
using System.Collections.Generic;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.TypeAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class MustBeEquivalentToTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustBeEquivalentTo must throw a TypeException when the two types are not equivalent.")]
        public void TypesNotEquivalent()
        {
            var first = typeof(double);
            var second = typeof(Dictionary<,>);

            Action act = () => first.MustBeEquivalentTo(second, nameof(first));

            act.ShouldThrow<TypeException>()
               .And.Message.Should().Contain($"first \"{first}\" must be equivalent to \"{second}\", but it is not.");
        }

        [Fact(DisplayName = "MustBeEquivalentTo must not throw an exception when the two types are equivalent.")]
        public void TypeEquivalent()
        {
            var first = typeof(IList<string>);
            var second = typeof(IList<>);

            Action act = () => first.MustBeEquivalentTo(second);

            act.ShouldNotThrow();
        }

        [Fact(DisplayName = "MustBeEquivalentTo must throw an ArgumentNullException when parameter is null.")]
        public void ParameterNull()
        {
            Action act = () => ((Type) null).MustBeEquivalentTo(typeof(string), "foo");

            act.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("foo");
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => typeof(string).MustBeEquivalentTo(typeof(int), exception: exception)))
                    .Add(new CustomMessageTest<TypeException>(message => typeof(Delegate).MustBeEquivalentTo(typeof(Math), message: message)));
        }
    }
}