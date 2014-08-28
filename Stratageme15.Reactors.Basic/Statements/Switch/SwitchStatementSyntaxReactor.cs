using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom.Statements;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.Reactors;
using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Statements.Switch
{
    public class SwitchStatementSyntaxReactor : ReactorBase<SwitchStatementSyntax>
    {
        protected override void HandleNode(SwitchStatementSyntax node, TranslationContext context,
                                           TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;
            result.PrepareForManualPush(context);
            var sw = new SwitchStatement();
            context.TranslatedNode.CollectSymbol(sw);
            context.PushTranslated(sw);
            if (node.Sections != null)
            {
                foreach (SwitchSectionSyntax source in node.Sections.Reverse())
                {
                    context.TranslationStack.Push(source);
                }
            }
            context.TranslationStack.Push(SyntaxFactory.ParenthesizedExpression(node.Expression));
        }

        public override void OnAfterChildTraversal(TranslationContext context, SwitchStatementSyntax originalNode)
        {
            base.OnAfterChildTraversal(context, originalNode);
            context.PopTranslated();
        }
    }
}