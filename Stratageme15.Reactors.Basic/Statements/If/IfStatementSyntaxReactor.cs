using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom.Statements;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.TranslationContexts;
using Stratageme15.Reactors.Basic.Utility;

namespace Stratageme15.Reactors.Basic.Statements.If
{
    public class IfStatementSyntaxReactor : BasicReactorBase<IfStatementSyntax>
    {
        protected override void HandleNode(IfStatementSyntax node, TranslationContextWrapper context, TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;
            node = node.WithCondition(SyntaxFactory.ParenthesizedExpression(node.Condition));
            result.PrepareForManualPush(context.Context);
            var fs = new IfStatement();
            context.Context.TargetNode.CollectSymbol(fs);
            context.Context.PushTranslated(fs);

            if (node.Else != null)
            {
                context.Context.TranslationStack.Push(node.Else);
            }
            context.Context.TranslationStack.Push(node.Statement);
            context.Context.TranslationStack.Push(node.Condition);
        }

        public override void OnAfterChildTraversal(TranslationContextWrapper context, IfStatementSyntax originalNode)
        {
            base.OnAfterChildTraversal(context, originalNode);
            context.Context.PopTranslated();
        }
    }
}