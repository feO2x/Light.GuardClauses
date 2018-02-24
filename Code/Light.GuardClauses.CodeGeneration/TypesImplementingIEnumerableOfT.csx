#! "netcoreapp2.0"
#r "nuget: Light.GuardClauses, 3.5.0"

using Light.GuardClauses;

var enumerableTypes = typeof(IEnumerable<>).Assembly
                                           .ExportedTypes
                                           .Where(type => type.IsImplementing(typeof(IEnumerable<>)))
                                           .ToList();

Console.WriteLine($"Found {enumerableTypes.Count} types that implement IEnumerable<T>:");
foreach (var type in enumerableTypes)
{
    Console.WriteLine(type);
}