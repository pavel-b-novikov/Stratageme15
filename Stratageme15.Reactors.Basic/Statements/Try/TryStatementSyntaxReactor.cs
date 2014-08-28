using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom.Statements;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Statements.Try
{
    public class TryStatementSyntaxReactor : ReactorBase<TryStatementSyntax>
    {
        protected override void HandleNode(TryStatementSyntax node, TranslationContext context, TranslationResult result)
        {
            if (node.Catches != null)
            {
                if (node.Catches.Count > 1) throw new Exception("Javascript dos not supports multiple catches");
            }

            var tfb = new TryCatchFinallyStatement();

            context.TranslatedNode.CollectSymbol(tfb);
            result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;
            result.PrepareForManualPush(context);

            if (node.Finally != null)
            {
                context.TranslationStack.Push(node.Finally);
            }
            if (node.Catches != null && node.Catches.Count == 1)
            {
                context.TranslationStack.Push(node.Catches[0]);
            }
            context.TranslationStack.Push(node.Block);
            context.PushTranslated(tfb);
        }

        public override void OnAfterChildTraversal(TranslationContext context, TryStatementSyntax originalNode)
        {
            base.OnAfterChildTraversal(context, originalNode);
            context.PopTranslated();
        }
    }
}