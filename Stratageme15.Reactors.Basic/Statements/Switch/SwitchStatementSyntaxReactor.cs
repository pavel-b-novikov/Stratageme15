using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom.Statements;
using Stratageme15.Core.Translation;
using Stratageme15.Core.Translation.Reactors;
using Stratageme15.Core.Translation.TranslationContexts;
using Stratageme15.Reactors.Basic.Utility;

namespace Stratageme15.Reactors.Basic.Statements.Switch
{
    public class SwitchStatementSyntaxReactor : BasicReactorBase<SwitchStatementSyntax>
    {
        protected override void HandleNode(SwitchStatementSyntax node, TranslationContextWrapper context,
                                           TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;
            result.PrepareForManualPush(context.Context);
            var sw = new SwitchStatement();
            context.Context.TargetNode.CollectSymbol(sw);
            context.Context.PushTranslated(sw);
            if (node.Sections != null)
            {
                foreach (SwitchSectionSyntax source in node.Sections.Reverse())
                {
                    context.Context.TranslationStack.Push(source);
                }
            }
            context.Context.TranslationStack.Push(SyntaxFactory.ParenthesizedExpression(node.Expression));
        }

        public override void OnAfterChildTraversal(TranslationContextWrapper context, SwitchStatementSyntax originalNode)
        {
            base.OnAfterChildTraversal(context, originalNode);
            context.Context.PopTranslated();
        }
    }
}