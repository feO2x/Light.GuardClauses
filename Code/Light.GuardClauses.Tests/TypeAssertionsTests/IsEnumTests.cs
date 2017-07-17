using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.TypeAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class IsEnumTests
    {
        [Fact]
        public void Class()
        {
            TestIsEnum(typeof(string), false);
            TestIsEnum(typeof(object), false);
            TestIsEnum(typeof(Dictionary<string, object>), false);
            TestIsEnum(typeof(ObservableCollection<>), false);
            TestIsEnum(typeof(Enum), false);
            TestIsEnum(typeof(ValueType), false);
            TestIsEnum(typeof(Delegate), false);
            TestIsEnum(typeof(MulticastDelegate), false);
        }

        [Fact]
        public void ValueType()
        {
            TestIsEnum(typeof(int), false);
            TestIsEnum(typeof(double), false);
            TestIsEnum(typeof(decimal), false);
        }

        [Fact]
        public void Interface()
        {
            TestIsEnum(typeof(IDisposable), false);
            TestIsEnum(typeof(IDictionary<,>), false);
            TestIsEnum(typeof(IEnumerable<>), false);
        }

        [Fact]
        public void Delegate()
        {
            TestIsEnum(typeof(Action), false);
            TestIsEnum(typeof(Action<>), false);
            TestIsEnum(typeof(Func<IEqualityComparer<object>>), false);
        }

        [Fact]
        public void Enum()
        {
            TestIsEnum(typeof(ConsoleColor), true);
            TestIsEnum(typeof(UriKind), true);
        }

        [Fact]
        public void ArgumentNull()
        {
            var type = default(Type);

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => type.IsEnum();

            act.ShouldThrow<ArgumentNullException>()
               .And.ParamName.Should().Be(nameof(type));
        }

        private static void TestIsEnum(Type type, bool expected)
        {
            type.IsEnum().Should().Be(expected);
        }
    }
}