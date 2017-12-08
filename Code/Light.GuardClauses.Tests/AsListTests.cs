using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FluentAssertions;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class AsListTests
    {
        [Theory(DisplayName = "AsList will return the same collection instance casted as an IList<T> when the cast is possible.")]
        [MemberData(nameof(ReturnsCastedInstanceIfPossibleData))]
        public void ReturnsCastedInstanceIfPossible(IEnumerable<string> enumerable)
        {
            // ReSharper disable PossibleMultipleEnumeration
            var list = enumerable.AsList();

            list.Should().BeSameAs(enumerable);
            // ReSharper restore PossibleMultipleEnumeration
        }

        [Theory(DisplayName = "AsList will return the same collection instance casted as an IList<T> when the cast is possible, even when a custom Create-Collection-Delegate was passed in.")]
        [MemberData(nameof(ReturnsCastedInstanceIfPossibleData))]
        public void ReturnsCastedInstanceIfPossibleWithCustomFactory(IEnumerable<string> enumerable)
        {
            // ReSharper disable PossibleMultipleEnumeration
            var list = enumerable.AsList(items => new ObservableCollection<string>(items));

            list.Should().BeSameAs(enumerable);
            // ReSharper restore PossibleMultipleEnumeration
        }

        public static readonly TestData ReturnsCastedInstanceIfPossibleData =
            new[]
            {
                new object[] { new List<string> { "Foo", "Bar" } },
                new object[] { new[] { "Foo", "Bar", "Baz" } },
                new object[] { new ObservableCollection<string>() }
            };

        [Fact(DisplayName = "AsList will return a new List<T> instance when the enumerable cannot be downcasted to IList<T>.")]
        public void ReturnsNewListIfCastNotPossible()
        {
            // ReSharper disable PossibleMultipleEnumeration
            var enumerable = Enumerable.Range(1, 5);

            var list = enumerable.AsList();

            list.Should().NotBeSameAs(enumerable);
            list.Should().BeOfType<List<int>>();
            list.Should().Equal(enumerable);
            // ReSharper restore PossibleMultipleEnumeration
        }

        [Fact(DisplayName = "AsList must throw an ArgumentNullException when the specified collection is null.")]
        public void EnumerableNull()
        {
            Action act = () => ((IEnumerable<string>) null).AsList();

            act.ShouldThrow<ArgumentNullException>()
               .And.ParamName.Should().Be("enumerable");
        }

        [Fact(DisplayName = "AsList must allow the client to create a custom collection.")]
        public void CreateCollection()
        {
            var collection = LazyEnumerable().AsList(items => new ObservableCollection<string>(items));

            collection.Should().BeAssignableTo<ObservableCollection<string>>();
            collection.Should().Equal(LazyEnumerable());
        }

        private static IEnumerable<string> LazyEnumerable()
        {
            yield return "Foo";
            yield return "Bar";
            yield return "Baz";
        }
    }
}