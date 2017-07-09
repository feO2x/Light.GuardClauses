using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class IsStructTests
    {
        [Fact]
        public void Class()
        {
            TestIsStruct(typeof(string), false);
            TestIsStruct(typeof(object), false);
            TestIsStruct(typeof(Dictionary<string, object>), false);
            TestIsStruct(typeof(ObservableCollection<>), false);
            TestIsStruct(typeof(Enum), false);
            TestIsStruct(typeof(ValueType), false);
            TestIsStruct(typeof(Delegate), false);
            TestIsStruct(typeof(MulticastDelegate), false);
        }

        [Fact]
        public void ValueType()
        {
            TestIsStruct(typeof(int), true);
            TestIsStruct(typeof(double), true);
            TestIsStruct(typeof(decimal), true);
            TestIsStruct(typeof(Nullable<>), true);
            TestIsStruct(typeof(DateTime?), true);
        }

        [Fact]
        public void Interface()
        {
            TestIsStruct(typeof(IDisposable), false);
            TestIsStruct(typeof(IDictionary<,>), false);
            TestIsStruct(typeof(IEnumerable<>), false);
        }

        [Fact]
        public void Delegate()
        {
            TestIsStruct(typeof(Action), false);
            TestIsStruct(typeof(Action<>), false);
            TestIsStruct(typeof(Func<IEqualityComparer<object>>), false);
        }

        [Fact]
        public void Enum()
        {
            TestIsStruct(typeof(ConsoleColor), false);
            TestIsStruct(typeof(UriKind), false);
        }

        [Fact]
        public void ArgumentNull()
        {
            var type = default(Type);

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => type.IsStruct();

            act.ShouldThrow<ArgumentNullException>()
               .And.ParamName.Should().Be(nameof(type));
        }

        private static void TestIsStruct(Type type, bool expected)
        {
            type.IsStruct().Should().Be(expected);
        }
    }
}