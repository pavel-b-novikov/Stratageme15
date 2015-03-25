using Microsoft.CodeAnalysis;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.TranslationContexts;
using Stratageme15.Reactors.Basic.Utility;

namespace Stratageme15.Reactors.Basic.Expressions
{
    public abstract class ExpressionReactorBase<TExpressionSyntax, TOutput>
        : BasicReactorBase<TExpressionSyntax>
        where TExpressionSyntax : SyntaxNode
        where TOutput : SyntaxTreeNodeBase
    {
        protected override void HandleNode(TExpressionSyntax node, TranslationContextWrapper context, TranslationResult result)
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
                context.Context.PushTranslated(translatedNode);
            }
        }

        private void Collect(SyntaxTreeNodeBase translatedNode, TranslationContextWrapper context)
        {
            context.Context.TargetNode.CollectSymbol(translatedNode);
        }

        public override void OnAfterChildTraversal(TranslationContextWrapper context, TExpressionSyntax originalNode)
        {
            base.OnAfterChildTraversal(context, originalNode);
            SyntaxTreeNodeBase translated = context.Context.TargetNode;
            context.Context.PopTranslated();
            Collect(translated, context);
        }

        public abstract TOutput TranslateNodeInner(TExpressionSyntax node, TranslationContextWrapper context,
                                                   TranslationResult result);
    }
}