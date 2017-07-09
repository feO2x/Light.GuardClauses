using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    [Trait("Category", "Functional Tests")]
    public sealed class IsClassTests
    {
        [Fact]
        public void Class()
        {
            TestIsClass(typeof(string), true);
            TestIsClass(typeof(object), true);
            TestIsClass(typeof(Dictionary<string, object>), true);
            TestIsClass(typeof(ObservableCollection<>), true);
            TestIsClass(typeof(Enum), true);
            TestIsClass(typeof(ValueType), true);
            TestIsClass(typeof(Delegate), true);
            TestIsClass(typeof(MulticastDelegate), true);
        }

        [Fact]
        public void ValueType()
        {
            TestIsClass(typeof(int), false);
            TestIsClass(typeof(double), false);
            TestIsClass(typeof(decimal), false);
        }

        [Fact]
        public void Interface()
        {
            TestIsClass(typeof(IDisposable), false);
            TestIsClass(typeof(IDictionary<,>), false);
            TestIsClass(typeof(IEnumerable<>), false);
        }

        [Fact]
        public void Delegate()
        {
            TestIsClass(typeof(Action), false);
            TestIsClass(typeof(Action<>), false);
            TestIsClass(typeof(Func<IEqualityComparer<object>>), false);
        }

        [Fact]
        public void Enum()
        {
            TestIsClass(typeof(ConsoleColor), false);
            TestIsClass(typeof(UriKind), false);
        }

        [Fact]
        public void ArgumentNull()
        {
            var type = default(Type);

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => type.IsClass();

            act.ShouldThrow<ArgumentNullException>()
               .And.ParamName.Should().Be(nameof(type));
        }

        private static void TestIsClass(Type type, bool expected)
        {
            type.IsClass().Should().Be(expected);
        }
    }
}