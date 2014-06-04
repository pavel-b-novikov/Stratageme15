using System;
using Roslyn.Compilers.CSharp;
using Stratageme15.Core.JavascriptCodeDom.Statements;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.Reactors;
using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Statements
{
    public class DoStatementSyntaxReactor : ReactorBase<DoStatementSyntax>
    {
        protected override void HandleNode(DoStatementSyntax node, TranslationContext context, TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;

            node = node.WithCondition(Syntax.ParenthesizedExpression(node.Condition));
            result.PrepareForManualPush(context);
            DoWhileStatement whl = new DoWhileStatement();
            context.TranslatedNode.CollectSymbol(whl);
            context.PushTranslated(whl);
            context.TranslationStack.Push(node.Condition);
            context.TranslationStack.Push(node.Statement);
        }

        public override void OnAfterChildTraversal(TranslationContext context, DoStatementSyntax originalNode)
        {
            base.OnAfterChildTraversal(context, originalNode);
            context.PopTranslated();
        }
    }
}
