using Microsoft.CodeAnalysis;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.TranslationContexts;

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
            TOutput translatedNode = TranslateNodeInner(node, context, result);
            if (result.IsStackManuallyFormed && translatedNode == null)
            {
                result.Strategy = TranslationStrategy.DontTraverseChildren;
                return;
            }
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

        public override void OnAfterChildTraversal(TranslationContext context, TExpressionSyntax originalNode)
        {
            base.OnAfterChildTraversal(context, originalNode);
            SyntaxTreeNodeBase translated = context.TranslatedNode;
            context.PopTranslated();
            Collect(translated, context);
        }

        public abstract TOutput TranslateNodeInner(TExpressionSyntax node, TranslationContext context,
                                                   TranslationResult result);
    }
}