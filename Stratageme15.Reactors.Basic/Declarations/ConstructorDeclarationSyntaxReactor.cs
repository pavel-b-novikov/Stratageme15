using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.TranslationContexts;
using Stratageme15.Reactors.Basic.Extensions;

namespace Stratageme15.Reactors.Basic.Declarations
{
    public class ConstructorDeclarationSyntaxReactor : ReactorBase<ConstructorDeclarationSyntax>
    {
        protected override void HandleNode(ConstructorDeclarationSyntax node, TranslationContext context,
                                           TranslationResult result)
        {
            if (node.Modifiers.Any(SyntaxKind.StaticKeyword))
                throw new Exception("Static constructors are not supported");

            result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;
            string typeName = context.JavascriptCurrentTypeName();

            // constructors always are translated firstly
            context.CurrentClassContext.CreateConstructor(typeName);
            context.TranslatedNode.CollectSymbol(context.CurrentClassContext.Constructor);
            context.CurrentClassContext.PushFunction(node, context.CurrentClassContext.Constructor);
            context.PushTranslated(context.CurrentClassContext.CurrentFunction.Function);
        }

        public override void OnAfterChildTraversal(TranslationContext context, ConstructorDeclarationSyntax originalNode)
        {
            base.OnAfterChildTraversal(context, originalNode);
            context.CurrentClassContext.PopFunction();
            context.PopTranslated();
        }
    }
}