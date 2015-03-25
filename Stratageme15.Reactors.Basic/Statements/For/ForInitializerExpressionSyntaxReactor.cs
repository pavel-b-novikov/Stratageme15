using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom.Expressions;
using Stratageme15.Core.JavascriptCodeDom.Statements;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.TranslationContexts;
using Stratageme15.Reactors.Basic.Utility;

namespace Stratageme15.Reactors.Basic.Statements.For
{
    internal class ForInitializerExpressionSyntaxReactor : BasicReactorBase<InitializerExpressionSyntax>, ISituationReactor
    {
        #region ISituationReactor Members

        public bool IsAcceptable(TranslationContext context)
        {
            return context.TargetNode is ForStatement;
        }

        #endregion

        public override void OnAfterChildTraversal(TranslationContextWrapper context, InitializerExpressionSyntax originalNode)
        {
            base.OnAfterChildTraversal(context, originalNode);
            var translatedSequence = context.Context.TargetNode as SequenceExpression;
            context.Context.PopTranslated();
            if (translatedSequence.Sequence.Count == 1)
            {
                context.Context.TargetNode.CollectSymbol(translatedSequence.Sequence[0]);
            }
            else
            {
                context.Context.TargetNode.CollectSymbol(translatedSequence);
            }
        }

        protected override void HandleNode(InitializerExpressionSyntax node, TranslationContextWrapper context,
                                           TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;
            context.Context.PushTranslated(new SequenceExpression());
        }
    }
}