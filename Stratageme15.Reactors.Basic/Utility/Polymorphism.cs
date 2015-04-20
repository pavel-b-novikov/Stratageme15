using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom.Markers;
using Stratageme15.Core.Translation.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Utility
{
    /// <summary>
    /// Be careful of Polymorphism instance
    /// It should be exactly one per class translation context
    /// </summary>
    public class Polymorphism
    {
        private readonly Dictionary<string, PolymorphMethod> _polymorps = new Dictionary<string, PolymorphMethod>();
        private readonly Dictionary<string, PolymorphMethod> _staticPolymorps = new Dictionary<string, PolymorphMethod>();

        public void RegisterPolymorphism(string methodName, bool isStatic, TranslationContext context)
        {
            var polymorph = isStatic ? _staticPolymorps : _polymorps;

            if (polymorph.ContainsKey(methodName)) return;
            polymorph.Add(methodName, new PolymorphMethod(methodName, context.SemanticModel));
        }

        public bool LookupPolymorohism(MethodDeclarationSyntax mdc)
        {
            return LookupPolymorohism(mdc.Identifier.ValueText, mdc.Modifiers.Any(SyntaxKind.StaticKeyword));
        }

        public bool LookupPolymorohism(string methodName, bool isStatic)
        {
            var polymorph = isStatic ? _staticPolymorps : _polymorps;

            if (!isStatic) return polymorph.ContainsKey(methodName);
            return polymorph.ContainsKey(methodName);
        }

        public void PushPolymorphism(MethodDeclarationSyntax mdc, TranslationContext context)
        {
            PushPolymorphism(mdc.Identifier.ValueText, mdc.ParameterList, mdc.Modifiers.Any(SyntaxKind.StaticKeyword), context);
        }

        public void PushPolymorphism(string methodName, ParameterListSyntax parameters, bool isStatic, TranslationContext context)
        {
            var polymorph = isStatic ? _staticPolymorps : _polymorps;

            var poly = polymorph[methodName];
            var codeBlock = poly.AddOverload(parameters);
            context.PushContextNode(codeBlock);
        }

        public void PopPolymorphism(TranslationContext context)
        {
            context.PopContextNode();
        }

        public IStatement[] EmitPolymorphicMethods(string className)
        {
            List<IStatement> polymorph = new List<IStatement>(_polymorps.Count);
            polymorph.AddRange(_polymorps
                .Select(polymorphMethod => polymorphMethod.Value)
                .Select(pm => JavascriptHelper.CreateEmptyFunction()
                        .AppendStatement(pm.PolymorphIfStatement)
                        .AsMethod(className, pm.MethodName))
                );
            polymorph.AddRange(_staticPolymorps
                .Select(polymorphMethod => polymorphMethod.Value)
                .Select(pm => JavascriptHelper.CreateEmptyFunction()
                        .AppendStatement(pm.PolymorphIfStatement)
                        .AsMethod(className, pm.MethodName, isStatic: true)));
            return polymorph.ToArray();
        }
    }
}
