using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom.Statements;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.Reactors;
using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Statements.Try
{
    public class FinallyClauseSyntaxReactor : ReactorBase<FinallyClauseSyntax>
    {
        protected override void HandleNode(FinallyClauseSyntax node, TranslationContext context,
                                           TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;
            var fb = new FinallyClause();
            context.TranslatedNode.CollectSymbol(fb);
            context.PushTranslated(fb);
        }

        public override void OnAfterChildTraversal(TranslationContext context, FinallyClauseSyntax originalNode)
        {
            base.OnAfterChildTraversal(context, originalNode);
            context.PopTranslated();
        }
    }
}