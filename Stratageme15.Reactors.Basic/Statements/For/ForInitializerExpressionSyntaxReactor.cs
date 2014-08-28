using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom.Expressions;
using Stratageme15.Core.JavascriptCodeDom.Statements;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Statements.For
{
    internal class ForInitializerExpressionSyntaxReactor : ReactorBase<InitializerExpressionSyntax>, ISituationReactor
    {
        #region ISituationReactor Members

        public bool IsAcceptable(TranslationContext context)
        {
            return context.TranslatedNode is ForStatement;
        }

        #endregion

        public override void OnAfterChildTraversal(TranslationContext context, InitializerExpressionSyntax originalNode)
        {
            base.OnAfterChildTraversal(context, originalNode);
            var translatedSequence = context.TranslatedNode as SequenceExpression;
            context.PopTranslated();
            if (translatedSequence.Sequence.Count == 1)
            {
                context.TranslatedNode.CollectSymbol(translatedSequence.Sequence[0]);
            }
            else
            {
                context.TranslatedNode.CollectSymbol(translatedSequence);
            }
        }

        protected override void HandleNode(InitializerExpressionSyntax node, TranslationContext context,
                                           TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;
            context.PushTranslated(new SequenceExpression());
        }
    }
}