using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FluentAssertions;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests
{
    public static class AsArrayTests
    {
        [Theory(DisplayName = "AsArray must downcast an existing array if possible.")]
        [InlineData(new[] { 1, 2, 3 })]
        [InlineData(new[] { 89, 87 })]
        [InlineData(new int[] { })]
        public static void DowncastArray(int[] array)
        {
            var result = array.AsArray();

            result.Should().BeSameAs(array);
        }

        [Theory(DisplayName = "AsArray must create a new array when the value passed in is no array.")]
        [MemberData(nameof(NotArrayData))]
        public static void CreateNewArray(IEnumerable<string> enumerable)
        {
            // ReSharper disable PossibleMultipleEnumeration
            var result = enumerable.AsArray();

            result.Should().NotBeSameAs(enumerable);
            result.Length.Should().Be(enumerable.Count());
            result.Should().Equal(enumerable);
            // ReSharper restore PossibleMultipleEnumeration
        }

        public static readonly TestData NotArrayData =
            new[]
            {
                new object[] { new List<string> { "Foo", "Bar", "Baz" } },
                new object[] { new ObservableCollection<string>() },
                new object[] { LazyStringCollection }
            };

        public static IEnumerable<string> LazyStringCollection
        {
            get
            {
                yield return "Foo";
                yield return "Baz";
                yield return "Quux";
            }
        }
    }
}