using System;
using Roslyn.Compilers.CSharp;
using Stratageme15.Core.JavascriptCodeDom.Statements;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.Reactors;
using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Statements
{
    public class WhileStatementSyntaxReactor : ReactorBase<WhileStatementSyntax>
    {
        protected override void HandleNode(WhileStatementSyntax node, TranslationContext context, TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;

            node = node.WithCondition(Syntax.ParenthesizedExpression(node.Condition));
            result.PrepareForManualPush(context);
            WhileStatement whl = new WhileStatement();
            context.TranslatedNode.CollectSymbol(whl);
            context.PushTranslated(whl);
            context.TranslationStack.Push(node.Statement);
            context.TranslationStack.Push(node.Condition);
        }

        public override void OnAfterChildTraversal(TranslationContext context, WhileStatementSyntax originalNode)
        {
            base.OnAfterChildTraversal(context, originalNode);
            context.PopTranslated();
        }
    }
}
