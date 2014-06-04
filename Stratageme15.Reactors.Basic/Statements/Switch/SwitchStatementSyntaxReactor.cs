using System.Linq;
using Roslyn.Compilers.CSharp;
using Stratageme15.Core.JavascriptCodeDom.Statements;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.Reactors;
using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Statements.Switch
{
    public class SwitchStatementSyntaxReactor : ReactorBase<SwitchStatementSyntax>
    {
        protected override void HandleNode(SwitchStatementSyntax node, TranslationContext context, TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;
            result.PrepareForManualPush(context);
            SwitchStatement sw = new SwitchStatement();
            context.TranslatedNode.CollectSymbol(sw);
            context.PushTranslated(sw);
            if (node.Sections!=null)
            {
                foreach (var source in node.Sections.Reverse())
                {
                    context.TranslationStack.Push(source);
                }
            }
            context.TranslationStack.Push(Syntax.ParenthesizedExpression(node.Expression));
        }

        public override void OnAfterChildTraversal(TranslationContext context, SwitchStatementSyntax originalNode)
        {
            base.OnAfterChildTraversal(context, originalNode);
            context.PopTranslated();
        }
    }
}
