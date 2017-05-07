using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class CheckConditionalAttributeAppliance
    {
        [Fact(DisplayName = "All static and extension methods returning void in namespace Light.GuardClauses must have the ConditionalAttribute applied to them.")]
        public void CheckMethodsForConditionalAttribute()
        {
            var methodsWithoutConditionalAttribute = typeof (Check).GetTypeInfo()
                                                                   .Assembly
                                                                   .ExportedTypes
                                                                   .Where(t => t.Namespace == typeof(Check).Namespace)
                                                                   .SelectMany(t => t.GetMethods())
                                                                   .Where(m => m.IsStatic && m.ReturnType == typeof (void))
                                                                   .Where(m =>
                                                                          {
                                                                              var conditionalAttribute = m.GetCustomAttribute<ConditionalAttribute>();
                                                                              if (conditionalAttribute == null)
                                                                                  return true;

                                                                              return conditionalAttribute.ConditionString != Check.CompileAssertionsSymbol;
                                                                          })
                                                                   .ToList();

            if (methodsWithoutConditionalAttribute.Count == 0)
                return;

            var stringBuilder = new StringBuilder("The following methods are not marked with [Conditional(Check.CompileAssertionsSymbol)]:").AppendLine();
            // ReSharper disable once PossibleNullReferenceException
            stringBuilder.AppendItemsWithNewLine(methodsWithoutConditionalAttribute.Select(m => $"{m.DeclaringType.Name}.{m.Name}").ToList());
            throw new Exception(stringBuilder.ToString());
        }
    }
}