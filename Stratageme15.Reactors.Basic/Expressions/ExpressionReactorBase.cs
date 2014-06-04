using Roslyn.Compilers.CSharp;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.JavascriptCodeDom.Expressions;
using Stratageme15.Core.JavascriptCodeDom.Markers;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.Reactors;
using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Expressions
{
    public abstract class ExpressionReactorBase<TExpressionSyntax, TOutput>
        : ReactorBase<TExpressionSyntax>, IStandaloneReactor<TExpressionSyntax>
        where TExpressionSyntax : SyntaxNode
        where TOutput : SyntaxTreeNodeBase
    {
        protected override void HandleNode(TExpressionSyntax node, TranslationContext context, TranslationResult result)
        {
            result.Strategy = TranslationStrategy.DontTraverseChildren;
            var translatedNode = TranslateNodeInner(node, context, result);
            if (result.Strategy == TranslationStrategy.DontTraverseChildren)
            {
                Collect(translatedNode, context);
            }
            else
            {
                result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;
                context.PushTranslated(translatedNode);
            }
        }
        private void Collect(SyntaxTreeNodeBase translatedNode, TranslationContext context)
        {
            context.TranslatedNode.CollectSymbol(translatedNode);
        }

        public override void OnAfterChildTraversal(TranslationContext context,TExpressionSyntax originalNode)
        {
            base.OnAfterChildTraversal(context,originalNode);
            var translated = context.TranslatedNode;
            context.PopTranslated();
            Collect(translated, context);
        }

        protected Expression TranslateChildExpression(ExpressionSyntax exprSyntax, TranslationContext context)
        {
            var reactor = context.Reactors.GetStandaloneReactor<ExpressionSyntax>(exprSyntax.GetType());
            return (Expression)reactor.TranslateNode(exprSyntax, context);
        }

        public abstract TOutput TranslateNodeInner(TExpressionSyntax node, TranslationContext context, TranslationResult result);

        public SyntaxTreeNodeBase TranslateNode(SyntaxNode node, TranslationContext context)
        {
            return TranslateNodeInner((TExpressionSyntax)node, context, null);
        }
    }
}
