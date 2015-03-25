using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom.Statements;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.TranslationContexts;
using Stratageme15.Reactors.Basic.Utility;

namespace Stratageme15.Reactors.Basic.Statements.For
{
    public class ForStatementSyntaxReactor : BasicReactorBase<ForStatementSyntax>
    {
        protected override void HandleNode(ForStatementSyntax node, TranslationContextWrapper context, TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;
            result.PrepareForManualPush(context.Context);
            var fs = new ForStatement();
            context.Context.TargetNode.CollectSymbol(fs);
            context.Context.PushTranslated(fs);
            context.CurrentClassContext.CurrentFunction.LocalVariables.PushContext();
            context.Context.TranslationStack.Push(node.Statement);

            InitializerExpressionSyntax incrementorsExpressionSyntax =
                SyntaxFactory.InitializerExpression(SyntaxKind.ObjectInitializerExpression, node.Incrementors);
            context.Context.TranslationStack.Push(incrementorsExpressionSyntax);

            context.Context.TranslationStack.Push(node.Condition);
            if (node.Initializers.Count > 0)
            {
                InitializerExpressionSyntax initializersExpressionSyntax =
                    SyntaxFactory.InitializerExpression(SyntaxKind.ObjectInitializerExpression, node.Initializers);
                context.Context.TranslationStack.Push(initializersExpressionSyntax);
            }
            else
            {
                if (node.Declaration != null) context.Context.TranslationStack.Push(node.Declaration);
            }
        }

        public override void OnAfterChildTraversal(TranslationContextWrapper context, ForStatementSyntax originalNode)
        {
            base.OnAfterChildTraversal(context, originalNode);
            context.Context.PopTranslated();
            context.CurrentClassContext.CurrentFunction.LocalVariables.PopContext();
        }
    }
}