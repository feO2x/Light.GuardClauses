using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FluentAssertions;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;

namespace Light.GuardClauses.Tests.FrameworkExtensions
{
    public static class AsArrayTests
    {
        [Theory]
        [InlineData(new[] { 1, 2, 3 })]
        [InlineData(new[] { 89, 87 })]
        [InlineData(new int[] { })]
        public static void DowncastArray(int[] array)
        {
            var result = array.AsArray();

            result.Should().BeSameAs(array);
        }

        [Theory]
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

        [Fact]
        public static void EnumerableNull()
        {
            Action act = () => ((ObservableCollection<string>) null).AsArray();

            act.Should().Throw<ArgumentNullException>();
        }

        public static readonly TheoryData<IEnumerable<string>> NotArrayData =
            new TheoryData<IEnumerable<string>>
            {
                new List<string> { "Foo", "Bar", "Baz" },
                new ObservableCollection<string>(),
                LazyStringCollection
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