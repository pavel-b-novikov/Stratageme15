using Roslyn.Compilers.CSharp;
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
            Core.JavascriptCodeDom.Statements.ForStatement fs = new Core.JavascriptCodeDom.Statements.ForStatement();
            context.TranslatedNode.CollectSymbol(fs);
            context.PushTranslated(fs);
            context.CurrentClassContext.CurrentFunction.LocalVariables.PushContext();
            context.TranslationStack.Push(node.Statement);

            InitializerExpressionSyntax incrementorsExpressionSyntax = Syntax.InitializerExpression(SyntaxKind.ObjectInitializerExpression, node.Incrementors);
            context.TranslationStack.Push(incrementorsExpressionSyntax);

            context.TranslationStack.Push(node.Condition);
            if (node.Initializers.Count > 0)
            {
                InitializerExpressionSyntax initializersExpressionSyntax =
                    Syntax.InitializerExpression(SyntaxKind.ObjectInitializerExpression, node.Initializers);
                context.TranslationStack.Push(initializersExpressionSyntax);
            }else
            {
                if (node.Declaration!=null) context.TranslationStack.Push(node.Declaration);
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
