#! "netcoreapp2.0"
#load "CSharpCodeWriter.csx"
#load "CollectionTypes.csx"
#load "Namespaces.csx"
#load "AssertionOverload.csx"

var targetFile = Path.Combine("..", "Light.GuardClauses", "UriAssertions.MustHaveOneSchemeOf.cs");
var stringBuilder = new StringBuilder();
var writer = new CSharpCodeWriter(new StringWriter(stringBuilder));
const string itemType = "string";
var supportedCollectionTypes = new CollectionTypeInfo[]
{
      new ArrayInfo(itemType),
      new ListInfo(itemType),
      new AbstractReadOnlyListInfo(itemType),
      new AbstractListInfo(itemType),
      new ObservableCollectionInfo(itemType),
      new CollectionInfo(itemType),
      new ReadOnlyCollectionInfo(itemType),
      new EnumerableInfo(itemType)
};

var reason = $"when {XmlComment.ToParamRef("parameter")} uses none of the specified schemes";
const string additionalParameter = "string parameterName = null";
Action<CSharpCodeWriter> additionalParamterComment = w => w.WriteXmlCommentParam("parameterName", $"The name of the parameter used for the {XmlComment.ToSee("ArgumentNullException")} and {XmlComment.ToSee("RelativeUriException")} (optional).");
var overloads = new AssertionOverload[]
{
      new DefaultOverload("Throw.UriMustHaveOneSchemeOf(parameter, schemes, parameterName, message);", "InvalidUriSchemeException", reason, "an"),
      new CustomExceptionOverload(reason, additionalParameter, additionalParamterComment),
      new CustomExceptionOverloadWithOneParameter("Uri", reason, additionalParameter, additionalParamterComment)
};

writer.WriteCodeGenerationNotice("UriAssertions.MustHaveOneSchemeOf.csx")
      .IncludeNamespace(Namespaces.System)
      .IncludeNamespace(Namespaces.SystemCollectionsGeneric)
      .IncludeNamespace(Namespaces.SystemRuntimeCompilerServices)
      .IncludeNamespace(Namespaces.SystemCollectionsObjectModel)
      .IncludeNamespace(Namespaces.LightGuardClausesExceptions)
      .WriteEmptyLine()
      .WriteReSharperDisablePossibleMultipleEnumeration()
      .WriteEmptyLine()
      .OpenNamespace(Namespaces.LightGuardClauses)
      .OpenPublicStaticPartialClass("UriAssertions");

foreach (var info in supportedCollectionTypes)
{
      foreach (var overload in overloads)
      {
          writer.WriteXmlCommentSummary($"Ensures that the parameter has one of the specified schemes, or otherwise throws {overload.ExceptionTextForSummary}.")
                .WriteXmlCommentParam("parameter", "The URI to be checked.")
                .WriteXmlCommentParam("schemes", "One of these schemes should apply to the URI.");

          overload.WriteXmlCommentForParameters(writer)
                  .WriteXmlCommentForException(writer)
                  .WriteXmlCommentException("RelativeUriException", $"Thrown when {XmlComment.ToParamRef("parameter")} is relative and thus has no scheme.")
                  .WriteDefaultArgumentNullException()
                  .WriteAggressiveInliningAttribute()
                  .OpenMember($"public static Uri MustHaveOneSchemeOf(this Uri parameter, {info.CollectionType} schemes, {overload.Parameters})")
                  .WriteLine("parameter.MustBeAbsoluteUri(parameterName);");

          info.OpenLoop(writer, "scheme", "schemes")
              .WriteLine("if (parameter.Scheme.Equals(scheme, StringComparison.OrdinalIgnoreCase))")
              .IncreaseIndentation()
              .WriteLine("return parameter;")
              .DecreaseIndentation()
              .CloseScope()
              .WriteEmptyLine()
              .WriteLine(overload.ThrowStatement)
              .WriteLine("return null;")
              .CloseScope()
              .WriteEmptyLine();
      }
}
writer.CloseRemainingScopes();

var fileContent = stringBuilder.ToString();
File.WriteAllText(targetFile, fileContent);
Console.WriteLine($"The following was written to \"{targetFile}\":");
Console.WriteLine(fileContent);