using System;
using System.Text;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests
{
    public sealed class MustBeSubsetOfTests
    {
        [Theory(DisplayName = "MustBeSubsetOf must throw a CollectionException when the specified collection is no subset of superset.")]
        [MemberData(nameof(IsNotSubsetData))]
        public void IsNotSubset(string[] collection, string[] superset)
        {
            Action act = () => collection.MustBeSubsetOf(superset, nameof(collection));

            act.ShouldThrow<CollectionException>()
               .And.Message.Should().Contain($"{nameof(collection)} must be a subset of:{Environment.NewLine}{new StringBuilder().AppenItemsWithNewLine(superset)}");
        }

        public static readonly TestData IsNotSubsetData =
            new[]
            {
                new object[] { new[] { "a", "b", "c" }, new[] { "a", "b" } },
                new object[] { new[] { "a", "b", "c" }, new[] { "c", "d", "a" } },
                new object[] { new[] { "Hello", "There" }, new[] { "Hello", "World" } },
                new object[] { new[] { "1", "2", "-12" }, new[] { "1", "-12" } }
            };

        [Theory(DisplayName = "MustBeSubsetOf must not throw an exception when the specified collection is a subset of superset.")]
        [MemberData(nameof(IsSubsetData))]
        public void IsSubset(char[] collection, char[] superset)
        {
            Action act = () => collection.MustBeSubsetOf(superset);

            act.ShouldNotThrow();
        }

        public static readonly TestData IsSubsetData =
            new[]
            {
                new object[] { new[] { 'a', 'b' }, new[] { 'a', 'b' } },
                new object[] { new[] { 'a', 'b' }, new[] { 'a', 'b', 'c' } },
                new object[] { new[] { 'X', 'Y' }, new[] { 'X', 'Z', 'Y' } },
                new object[] { new[] { 'f', 'p' }, new[] { 'a', 'p', 's', 'e', 'f' } },
                new object[] { new[] { 'a', 'b', 'a' }, new[] { 'a', 'b' } }
            };

        [Fact(DisplayName = "MustBeSubsetOf must throw an ArgumentNullException when superset is null.")]
        public void SupersetNull()
        {
            Action act = () => new[] { 'a' }.MustBeSubsetOf(null);

            act.ShouldThrow<ArgumentNullException>()
               .And.ParamName.Should().Be("superset");
        }

        [Fact(DisplayName = "The caller can specify a custom message that MustBeSubsetOf must inject instead of the default one.")]
        public void CustomMessage()
        {
            const string message = "Thou must be a subset of the other!";

            Action act = () => new[] { 'a', 'b' }.MustBeSubsetOf(new[] { 'a' }, message: message);

            act.ShouldThrow<CollectionException>()
               .And.Message.Should().Contain(message);
        }

        [Fact(DisplayName = "The caller can specify a custom exception that MustBeSubsetOf must raise instead of the default one.")]
        public void CustomException()
        {
            var exception = new Exception();

            Action act = () => new[] { 'a', 'b' }.MustBeSubsetOf(new[] { 'a' }, exception: exception);

            act.ShouldThrow<Exception>().Which.Should().BeSameAs(exception);
        }
    }
}