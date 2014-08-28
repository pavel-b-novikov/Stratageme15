using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.Reactors;
using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Declarations.Field
{
    public class FieldDeclarationSyntaxReactor : ReactorBase<FieldDeclarationSyntax>
    {
        protected override void HandleNode(FieldDeclarationSyntax node, TranslationContext context,
                                           TranslationResult result)
        {
            if (node.Modifiers.Any(SyntaxKind.StaticKeyword)) throw new Exception("Static fields are not supported");
            result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;
            context.SetTranslated(context.CurrentClassContext.FieldsDefinitionBlock);
            context.CurrentClassContext.OutOfContext();
        }

        public override void OnAfterChildTraversal(TranslationContext context, FieldDeclarationSyntax originalNode)
        {
            base.OnAfterChildTraversal(context, originalNode);
            context.RestoreTranslated();
            context.CurrentClassContext.ReturnToContext();
        }
    }
}