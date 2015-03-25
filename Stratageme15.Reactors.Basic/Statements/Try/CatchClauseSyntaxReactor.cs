using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom.Statements;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.TranslationContexts;
using Stratageme15.Reactors.Basic.Utility;

namespace Stratageme15.Reactors.Basic.Statements.Try
{
    public class CatchClauseSyntaxReactor : BasicReactorBase<CatchClauseSyntax>
    {
        protected override void HandleNode(CatchClauseSyntax node, TranslationContextWrapper context, TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;
            result.PrepareForManualPush(context.Context);

            var ctch = new CatchClause();
            context.Context.TargetNode.CollectSymbol(ctch);
            if (node.Declaration != null)
            {
                if (node.Declaration.Identifier.CSharpKind() != SyntaxKind.None)
                {
                    ctch.Identifier = node.Declaration.Identifier.ValueText.ToIdent();
                    context.CurrentClassContext.CurrentFunction.LocalVariables.PushContext();
                    var exctype = context.Context.SemanticModel.GetTypeInfo(node.Declaration.Type); // todo
                    context.CurrentClassContext.CurrentFunction.LocalVariables.DefineVariable(
                        node.Declaration.Identifier.ValueText, exctype);
                }
            }
            context.Context.PushTranslated(ctch);
            context.Context.TranslationStack.Push(node.Block);
        }

        public override void OnAfterChildTraversal(TranslationContextWrapper context, CatchClauseSyntax originalNode)
        {
            base.OnAfterChildTraversal(context, originalNode);
            context.Context.PopTranslated();
            if (originalNode.Declaration != null)
            {
                context.CurrentClassContext.CurrentFunction.LocalVariables.PopContext();
            }
        }
    }
}