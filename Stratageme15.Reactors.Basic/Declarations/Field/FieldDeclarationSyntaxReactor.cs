using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.TranslationContexts;
using Stratageme15.Reactors.Basic.Utility;

namespace Stratageme15.Reactors.Basic.Declarations.Field
{
    public class FieldDeclarationSyntaxReactor : BasicReactorBase<FieldDeclarationSyntax>
    {
        protected override void HandleNode(FieldDeclarationSyntax node, TranslationContextWrapper context,
                                           TranslationResult result)
        {
            if (node.Modifiers.Any(SyntaxKind.StaticKeyword)) throw new Exception("Static fields are not supported");
            result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;
            context.Context.PushContextNode(context.CurrentClassContext.FieldsDefinitionBlock);
            context.CurrentClassContext.OutOfContext();
        }

        public override void OnAfterChildTraversal(TranslationContextWrapper context, FieldDeclarationSyntax originalNode)
        {
            base.OnAfterChildTraversal(context, originalNode);
            context.Context.PopContextNode();
            context.CurrentClassContext.ReturnToContext();
        }
    }
}