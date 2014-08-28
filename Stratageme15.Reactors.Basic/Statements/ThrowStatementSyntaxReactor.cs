using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom.Statements;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.Reactors;
using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Statements
{
    public class ThrowStatementSyntaxReactor : ReactorBase<ThrowStatementSyntax>
    {
        public override void OnAfterChildTraversal(TranslationContext context, ThrowStatementSyntax originalNode)
        {
            base.OnAfterChildTraversal(context, originalNode);
            context.PopTranslated();
        }

        protected override void HandleNode(ThrowStatementSyntax node, TranslationContext context,
                                           TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;
            if (node.Expression == null) throw new Exception("Exception rethrown is not supported");
            var thr = new ThrowStatement();
            context.TranslatedNode.CollectSymbol(thr);
            context.PushTranslated(thr);
        }
    }
}