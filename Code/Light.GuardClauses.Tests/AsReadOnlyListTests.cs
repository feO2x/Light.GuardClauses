using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using FluentAssertions;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class AsReadOnlyListTests
    {
        [Theory(DisplayName = "AsReadOnlyList will return the same collection instance casted as an IReadOnlyList<T> when the cast is possible.")]
        [MemberData(nameof(ReturnsCastedInstanceIfPossibleData))]
        public void ReturnsCastedInstanceIfPossible<T>(IEnumerable<T> enumerable)
        {
            // ReSharper disable PossibleMultipleEnumeration
            var readOnlyList = enumerable.AsReadOnlyList();

            readOnlyList.Should().BeSameAs(enumerable);
            // ReSharper restore PossibleMultipleEnumeration
        }

        public static readonly TestData ReturnsCastedInstanceIfPossibleData =
            new[]
            {
                new object[] { new List<string> { "Foo", "Bar" } },
                new object[] { new[] { "Foo", "Bar", "Baz" } },
                new object[] { new List<object>() },
                new object[] { new ObservableCollection<Encoding> { Encoding.ASCII, Encoding.UTF8 } }
            };

        [Fact(DisplayName = "AsReadOnlyList will return a new List<T> instance when the enumerable cannot be downcasted to IReadOnlyList<T>.")]
        public void ReturnsNewListIfCastNotPossible()
        {
            // ReSharper disable PossibleMultipleEnumeration
            var enumerable = Enumerable.Range(1, 5);

            var list = enumerable.AsReadOnlyList();

            list.Should().NotBeSameAs(enumerable);
            list.Should().BeOfType<List<int>>();
            list.Should().Equal(enumerable);
            // ReSharper restore PossibleMultipleEnumeration
        }

        [Fact(DisplayName = "AsReadOnlyList must throw an ArgumentNullException when the specified collection is null.")]
        public void EnumerableNull()
        {
            Action act = () => ((IEnumerable<string>) null).AsReadOnlyList();

            act.ShouldThrow<ArgumentNullException>()
               .And.ParamName.Should().Be("enumerable");
        }
    }
}