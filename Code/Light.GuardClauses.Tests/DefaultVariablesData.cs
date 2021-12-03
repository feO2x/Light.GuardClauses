using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit.Sdk;

namespace Light.GuardClauses.Tests
{
    public sealed class DefaultVariablesData : DataAttribute
    {
        public static readonly IReadOnlyList<string> All =
            new[]
            {
                "Foo",
                "Bar",
                "Baz",
                "Qux",
                "Quux",
                "Corge",
                "Grault",
                "Garply",
                "Waldo",
                "Fred",
                "Plugh",
                "Xyzzy",
                "Thud"
            };

        private static readonly List<object[]> TransformedVariables =
            All.Select(variable => new object[] { variable }).ToList();

        private readonly int _numberOfConstants;

        public DefaultVariablesData(int numberOfConstants = 3) => _numberOfConstants = numberOfConstants;

        public override IEnumerable<object[]> GetData(MethodInfo testMethod) => TransformedVariables.Take(_numberOfConstants);
    }
}