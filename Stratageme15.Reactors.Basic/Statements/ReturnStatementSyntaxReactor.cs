using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom.Statements;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.Reactors;
using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Statements
{
    public class ReturnStatementSyntaxReactor : ReactorBase<ReturnStatementSyntax>
    {
        protected override void HandleNode(ReturnStatementSyntax node, TranslationContext context,
                                           TranslationResult result)
        {
            if (node.Expression == null)
            {
                result.Strategy = TranslationStrategy.DontTraverseChildren;
                context.TranslatedNode.CollectSymbol(new ReturnStatement());
                return;
            }

            result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;
            var rs = new ReturnStatement();
            context.TranslatedNode.CollectSymbol(rs);
            context.PushTranslated(rs);
        }

        public override void OnAfterChildTraversal(TranslationContext context, ReturnStatementSyntax originalNode)
        {
            base.OnAfterChildTraversal(context, originalNode);
            context.PopTranslated();
        }
    }
}