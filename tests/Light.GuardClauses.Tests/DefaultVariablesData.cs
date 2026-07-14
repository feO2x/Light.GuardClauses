using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;
using Xunit.v3;

namespace Light.GuardClauses.Tests;

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
            "Thud",
        };

    private static readonly IReadOnlyCollection<ITheoryDataRow> TransformedVariables =
        All.Select(ITheoryDataRow (variable) => new TheoryDataRow(variable)).ToArray();

    private readonly int _numberOfConstants;

    public DefaultVariablesData(int numberOfConstants = 3) => _numberOfConstants = numberOfConstants;

    public override ValueTask<IReadOnlyCollection<ITheoryDataRow>> GetData(
        MethodInfo testMethod,
        DisposalTracker disposalTracker
    ) => new (TransformedVariables.Take(_numberOfConstants).ToArray());

    public override bool SupportsDiscoveryEnumeration() => true;
}
