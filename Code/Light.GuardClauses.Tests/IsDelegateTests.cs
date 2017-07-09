using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class IsDelegateTests
    {
        [Fact]
        public void Class()
        {
            TestIsDelegate(typeof(string), false);
            TestIsDelegate(typeof(object), false);
            TestIsDelegate(typeof(Dictionary<string, object>), false);
            TestIsDelegate(typeof(ObservableCollection<>), false);
            TestIsDelegate(typeof(Enum), false);
            TestIsDelegate(typeof(ValueType), false);
            TestIsDelegate(typeof(Delegate), false);
            TestIsDelegate(typeof(MulticastDelegate), false);
        }

        [Fact]
        public void ValueType()
        {
            TestIsDelegate(typeof(int), false);
            TestIsDelegate(typeof(double), false);
            TestIsDelegate(typeof(decimal), false);
        }

        [Fact]
        public void Interface()
        {
            TestIsDelegate(typeof(IDisposable), false);
            TestIsDelegate(typeof(IDictionary<,>), false);
            TestIsDelegate(typeof(IEnumerable<>), false);
        }

        [Fact]
        public void Delegate()
        {
            TestIsDelegate(typeof(Action), true);
            TestIsDelegate(typeof(Action<>), true);
            TestIsDelegate(typeof(Func<IEqualityComparer<object>>), true);
        }

        [Fact]
        public void Enum()
        {
            TestIsDelegate(typeof(ConsoleColor), false);
            TestIsDelegate(typeof(UriKind), false);
        }

        [Fact]
        public void ArgumentNull()
        {
            var type = default(Type);

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => type.IsDelegate();

            act.ShouldThrow<ArgumentNullException>()
               .And.ParamName.Should().Be(nameof(type));
        }

        private static void TestIsDelegate(Type type, bool expected)
        {
            type.IsDelegate().Should().Be(expected);
        }
    }
}