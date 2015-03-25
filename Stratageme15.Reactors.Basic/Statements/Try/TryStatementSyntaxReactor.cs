using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom.Statements;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.TranslationContexts;
using Stratageme15.Reactors.Basic.Utility;

namespace Stratageme15.Reactors.Basic.Statements.Try
{
    public class TryStatementSyntaxReactor : BasicReactorBase<TryStatementSyntax>
    {
        protected override void HandleNode(TryStatementSyntax node, TranslationContextWrapper context, TranslationResult result)
        {
            if (node.Catches != null)
            {
                if (node.Catches.Count > 1) throw new Exception("Javascript dos not supports multiple catches");
            }

            var tfb = new TryCatchFinallyStatement();

            context.Context.TargetNode.CollectSymbol(tfb);
            result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;
            result.PrepareForManualPush(context.Context);

            if (node.Finally != null)
            {
                context.Context.TranslationStack.Push(node.Finally);
            }
            if (node.Catches != null && node.Catches.Count == 1)
            {
                context.Context.TranslationStack.Push(node.Catches[0]);
            }
            context.Context.TranslationStack.Push(node.Block);
            context.Context.PushTranslated(tfb);
        }

        public override void OnAfterChildTraversal(TranslationContextWrapper context, TryStatementSyntax originalNode)
        {
            base.OnAfterChildTraversal(context, originalNode);
            context.Context.PopTranslated();
        }
    }
}