#! "netcoreapp2.0"
#r "nuget: Light.GuardClauses 3.5.0"
#load "CSharpCodeFileWriter.csx"

using Light.GuardClauses;

public abstract class CollectionTypeInfo
{
    protected CollectionTypeInfo(string collectionType, string itemTypeName, string countProperty = "Count")
    {
        CollectionType = collectionType.MustNotBeNullOrWhiteSpace(nameof(collectionType));
        ItemTypeName = itemTypeName.MustNotBeNullOrWhiteSpace(nameof(itemTypeName));
        CountProperty = countProperty.MustNotBeNullOrWhiteSpace(nameof(countProperty));
    }

    public string CollectionType { get; }
    public string ItemTypeName { get; }
    public string CountProperty { get; }

    public abstract CSharpCodeFileWriter OpenLoop(CSharpCodeFileWriter writer, string variableName, string collectionVariableName, string counterName = "i");
}

public abstract class ForEachTypeCollection : CollectionTypeInfo
{
    protected ForEachTypeCollection(string collectionType, string itemTypeName, string countProperty = "Count")
        : base(collectionType, itemTypeName, countProperty) { }

    public override CSharpCodeFileWriter OpenLoop(CSharpCodeFileWriter writer, string variableName, string collectionVariableName, string counterName = "i")
    {
        return writer.WriteLine($"foreach (var {variableName} in {collectionVariableName})")
                     .OpenScopeAndIndent();
    }
}

public abstract class ForTypeCollection : CollectionTypeInfo
{
    protected ForTypeCollection(string collectionType, string itemTypeName, string countProperty = "Count")
        : base(collectionType, itemTypeName, countProperty) { }

    public override CSharpCodeFileWriter OpenLoop(CSharpCodeFileWriter writer, string variableName, string collectionVariableName, string counterName = "i")
    {
        return writer.WriteLine($"for (var {counterName} = 0; {counterName} < {collectionVariableName}.{CountProperty}; ++{counterName})")
                     .OpenScopeAndIndent()
                     .WriteLine($"var {variableName} = {collectionVariableName}[{counterName}];");
    }
}

public sealed class ArrayInfo : ForTypeCollection
{
    public ArrayInfo(string itemTypeName) : base($"{itemTypeName}[]", itemTypeName, "Length") { }
}


public sealed class ListInfo : ForTypeCollection
{
    public ListInfo(string itemTypeName) : base($"List<{itemTypeName}>", itemTypeName) { }
}

public sealed class AbstractReadOnlyListInfo : ForTypeCollection
{
    public AbstractReadOnlyListInfo(string itemTypeName) : base($"IReadOnlyList<{itemTypeName}>", itemTypeName) { }
}

public sealed class AbstractListInfo : ForTypeCollection
{
    public AbstractListInfo(string itemTypeName) : base($"IList<{itemTypeName}>", itemTypeName) { }
}

public sealed class ObservableCollectionInfo : ForTypeCollection
{
    public ObservableCollectionInfo(string itemTypeName) : base($"ObservableCollection<{itemTypeName}>", itemTypeName) { }
}

public sealed class CollectionInfo : ForTypeCollection
{
    public CollectionInfo(string itemTypeName) : base($"Collection<{itemTypeName}>", itemTypeName) { }
}

public sealed class ReadOnlyCollectionInfo : ForTypeCollection
{
    public ReadOnlyCollectionInfo(string itemTypeName) : base($"ReadOnlyCollection<{itemTypeName}>", itemTypeName) { }
}

public sealed class EnumerableInfo : ForEachTypeCollection
{
    public EnumerableInfo(string itemTypeName) : base($"IEnumerable<{itemTypeName}>", itemTypeName) { }
}