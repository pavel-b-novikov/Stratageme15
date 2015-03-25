using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom.Statements;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.TranslationContexts;
using Stratageme15.Reactors.Basic.Utility;

namespace Stratageme15.Reactors.Basic.Statements
{
    public class ThrowStatementSyntaxReactor : BasicReactorBase<ThrowStatementSyntax>
    {
        public override void OnAfterChildTraversal(TranslationContextWrapper context, ThrowStatementSyntax originalNode)
        {
            base.OnAfterChildTraversal(context, originalNode);
            context.Context.PopTranslated();
        }

        protected override void HandleNode(ThrowStatementSyntax node, TranslationContextWrapper context,
                                           TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;
            if (node.Expression == null) throw new Exception("Exception rethrown is not supported");
            var thr = new ThrowStatement();
            context.Context.TargetNode.CollectSymbol(thr);
            context.Context.PushTranslated(thr);
        }
    }
}