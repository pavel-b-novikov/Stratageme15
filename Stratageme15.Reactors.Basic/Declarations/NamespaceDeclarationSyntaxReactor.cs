using System;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.TranslationContexts;
using Stratageme15.Reactors.Basic.Utility;

namespace Stratageme15.Reactors.Basic.Declarations
{
    internal class NamespaceDeclarationSyntaxReactor : BasicReactorBase<NamespaceDeclarationSyntax>
    {
        protected override void HandleNode(NamespaceDeclarationSyntax node, TranslationContextWrapper context,
                                           TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildren;
            context.Namespace = node.FullQualifiedName();
            string[] namespaceHierarchy = context.Namespace.Split('.');
            var root = namespaceHierarchy[0];

            var rootNamespaceSymbol = (INamespaceSymbol) context.Context.SemanticModel.LookupSymbols(0,name:root)[0];
            var cNamespace = rootNamespaceSymbol;
            for (int i = 1; i < namespaceHierarchy.Length; i++)
            {
                var nh = namespaceHierarchy[i];
                var subNamespaces = cNamespace.GetNamespaceMembers();
                cNamespace = subNamespaces.FirstOrDefault(c=>c.Name==nh);
                if (cNamespace==null) throw new Exception("No namespace found");
            }

            context.NamespaceSymbol = cNamespace;
        }
    }
}