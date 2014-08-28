using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.JavascriptCodeDom.Statements;
using Stratageme15.Core.Transaltion;
using Stratageme15.Core.Transaltion.Reactors;
using Stratageme15.Core.Transaltion.TranslationContexts;

namespace Stratageme15.Reactors.Basic.Statements.Switch
{
    public class SwitchLabelSyntaxReactor : ReactorBase<SwitchLabelSyntax>
    {
        protected override void HandleNode(SwitchLabelSyntax node, TranslationContext context, TranslationResult result)
        {
            result.Strategy = TranslationStrategy.TraverseChildrenAndNotifyMe;
            SyntaxTreeNodeBase snb = null;
            if (node.CaseOrDefaultKeyword.CSharpKind() == SyntaxKind.CaseKeyword)
            {
                snb = new CaseClause();
            }
            else
            {
                snb = new DefaultClause();
            }
            context.TranslatedNode.CollectSymbol(snb);
            context.PushTranslated(snb);

            var parent = (SwitchSectionSyntax) node.Parent;
            if (node == parent.Labels.Last())
            {
                result.PrepareForManualPush(context);
                foreach (StatementSyntax statement in parent.Statements.Reverse())
                {
                    context.TranslationStack.Push(statement);
                }
                if (node.Value != null) context.TranslationStack.Push(node.Value);
            }
        }

        public override void OnAfterChildTraversal(TranslationContext context, SwitchLabelSyntax originalNode)
        {
            base.OnAfterChildTraversal(context, originalNode);
            context.PopTranslated();
        }
    }
}