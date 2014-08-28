using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom.Statements;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.Builders;
using Stratageme15.Core.Transaltion.Reactors;
using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Statements.Try
{
    public class CatchClauseSyntaxReactor : ReactorBase<CatchClauseSyntax>
    {
        protected override void HandleNode(CatchClauseSyntax node, TranslationContext context, TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;
            result.PrepareForManualPush(context);

            var ctch = new CatchClause();
            context.TranslatedNode.CollectSymbol(ctch);
            if (node.Declaration != null)
            {
                if (node.Declaration.Identifier.CSharpKind() != SyntaxKind.None)
                {
                    ctch.Identifier = node.Declaration.Identifier.ValueText.Ident();
                    context.CurrentClassContext.CurrentFunction.LocalVariables.PushContext();
                    Type exctype = TypeInferer.GetTypeFromContext(node.Declaration.Type, context);
                    context.CurrentClassContext.CurrentFunction.LocalVariables.DefineVariable(
                        node.Declaration.Identifier.ValueText, exctype);
                }
            }
            context.PushTranslated(ctch);
            context.TranslationStack.Push(node.Block);
        }

        public override void OnAfterChildTraversal(TranslationContext context, CatchClauseSyntax originalNode)
        {
            base.OnAfterChildTraversal(context, originalNode);
            context.PopTranslated();
            if (originalNode.Declaration != null)
            {
                context.CurrentClassContext.CurrentFunction.LocalVariables.PopContext();
            }
        }
    }
}