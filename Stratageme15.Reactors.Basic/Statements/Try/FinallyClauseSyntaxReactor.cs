using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom.Statements;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.TranslationContexts;
using Stratageme15.Reactors.Basic.Utility;

namespace Stratageme15.Reactors.Basic.Statements.Try
{
    public class FinallyClauseSyntaxReactor : BasicReactorBase<FinallyClauseSyntax>
    {
        protected override void HandleNode(FinallyClauseSyntax node, TranslationContextWrapper context,
                                           TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;
            var fb = new FinallyClause();
            context.Context.TargetNode.CollectSymbol(fb);
            context.Context.PushTranslated(fb);
        }

        public override void OnAfterChildTraversal(TranslationContextWrapper context, FinallyClauseSyntax originalNode)
        {
            base.OnAfterChildTraversal(context, originalNode);
            context.Context.PopTranslated();
        }
    }
}