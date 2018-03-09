#! "netcoreapp2.0"
#r "nuget: Light.GuardClauses 3.5.0"
#load "CSharpCodeWriter.csx"
#load "AssertionOverload.csx"
#load "Namespaces.csx"

using Light.GuardClauses;

public class GuardClauseInfo
{
    public GuardClauseInfo(string assertionName,
                           string targetClass,
                           Parameter subject,
                           IReadOnlyList<AssertionOverload> overloads,
                           IReadOnlyList<string> requiredNamespaces,
                           string summaryComment,
                           string parameterComment,
                           Action<GuardClauseInfo, CSharpCodeWriter, AssertionOverload> writeBody,
                           string targetNamespace = Namespaces.LightGuardClauses)
    {
        overloads.MustHaveMinimumCount(1, nameof(overloads));
        requiredNamespaces.MustNotBeNull(nameof(requiredNamespaces));

        AssertionName = assertionName.MustNotBeNullOrWhiteSpace(nameof(assertionName));
        TargetClass = targetClass.MustNotBeNullOrWhiteSpace(nameof(targetClass));
        Subject = subject.MustNotBeNull(nameof(subject));
        Overloads = overloads;
        RequiredNamespaces = requiredNamespaces;
        SummaryComment = summaryComment.MustNotBeNullOrWhiteSpace(nameof(summaryComment));
        ParameterComment = parameterComment.MustNotBeNullOrWhiteSpace(nameof(summaryComment));
        WriteBody = writeBody.MustNotBeNull(nameof(writeBody));
        TargetNamespace = targetNamespace.MustNotBeNullOrWhiteSpace(nameof(targetNamespace));
    }

    public string AssertionName { get; }
    public string TargetClass { get; }
    public Parameter Subject { get; }
    public IReadOnlyList<AssertionOverload> Overloads { get; }
    public IReadOnlyList<string> RequiredNamespaces { get; }
    public string SummaryComment { get; }
    public string ParameterComment { get; }
    public Action<GuardClauseInfo, CSharpCodeWriter, AssertionOverload> WriteBody {get;}
    public string TargetNamespace { get; }

    public void WriteGuardClauseOverloads()
    {
        var stringBuilder = new StringBuilder();
        var writer = new CSharpCodeWriter(new StringWriter(stringBuilder));

        foreach (var @namespace in RequiredNamespaces.OrderBy(n => n))
        {
            writer.IncludeNamespace(@namespace);
        }
        writer.WriteEmptyLine();

        writer.OpenNamespace(TargetNamespace)
              .OpenPublicStaticPartialClass(TargetClass);

        foreach (var overload in Overloads)
        {
            var genericParameters = Subject.IsGeneric ? "<T>" : string.Empty;

            writer.WriteXmlCommentSummary($"{SummaryComment}, or otherwise throws {overload.ExceptionTextForSummary}.")
                  .WriteXmlCommentParam("parameter", ParameterComment);
            overload.WriteXmlCommentForParameters(writer)
                    .WriteXmlCommentForException(writer)
                    .WriteAggressiveInliningAttribute()
                    .OpenMember($"public static {Subject.Type} {AssertionName}{genericParameters}({Subject.Type} {Subject.Name}, {overload.Parameters})");
            
            WriteBody(this, writer, overload);
            writer.CloseScope();
        }
        writer.CloseRemainingScopes();

        var targetFile = Path.Combine("..", "Light.GuardClauses", $"{TargetClass}.{AssertionName}.cs");
        var content = stringBuilder.ToString();
        File.WriteAllText(targetFile, content);
        Console.WriteLine($"Successfully wrote \"{AssertionName}\" to \"{targetFile}\".");
    }
}

public class Parameter
{
    public Parameter(string type, bool isGeneric = false, string name = "parameter")
    {
        Name = name.MustNotBeNullOrWhiteSpace(nameof(name));
        Type = type.MustNotBeNullOrWhiteSpace(nameof(type));
        IsGeneric = isGeneric;
    }

    public string Name { get; }
    public string Type { get; }
    public bool IsGeneric { get; }
}