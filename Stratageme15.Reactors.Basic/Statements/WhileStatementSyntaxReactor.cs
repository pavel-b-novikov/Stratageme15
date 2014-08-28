using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom.Statements;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Statements
{
    public class WhileStatementSyntaxReactor : ReactorBase<WhileStatementSyntax>
    {
        protected override void HandleNode(WhileStatementSyntax node, TranslationContext context,
                                           TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;

            node = node.WithCondition(SyntaxFactory.ParenthesizedExpression(node.Condition));
            result.PrepareForManualPush(context);
            var whl = new WhileStatement();
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