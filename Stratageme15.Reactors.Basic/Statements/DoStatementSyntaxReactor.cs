using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom.Statements;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.TranslationContexts;
using Stratageme15.Reactors.Basic.Utility;

namespace Stratageme15.Reactors.Basic.Statements
{
    public class DoStatementSyntaxReactor : BasicReactorBase<DoStatementSyntax>
    {
        protected override void HandleNode(DoStatementSyntax node, TranslationContextWrapper context, TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;

            node = node.WithCondition(SyntaxFactory.ParenthesizedExpression(node.Condition));
            result.PrepareForManualPush(context.Context);
            var whl = new DoWhileStatement();
            context.Context.TargetNode.CollectSymbol(whl);
            context.Context.PushTranslated(whl);
            context.Context.TranslationStack.Push(node.Condition);
            context.Context.TranslationStack.Push(node.Statement);
        }

        public override void OnAfterChildTraversal(TranslationContextWrapper context, DoStatementSyntax originalNode)
        {
            base.OnAfterChildTraversal(context, originalNode);
            context.Context.PopTranslated();
        }
    }
}