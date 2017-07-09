using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class IsInterfaceTests
    {
        [Fact]
        public void Class()
        {
            TestIsInterface(typeof(string), false);
            TestIsInterface(typeof(object), false);
            TestIsInterface(typeof(Dictionary<string, object>), false);
            TestIsInterface(typeof(ObservableCollection<>), false);
            TestIsInterface(typeof(Enum), false);
            TestIsInterface(typeof(ValueType), false);
            TestIsInterface(typeof(Delegate), false);
            TestIsInterface(typeof(MulticastDelegate), false);
        }

        [Fact]
        public void ValueType()
        {
            TestIsInterface(typeof(int), false);
            TestIsInterface(typeof(double), false);
            TestIsInterface(typeof(decimal), false);
            TestIsInterface(typeof(Nullable<>), false);
            TestIsInterface(typeof(DateTime?), false);
        }

        [Fact]
        public void Interface()
        {
            TestIsInterface(typeof(IDisposable), true);
            TestIsInterface(typeof(IDictionary<,>), true);
            TestIsInterface(typeof(IEnumerable<object>), true);
        }

        [Fact]
        public void Delegate()
        {
            TestIsInterface(typeof(Action), false);
            TestIsInterface(typeof(Action<>), false);
            TestIsInterface(typeof(Func<IEqualityComparer<object>>), false);
        }

        [Fact]
        public void Enum()
        {
            TestIsInterface(typeof(ConsoleColor), false);
            TestIsInterface(typeof(UriKind), false);
        }

        [Fact]
        public void ArgumentNull()
        {
            var type = default(Type);

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => type.IsInterface();

            act.ShouldThrow<ArgumentNullException>()
               .And.ParamName.Should().Be(nameof(type));
        }

        private static void TestIsInterface(Type type, bool expected)
        {
            type.IsInterface().Should().Be(expected);
        }
    }
}