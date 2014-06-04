using Roslyn.Compilers.CSharp;
using Stratageme15.Core.JavascriptCodeDom.Expressions;
using Stratageme15.Core.JavascriptCodeDom.Statements;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.Reactors;
using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Statements.For
{
    class ForInitializerExpressionSyntaxReactor : ReactorBase<InitializerExpressionSyntax>,ISituationReactor
    {
        public override void OnAfterChildTraversal(TranslationContext context, InitializerExpressionSyntax originalNode)
        {
            base.OnAfterChildTraversal(context, originalNode);
            var translatedSequence = context.TranslatedNode as SequenceExpression;
            context.PopTranslated();
            if (translatedSequence.Sequence.Count==1)
            {
                context.TranslatedNode.CollectSymbol(translatedSequence.Sequence[0]);    
            }else
            {
                context.TranslatedNode.CollectSymbol(translatedSequence);    
            }
            
        }

        protected override void HandleNode(InitializerExpressionSyntax node, TranslationContext context, TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;
            context.PushTranslated(new SequenceExpression());
        }

        public bool IsAcceptable(TranslationContext context)
        {
            return context.TranslatedNode is ForStatement;
        }
    }
}
