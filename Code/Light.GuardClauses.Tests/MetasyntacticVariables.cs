using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit.Sdk;

namespace Light.GuardClauses.Tests
{
    public static class MetasyntacticVariables
    {
        public const string Foo = "Foo";
        public const string Bar = "Bar";
        public const string Baz = "Baz";
        public const string Qux = "Qux";
        public const string Quux = "Quux";
        public const string Corge = "Corge";
        public const string Grault = "Grault";
        public const string Garply = "Garply";
        public const string Waldo = "Waldo";
        public const string Fred = "Fred";
        public const string Plugh = "Plugh";
        public const string Xyzzy = "Xyzzy";
        public const string Thud = "Thud";

        public static readonly IReadOnlyList<string> All =
            new[]
            {
                Foo,
                Bar,
                Baz,
                Qux,
                Quux,
                Corge,
                Grault,
                Garply,
                Waldo,
                Fred,
                Plugh,
                Xyzzy,
                Thud
            };
    }

    public sealed class MetasyntacticVariablesData : DataAttribute
    {
        private static readonly List<object[]> TransformedVariables =
            MetasyntacticVariables.All.Select(variable => new object[] { variable }).ToList();

        private readonly int _numberOfConstants;

        public MetasyntacticVariablesData(int numberOfConstants = 3) => _numberOfConstants = numberOfConstants;

        public override IEnumerable<object[]> GetData(MethodInfo testMethod) => TransformedVariables.Take(_numberOfConstants);
    }
}