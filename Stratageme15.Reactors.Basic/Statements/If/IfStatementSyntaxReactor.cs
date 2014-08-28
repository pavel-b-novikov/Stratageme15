using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom.Statements;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Statements.If
{
    public class IfStatementSyntaxReactor : ReactorBase<IfStatementSyntax>
    {
        protected override void HandleNode(IfStatementSyntax node, TranslationContext context, TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;
            node = node.WithCondition(SyntaxFactory.ParenthesizedExpression(node.Condition));
            result.PrepareForManualPush(context);
            var fs = new IfStatement();
            context.TranslatedNode.CollectSymbol(fs);
            context.PushTranslated(fs);

            if (node.Else != null)
            {
                context.TranslationStack.Push(node.Else);
            }
            context.TranslationStack.Push(node.Statement);
            context.TranslationStack.Push(node.Condition);
        }

        public override void OnAfterChildTraversal(TranslationContext context, IfStatementSyntax originalNode)
        {
            base.OnAfterChildTraversal(context, originalNode);
            context.PopTranslated();
        }
    }
}