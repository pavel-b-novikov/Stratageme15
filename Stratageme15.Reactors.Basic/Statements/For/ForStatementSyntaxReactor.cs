using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom.Statements;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.Reactors;
using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Statements.For
{
    public class ForStatementSyntaxReactor : ReactorBase<ForStatementSyntax>
    {
        protected override void HandleNode(ForStatementSyntax node, TranslationContext context, TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;
            result.PrepareForManualPush(context);
            var fs = new ForStatement();
            context.TranslatedNode.CollectSymbol(fs);
            context.PushTranslated(fs);
            context.CurrentClassContext.CurrentFunction.LocalVariables.PushContext();
            context.TranslationStack.Push(node.Statement);

            InitializerExpressionSyntax incrementorsExpressionSyntax =
                SyntaxFactory.InitializerExpression(SyntaxKind.ObjectInitializerExpression, node.Incrementors);
            context.TranslationStack.Push(incrementorsExpressionSyntax);

            context.TranslationStack.Push(node.Condition);
            if (node.Initializers.Count > 0)
            {
                InitializerExpressionSyntax initializersExpressionSyntax =
                    SyntaxFactory.InitializerExpression(SyntaxKind.ObjectInitializerExpression, node.Initializers);
                context.TranslationStack.Push(initializersExpressionSyntax);
            }
            else
            {
                if (node.Declaration != null) context.TranslationStack.Push(node.Declaration);
            }
        }

        public override void OnAfterChildTraversal(TranslationContext context, ForStatementSyntax originalNode)
        {
            base.OnAfterChildTraversal(context, originalNode);
            context.PopTranslated();
            context.CurrentClassContext.CurrentFunction.LocalVariables.PopContext();
        }
    }
}