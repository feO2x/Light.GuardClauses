using System;
using System.Collections.Generic;
using System.Reflection;

namespace Light.GuardClauses.Tests.TypeAssertions;

public static class GenericTypeKinds
{
    public static readonly Type NonGenericType = typeof(string);
    public static readonly Type ClosedConstructedGenericType = typeof(List<string>);
    public static readonly Type GenericTypeDefinition = typeof(IList<>);
    public static readonly Type OpenConstructedGenericType = typeof(GuidDictionary<>).GetTypeInfo().BaseType;
    public static readonly Type GenericTypeParameter = typeof(GuidDictionary<>).GetTypeInfo().GenericTypeParameters[0];


    public class GuidDictionary<T> : Dictionary<Guid, T> { }
}