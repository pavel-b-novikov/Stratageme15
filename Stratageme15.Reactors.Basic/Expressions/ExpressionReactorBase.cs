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
        : ReactorBase<TExpressionSyntax>
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

        public abstract TOutput TranslateNodeInner(TExpressionSyntax node, TranslationContext context, TranslationResult result);
    }
}
