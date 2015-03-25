using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom.Statements;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.TranslationContexts;
using Stratageme15.Reactors.Basic.Utility;

namespace Stratageme15.Reactors.Basic.Statements
{
    public class ReturnStatementSyntaxReactor : BasicReactorBase<ReturnStatementSyntax>
    {
        protected override void HandleNode(ReturnStatementSyntax node, TranslationContextWrapper context,
                                           TranslationResult result)
        {
            if (node.Expression == null)
            {
                result.Strategy = TranslationStrategy.DontTraverseChildren;
                context.Context.TargetNode.CollectSymbol(new ReturnStatement());
                return;
            }

            result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;
            var rs = new ReturnStatement();
            context.Context.TargetNode.CollectSymbol(rs);
            context.Context.PushTranslated(rs);
        }

        public override void OnAfterChildTraversal(TranslationContextWrapper context, ReturnStatementSyntax originalNode)
        {
            base.OnAfterChildTraversal(context, originalNode);
            context.Context.PopTranslated();
        }
    }
}