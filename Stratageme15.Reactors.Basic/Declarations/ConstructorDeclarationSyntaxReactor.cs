using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.TranslationContexts;
using Stratageme15.Reactors.Basic.Utility;

namespace Stratageme15.Reactors.Basic.Declarations
{
    public class ConstructorDeclarationSyntaxReactor : BasicReactorBase<ConstructorDeclarationSyntax>
    {
        protected override void HandleNode(ConstructorDeclarationSyntax node, TranslationContextWrapper context,
                                           TranslationResult result)
        {
            if (node.Modifiers.Any(SyntaxKind.StaticKeyword))
                throw new Exception("Static constructors are not supported");

            result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;
            context.CurrentClassContext.PushFunction(node,context.CurrentClassContext.Constructor);
        }

        public override void OnAfterChildTraversal(TranslationContextWrapper context, ConstructorDeclarationSyntax originalNode)
        {
            base.OnAfterChildTraversal(context, originalNode);
            context.CurrentClassContext.PopFunction();
            context.Context.PopTranslated();
        }
    }
}