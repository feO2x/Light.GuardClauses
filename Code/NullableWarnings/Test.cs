using Light.GuardClauses;

namespace NullableWarnings;

// ReSharper disable once UnusedType.Global
public static class Test
{
    // ReSharper disable once UnusedMember.Global
    public static void OperateOnObject(object @object)
    {
        @object.MustNotBeNull();
        _ = @object.ToString();
    }
}

